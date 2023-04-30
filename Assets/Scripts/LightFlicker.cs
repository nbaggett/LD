using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light _light;
    private BoolTimer _flickerTimer;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        if (!_flickerTimer)
        {
            _light.intensity = Random.Range(0.5f, 1f);
            _flickerTimer.Set(Random.Range(0.2f, 0.3f));
        }
    }
}
