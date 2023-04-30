using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeightFollow : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, _camera.transform.position.y - 2, transform.position.z);
    }
}
