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

    [Header("Head Bob")]
    public bool EnableHeadBob = true;
    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float _frequency = 10.0f;
    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    public Transform HeadBobTransform;

    private Tween _landTween;

    public AudioClip FootstepClip;
    private AudioSource _audioSource;
    private BoolTimer _footstepTimer;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerCharacterController.OnJump.AddListener(Event_OnJump);
        _playerCharacterController.OnLand.AddListener(Event_OnLand);
        _startPos = HeadBobTransform.localPosition;
    }
    public void SetInputs(PlayerCharacterInputs playerCharacterInputs)
    {
        _playerCharacterInputs = playerCharacterInputs;
    }

    private void LateUpdate()
    {
        if (!PlayerGameManager.Instance.IntroCinematicComplete || PauseMenu.IsPaused) return;

        Vector2 moveInput = new Vector2(_playerCharacterInputs.MoveAxisRight, _playerCharacterInputs.MoveAxisForward);
        moveInput.y = Mathf.Clamp(moveInput.y, -1f, 0f);
        transform.DOLocalRotate(new Vector3(moveInput.y * LeanAmount, 0, -moveInput.x * LeanAmount), LeanSpeed).SetEase(LeanEase);

        if (EnableHeadBob)
        {
            CheckMotion();
            ResetPosition();
        }
    }

    private void Event_OnLand()
    {
        if (!PlayerGameManager.Instance.IntroCinematicComplete || PauseMenu.IsPaused) return;
        Vector3 targetPosition = new Vector3(0, -0.3f, 0);
        _landTween = transform.DOLocalMove(targetPosition, 0.1f).OnComplete(() => transform.DOLocalMove(Vector3.zero, 0.2f));
    }

    private void Event_OnJump()
    {

    }

    private Vector3 FootstepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Sin(Time.time * _frequency) * _amplitude;

        if (!_footstepTimer)
        {
            _footstepTimer.Set(0.25f);
            _audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(FootstepClip);
        }
        //pos.x = Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }

    private void CheckMotion()
    {
        Vector3 velocity = _playerCharacterController.Motor.Velocity;
        float speed = new Vector3(velocity.x, 0, velocity.z).magnitude;
        if (speed < _toggleSpeed) return;
        if (!_playerCharacterController.Motor.GroundingStatus.FoundAnyGround) return;

        PlayMotion(FootstepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
        HeadBobTransform.localPosition += motion;
    }

    private void ResetPosition()
    {
        if (HeadBobTransform.localPosition == _startPos) return;
        HeadBobTransform.localPosition = Vector3.Lerp(HeadBobTransform.localPosition, _startPos, Time.deltaTime);
    }
}
