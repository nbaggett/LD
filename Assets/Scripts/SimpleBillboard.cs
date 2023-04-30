using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBillboard : MonoBehaviour
{
    private Camera Camera;
    private void Start()
    {
        Camera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = Camera.transform.rotation;
    }
}
