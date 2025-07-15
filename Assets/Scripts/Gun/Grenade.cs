using Unity.Cinemachine;
using System;
using UnityEngine;
using System.Collections;


public class Grenade : MonoBehaviour
{
    public Action OnExplode;
    public Action OnBeep;
    
    [SerializeField] private GameObject _explodeVFX;
    [SerializeField] private GameObject _grenadeLight;
    [SerializeField] private float _launchForce = 14f;
    [SerializeField] private float _torqueAmount = 1.7f;
    [SerializeField] private float _explosionRadius = 3.6f;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private int _damageAmount = 3;
    [SerializeField] private float _lightBlinkTime = .16f;
    [SerializeField] private int _totalBlinks = 3;
    [SerializeField] private int _explodeTime = 3;

    private int _currentBlinks;
    private Rigidbody2D _rigidBody;
    private CinemachineImpulseSource _impulseSource;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void OnEnable()
    {
        OnExplode += Explosion;
        OnExplode += GrenadeScreenShake;
        OnExplode += DamageNearby;
        OnBeep += BlinkLight;
        OnExplode += AudioManager.Instance.Grenade_OnExplode;
        OnBeep += AudioManager.Instance.Grenade_OnBeep;
    }

    void OnDisable()
    {
        OnExplode -= Explosion;
        OnExplode -= GrenadeScreenShake;
        OnExplode -= DamageNearby;
        OnBeep -= BlinkLight;
        OnExplode -= AudioManager.Instance.Grenade_OnExplode;
        OnBeep -= AudioManager.Instance.Grenade_OnBeep;
    }

    void Start()
    {
        LaunchGrenade();
        StartCoroutine(CountdownExplodeRoutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            OnExplode?.Invoke();
        }
    }

    private void Explosion()
    {
        Instantiate(_explodeVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void LaunchGrenade()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePos - (Vector2)transform.position).normalized;
        _rigidBody.AddForce(directionToMouse * _launchForce, ForceMode2D.Impulse);
        _rigidBody.AddTorque(_torqueAmount, ForceMode2D.Impulse);

    }

    private void GrenadeScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void DamageNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);
        foreach (Collider2D hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            health?.TakeDamage(_damageAmount);
        }
    }

    private IEnumerator CountdownExplodeRoutine()
    {
        while (_currentBlinks < _totalBlinks)
        {
            yield return new WaitForSeconds(_explodeTime);
            OnBeep?.Invoke();
            yield return new WaitForSeconds(_lightBlinkTime);
            _grenadeLight.SetActive(false);
        }
        OnExplode?.Invoke();
    }

    private void BlinkLight()
    {
        _grenadeLight.SetActive(true);
        _currentBlinks++;
    }
}
