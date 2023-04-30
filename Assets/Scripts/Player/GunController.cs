using UnityEngine;
using DG.Tweening;
using System.Collections;

public class GunController : MonoBehaviour
{
    private Animator _animator;

    public float shootInterval = 2f;

    public Transform MuzzleTransform;
    public BulletTrail BulletTrailPrefab;
    public TrailRenderer SmokeTrail;

    public PlayerCharacterController PlayerCharacterController;
    public MuzzleFlash muzzleFlash;
    public AudioClip ShootAudioClip;
    public AudioClip ReloadAudioClip;

    public GameObject HitFXPrefab;

    private BoolTimer _shootTimer;

    private Camera _camera;
    public AudioSource _audioSource;

    private BoolTimer _smokeTrailTimer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        PlayerCharacterController.OnLeaveGround.AddListener(Event_OnLeaveGround);
        PlayerCharacterController.OnLand.AddListener(Event_OnLand);
        _camera = Camera.main;
    }

    private void Event_OnLand()
    {
        Vector3 targetRotation = new Vector3(15, 0, 0);
        transform.DOKill();
        transform.DOLocalRotate(targetRotation, 0.1f).OnComplete(() => transform.DOLocalRotate(Vector3.zero, 0.1f));
    }

    private void Event_OnLeaveGround()
    {
        Vector3 targetRotation = new Vector3(-60, 0, 0);
        transform.DOKill();
        transform.DOLocalRotate(targetRotation, 3f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_shootTimer)
        {
            Shoot();
            _shootTimer.Set(shootInterval);
        }

        if (Input.GetMouseButton(1))
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.4f, 0.1f).SetUpdate(true);
        }
        else
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.1f).SetUpdate(true);
        }

        _audioSource.pitch = Time.timeScale;

        if (!_smokeTrailTimer)
        {
            SmokeTrail.emitting = false;
        }
    }

    public void Shoot()
    {
        var BulletTrail = Instantiate(BulletTrailPrefab, MuzzleTransform.position, Quaternion.identity);

        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity))
        {
            BulletTrail.Initialize(MuzzleTransform.position, hit.point);
            Debug.DrawLine(MuzzleTransform.position, hit.point, Color.red, 5f);

            var hitFX = Instantiate(HitFXPrefab, hit.point, Quaternion.FromToRotation(transform.up, hit.normal));
            Destroy(hitFX, 5f);

            if (hit.collider.gameObject.TryGetComponent(out Breakable breakable))
            {
                breakable.Break();
            }
        }
        else
        {
            Vector3 desiredEndpoint = _camera.transform.position + _camera.transform.forward * 100f;
            Debug.DrawRay(MuzzleTransform.position, desiredEndpoint, Color.red, 5f);
            BulletTrail.Initialize(MuzzleTransform.position, desiredEndpoint);
        }

        _audioSource.clip = ShootAudioClip;
        _audioSource.Play();
        StartCoroutine(PlayReloadSound());

        SmokeTrail.emitting = true;
        _smokeTrailTimer.Set(1f);

        Destroy(BulletTrail.gameObject, 5f);

        muzzleFlash.DoMuzzleFlash();
        _camera.DOShakeRotation(0.2f, 5f, 10, 0, false);

        _animator.ResetTrigger("Shoot");
        _animator.SetTrigger("Shoot");
    }

    private IEnumerator PlayReloadSound()
    {
        yield return new WaitForSeconds(0.35f);
        _audioSource.clip = ReloadAudioClip;
        _audioSource.Play();
    }
}
