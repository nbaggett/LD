using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public Transform SoulCollectTransform;
    public List<OrbGate> gates = new List<OrbGate>();

    private float remainingFocus = 1f;
    private float maxFocusTime = 2f;
    private float focusTimer = 0f;
    private BoolTimer focusResetTimer;

    public Vector3 LastCheckpointPosition = default;
    public Transform PlayerTransform;
    public PlayerCharacterController PlayerCharacterController;

    private void Awake()
    {
        LastCheckpointPosition = PlayerTransform.position;
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
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

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;

    }

    private void LateUpdate()
    {


        if (Input.GetMouseButton(1) && focusTimer < maxFocusTime && !focusResetTimer)
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.4f, 0.1f).SetUpdate(true);
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
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.1f).SetUpdate(true);
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
    }

    public void ResetPlayer()
    {
        Debug.Log("Resetting player");
        StartCoroutine(ResetPlayerCoroutine());
    }

    private IEnumerator ResetPlayerCoroutine()
    {
        yield return FadeCanvasGroup.DOFade(1f, 0.5f).WaitForCompletion();
        PlayerCharacterController.Motor.SetPosition(LastCheckpointPosition);
        FadeCanvasGroup.DOFade(0f, 0.5f);
    }

    public void SetCheckpoint(Transform t)
    {
        LastCheckpointPosition = t.position;
    }
}
