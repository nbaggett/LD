using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchCorrector : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        audioSource.pitch = Time.timeScale;
    }
}
