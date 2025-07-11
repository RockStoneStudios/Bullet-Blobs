using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletVFX;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackThrust = 19f;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;
    private Gun _gun;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // private void Start() {
    //     if (PlayerController.Instance.IsFacingRight()) {
    //         _fireDirection = Vector2.right;
    //     } else {
    //         _fireDirection = Vector2.left;
    //     }
    // }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = _fireDirection * _moveSpeed;
    }

    public void Init(Gun gun ,Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(_bulletVFX, transform.position, Quaternion.identity);
        Debug.Log(other.gameObject.name);
        
        IHitable iHitable = other.gameObject.GetComponent<IHitable>();
        iHitable?.TakeHit();

        
        IDamage iDamage = other.gameObject.GetComponent<IDamage>();
        iDamage?.TakeDamage(_damageAmount,_knockbackThrust);

       
        _gun.ReleaseBulletFromPool(this);
    }
}