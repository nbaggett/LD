using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLean : MonoBehaviour
{
    public float rotationAmount = 4f;
    public float maxRoationAmount = 5f;
    public float smoothRotation = 12f;
    private Quaternion _intialRotation;

    private void Awake()
    {
        _intialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        TiltSway();
    }

    private void TiltSway()
    {
        float InputX = -Input.GetAxis("Mouse X");
        float InputY = -Input.GetAxis("Mouse Y");
        float moveX = Mathf.Clamp(InputX * rotationAmount, -maxRoationAmount, maxRoationAmount);
        float moveY = Mathf.Clamp(InputY * rotationAmount, -maxRoationAmount, maxRoationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(moveY, moveX, 0));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * _intialRotation, Time.deltaTime * smoothRotation);
    }
}
