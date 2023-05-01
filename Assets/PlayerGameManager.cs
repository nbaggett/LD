using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerGameManager : MonoBehaviour
{
    private static PlayerGameManager _instance;

    public static PlayerGameManager Instance { get { return _instance; } }

    public Image FocusImage;
    public Volume FocusVolume;
    public SoulCounterUI SoulCounterUI;
    public TMPro.TextMeshProUGUI GunText;
    public CanvasGroup FadeCanvasGroup;

    private int numSoulOrbsCollected = 0;

    private float totalTime = 0f;

    public Transform SoulCollectTransform;
    public List<OrbGate> gates = new List<OrbGate>();

    private float remainingFocus = 1f;
    private float maxFocusTime = 2f;
    private float focusTimer = 0f;
    private BoolTimer focusResetTimer;

    public Vector3 LastCheckpointPosition = default;
    public Vector3 LastCheckpointRotation = default;
    public Transform PlayerTransform;
    public PlayerCharacterController PlayerCharacterController;

    public GameObject VictoryScreen;
    public TextMeshProUGUI TimeElapsedText;

    private bool _introCinematicComplete = false;
    public bool IntroCinematicComplete { get { return _introCinematicComplete; } }

    [Header("Intro Cinematic")]
    public DissolveText TakeThisWeapon;
    public DissolveText DeliverSoulsToMe;
    public Image Crosshair;
    public Volume IntroVolume;
    public Transform GunHolder;
    public Transform GunStartingTransform;
    public Transform GunMiddleTransform;
    public Transform GunEndingTransform;
    private Vector3 _gunEndingPosition;
    public Player Player;
    public DissolveText TurnAroundText;

    public float TargetTimeScale = 1f;

    private void Awake()
    {
        LastCheckpointPosition = PlayerTransform.position;
        LastCheckpointRotation = PlayerTransform.rotation.eulerAngles;
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void Start()
    {
        Crosshair.enabled = false;
        GunHolder.transform.position = GunStartingTransform.position;
        GunHolder.transform.rotation = GunStartingTransform.rotation;
        SetGameLayerRecursive(GunHolder.gameObject, LayerMask.NameToLayer("Default"));
    }

    public void DoCinematic()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(IntroCinematic());
    }

    private IEnumerator IntroCinematic()
    {

        if (FadeCanvasGroup.alpha == 1)
        {
            yield return FadeCanvasGroup.DOFade(0, 1f).WaitForCompletion();
        }

        yield return new WaitForSeconds(1f);
        GunHolder.DORotate(GunMiddleTransform.rotation.eulerAngles, 2f);
        yield return GunHolder.DOMove(GunMiddleTransform.position, 3f).WaitForCompletion();

        // blah blah blah do intro text here
        TakeThisWeapon.ShowText();
        yield return new WaitForSeconds(3f);
        TakeThisWeapon.HideText();

        // return gun to hand
        GunHolder.DORotate(GunEndingTransform.rotation.eulerAngles, 2f);
        yield return GunHolder.DOMove(GunEndingTransform.position, 3f).WaitForCompletion();

        DeliverSoulsToMe.ShowText();
        yield return new WaitForSeconds(3f);
        DeliverSoulsToMe.HideText();

        yield return DOTween.To(() => IntroVolume.weight, x => IntroVolume.weight = x, 0f, 1.5f).SetUpdate(true).WaitForCompletion();

        TurnAroundText.ShowText();
        _introCinematicComplete = true;
        Player.CanMove = true;
        Crosshair.enabled = true;
        SetGameLayerRecursive(GunHolder.gameObject, LayerMask.NameToLayer("FirstPersonOverlay"));

        yield return new WaitForSeconds(3f);
        TurnAroundText.HideText();
    }

    public void CollectSoulOrb()
    {
        numSoulOrbsCollected++;
        // update UI
        SoulCounterUI.ShowSoulCount(numSoulOrbsCollected);
        // update gun UI
        GunText.text = SoulCounterUI.ToRoman(numSoulOrbsCollected);

        // update gates
        foreach (OrbGate gate in gates)
        {
            gate.TryUnlock(numSoulOrbsCollected);
        }
    }

    public int GetNumSoulOrbsCollected()
    {
        return numSoulOrbsCollected;
    }

    private void LateUpdate()
    {
        totalTime += Time.unscaledDeltaTime;

        if (Input.GetMouseButton(1) && focusTimer < maxFocusTime && !focusResetTimer && _introCinematicComplete)
        {
            DOTween.To(() => TargetTimeScale, x => TargetTimeScale = x, 0.4f, 0.1f).SetUpdate(true);
            DOTween.To(() => FocusVolume.weight, x => FocusVolume.weight = x, 1.0f, 0.25f).SetUpdate(true);
            FocusImage.enabled = true;
            focusTimer += Time.unscaledDeltaTime;
            focusTimer = Mathf.Clamp(focusTimer, 0f, maxFocusTime);
            FocusImage.fillAmount = Mathf.Clamp01(1 - (focusTimer / maxFocusTime));

            if (focusTimer >= maxFocusTime)
            {
                focusResetTimer.Set(maxFocusTime);
            }
        }
        else
        {
            DOTween.To(() => TargetTimeScale, x => TargetTimeScale = x, 1f, 0.1f).SetUpdate(true);
            DOTween.To(() => FocusVolume.weight, x => FocusVolume.weight = x, 0.0f, 0.25f).SetUpdate(true);

            if (focusTimer > 0f)
            {
                focusTimer -= Time.unscaledDeltaTime;
                focusTimer = Mathf.Clamp(focusTimer, 0f, maxFocusTime);
                FocusImage.fillAmount = Mathf.Clamp01(1 - (focusTimer / maxFocusTime));
            }
            if (focusTimer <= 0f)
            {
                FocusImage.enabled = false;
            }
        }

        if (focusResetTimer)
        {
            Color c = FocusImage.color;
            c.a = 0.25f;
            FocusImage.color = c;
        }
        else
        {
            Color c = FocusImage.color;
            c.a = 1f;
            FocusImage.color = c;
        }


        if (!PauseMenu.IsPaused)
        {
            Time.timeScale = TargetTimeScale;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void ResetPlayer()
    {
        StartCoroutine(ResetPlayerCoroutine());
    }

    private IEnumerator ResetPlayerCoroutine()
    {
        yield return FadeCanvasGroup.DOFade(1f, 0.5f).WaitForCompletion();
        PlayerCharacterController.Motor.SetPosition(LastCheckpointPosition);
        PlayerCharacterController.Motor.SetRotation(Quaternion.Euler(LastCheckpointRotation));
        FadeCanvasGroup.DOFade(0f, 0.5f);
    }

    public void SetCheckpoint(Transform t)
    {
        LastCheckpointPosition = t.position;
        LastCheckpointRotation = t.rotation.eulerAngles;
    }

    private void SetGameLayerRecursive(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, _layer);

        }
    }

    public void ShowVictoryScreen()
    {
        VictoryScreen.SetActive(true);
        int intTime = (int)totalTime;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = totalTime * 1000;
        fraction = (fraction % 1000);
        string timeText = String.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        TimeElapsedText.text = "Time: " + timeText;
        Time.timeScale = 0f;
        PauseMenu.IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        SceneManager.Instance.ReloadScene();
    }
}
