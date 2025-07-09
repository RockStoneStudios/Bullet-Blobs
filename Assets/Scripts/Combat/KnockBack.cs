using System;
using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;
    [SerializeField] private float _knockbackTime = .19f;
    private Rigidbody2D _rigidBody;
    private Vector3 _hitDirection;
    private float _knockbackThrust;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        OnKnockbackStart += ApplyKnockbackFoce;
        OnKnockbackEnd += StopKnockRoutine;
    }

    void OnDisable()
    {
         OnKnockbackStart -= ApplyKnockbackFoce;
        OnKnockbackEnd -= StopKnockRoutine;
    }

    public void GetKnockedBack(Vector3 hitDirection, float knockbacktrust)
    {
        _hitDirection = hitDirection;
        _knockbackThrust = knockbacktrust;
        OnKnockbackStart?.Invoke();
    }

    private void ApplyKnockbackFoce()
    {
        Vector3 difference = (transform.position - _hitDirection).normalized * _knockbackThrust * _rigidBody.mass;
        _rigidBody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(_knockbackTime);
        OnKnockbackEnd?.Invoke();

    }


    private void StopKnockRoutine() {
        _rigidBody.linearVelocity = Vector2.zero;
    }
}
