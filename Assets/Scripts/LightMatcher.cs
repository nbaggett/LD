using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightMatcher : MonoBehaviour
{
    public Light pointLight;

    private Light _myPointLight;

    private void Awake()
    {
        _myPointLight = GetComponent<Light>();
    }

    private void LateUpdate()
    {
        _myPointLight.range = pointLight.range;
        _myPointLight.intensity = pointLight.intensity;
    }
}
