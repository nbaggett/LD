using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAboutY : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        transform.Rotate(0, Speed * Time.deltaTime, 0, Space.World);
    }
}
