using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTrail : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Vector3 start, Vector3 end)
    {
        _lineRenderer.material.SetFloat("_FadeAmount", 0f);

        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);

        StartCoroutine(Fade());

    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.5f);
        _lineRenderer.material.DOFloat(1, "_FadeAmount", 1f);
    }
}
