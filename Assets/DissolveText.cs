using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DissolveText : MonoBehaviour
{
    public Texture2D wordTexture;
    public bool showOnce = true;

    private bool _hasShown = false;
    private bool _hasHided = false;

    private Tween _tween;
    private Material _material;

    public AudioClip Showclip;
    public AudioClip Hideclip;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _material = GetComponent<MeshRenderer>().material;
        _material.SetTexture("_WordTex", wordTexture);
        _material.SetFloat("_Dissolve", 1);
    }

    public void ShowText()
    {
        if (showOnce && _hasShown)
        {
            return;
        }
        _hasShown = true;
        _material.SetFloat("_Dissolve", 1);
        _tween?.Kill();
        _tween = _material.DOFloat(0, "_Dissolve", 0.5f);
        _audioSource.PlayOneShot(Showclip);
    }

    public void HideText()
    {
        if (showOnce && _hasHided)
        {
            return;
        }
        _hasHided = true;
        _tween?.Kill();
        _tween = _material.DOFloat(1, "_Dissolve", 0.5f);
        _audioSource.PlayOneShot(Hideclip);
    }
}
