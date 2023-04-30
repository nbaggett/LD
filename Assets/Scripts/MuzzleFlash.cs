using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MuzzleFlash : MonoBehaviour
{
    public MeshRenderer mainFlashRenderer;
    public MeshRenderer secondaryFlashRenderer;
    public ParticleSystem muzzleFlashParticles;

    public void DoMuzzleFlash()
    {
        StopCoroutine(Flash());
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        mainFlashRenderer.enabled = true;
        secondaryFlashRenderer.enabled = true;

        mainFlashRenderer.material.SetFloat("_Opacity", 1);
        secondaryFlashRenderer.material.SetFloat("_Opacity", 1);

        muzzleFlashParticles.Play();

        secondaryFlashRenderer.transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 360f)));

        yield return new WaitForSeconds(0.05f);

        mainFlashRenderer.material.DOFloat(0, "_Opacity", 0.05f);
        secondaryFlashRenderer.material.DOFloat(0, "_Opacity", 0.05f);

        yield return new WaitForSeconds(0.05f);

        mainFlashRenderer.enabled = false;
        secondaryFlashRenderer.enabled = false;
    }
}
