using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    // singleton
    private static MainMenu _instance;
    public static MainMenu Instance { get { return _instance; } }

    public Transform SoulReaper;

    public CanvasGroup FadeCanvasGroup;
    public CanvasGroup MainMenuCanvasGroup;
    private float startingScale;

    public AudioClip ButtonPressed;
    public AudioClip ButtonMouseOver;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        startingScale = SoulReaper.localScale.x;
        SoulReaper.localScale = new Vector3(0, SoulReaper.localScale.y, SoulReaper.localScale.z);
        StartCoroutine(ShowMenu());
    }

    private IEnumerator ShowMenu()
    {
        FadeCanvasGroup.alpha = 1;
        yield return new WaitForSeconds(1f);
        yield return FadeCanvasGroup.DOFade(0, 1.5f).WaitForCompletion();

        yield return new WaitForSeconds(1f);
        SoulReaper.DOScaleX(startingScale, 1f).SetEase(Ease.OutBack);
    }

    public void OnGameStartPressed()
    {
        _audioSource.PlayOneShot(ButtonPressed);
        MainMenuCanvasGroup.DOFade(0, 0.5f);
        PlayerGameManager.Instance.DoCinematic();
    }

    public void OnGameQuitPressed()
    {
        Application.Quit();
    }

    public void OnButtonMouseOver()
    {
        _audioSource.PlayOneShot(ButtonMouseOver);
    }
}
