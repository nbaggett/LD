using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class SoulCounterUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private TextMeshProUGUI _textMeshProUGUI;
    private AudioSource _audioSource;
    private Tween _tween;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void ShowSoulCount(int count)
    {
        _textMeshProUGUI.text = ToRoman(count);
        _canvasGroup.alpha = 1;
        _tween?.Kill();
        _tween = _canvasGroup.DOFade(0, 1f);
        _audioSource.Play();
    }

    private void Update()
    {
        _audioSource.pitch = Time.timeScale * (0.7f + (0.05f * PlayerGameManager.Instance.GetNumSoulOrbsCollected()));
    }

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new ArgumentOutOfRangeException("Impossible state reached");
    }
}
