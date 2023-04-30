using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class OrbGate : MonoBehaviour
{
    private bool _locked = true;

    public int requiredSouls = 1;

    public Color UnlockedColor = Color.green;
    public TextMeshProUGUI text;
    public GameObject gate;
    public Light light;
    public AudioSource audioSource;
    public ParticleSystem particleSystem;

    private void Start()
    {
        if (PlayerGameManager.Instance)
        {
            PlayerGameManager.Instance.gates.Add(this);
        }
    }

    public void TryUnlock(int souls)
    {
        if (souls >= requiredSouls && _locked)
        {
            _locked = false;
            light.color = UnlockedColor;
            text.color = UnlockedColor;
            audioSource.Play();
            gate.transform.DOLocalMoveY(-3, 5f);
            particleSystem.Play();
        }
    }

}