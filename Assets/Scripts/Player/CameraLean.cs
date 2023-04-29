using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraLean : MonoBehaviour
{
    public float LeanSpeed = 0.1f;
    public float LeanAmount = 10f;
    public Ease LeanEase = Ease.OutBack;
    private PlayerCharacterInputs _playerCharacterInputs;
    [SerializeField] private PlayerCharacterController _playerCharacterController;

    private Tween _landTween;

    private void Awake()
    {
        _playerCharacterController.OnJump.AddListener(Event_OnJump);
        _playerCharacterController.OnLand.AddListener(Event_OnLand);
    }
    public void SetInputs(PlayerCharacterInputs playerCharacterInputs)
    {
        _playerCharacterInputs = playerCharacterInputs;
    }

    private void LateUpdate()
    {
        Vector2 moveInput = new Vector2(_playerCharacterInputs.MoveAxisRight, _playerCharacterInputs.MoveAxisForward);
        moveInput.y = Mathf.Clamp(moveInput.y, -1f, 0f);
        transform.DOLocalRotate(new Vector3(moveInput.y * LeanAmount, 0, -moveInput.x * LeanAmount), LeanSpeed).SetEase(LeanEase);
    }

    private void Event_OnLand()
    {
        Vector3 targetPosition = new Vector3(0, -0.3f, 0);
        _landTween = transform.DOLocalMove(targetPosition, 0.1f).OnComplete(() => transform.DOLocalMove(Vector3.zero, 0.2f));
    }

    private void Event_OnJump()
    {

    }

}
