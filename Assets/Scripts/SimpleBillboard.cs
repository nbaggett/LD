using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBillboard : MonoBehaviour
{
    private Camera Camera;
    private void Awake()
    {
        Camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(transform.position + Camera.transform.rotation * Vector3.forward, Camera.transform.rotation * Vector3.up);
    }
}
