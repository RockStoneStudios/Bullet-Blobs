using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Cinemachine;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = .5f;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _muzzleFlashTimer = .05f;

    private Coroutine _muzzleFlashRoutine;
    private ObjectPool<Bullet> _bulletPool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;

    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Start()
    {
        CreateBulletPool();
    }

    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
    }


    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(_bulletPrefab);
        }, bullet =>
        {
            bullet.gameObject.SetActive(true);
        },
            bullet =>
            {
                bullet.gameObject.SetActive(false);
            },
            bullet =>
            {
                Destroy(bullet);
            }, false
        );
    }



    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime)
        {
            OnShoot?.Invoke();
            // animation
            // sfx
            // screen shake
            // muzzle flash
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);

    }

    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCD;
    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Vector2 direction = _mousePos - (Vector2)PlayerController.Instance.transform.position;
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void MuzzleFlash()
    {
        if (_muzzleFlashRoutine != null)
        {
            StopCoroutine(_muzzleFlashRoutine);
        }

        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTimer);
        _muzzleFlash.SetActive(false);
    }
    
}
