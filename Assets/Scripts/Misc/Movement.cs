using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    private float _moveX;
    private bool _canMove = true;
    private Rigidbody2D _rigidBody;
    private KnockBack _knockback;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<KnockBack>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnEnable()
    {
        _knockback.OnKnockbackStart += CanMoveFalse;
        _knockback.OnKnockbackEnd += CanMoveTrue;
    }

    private  void OnDisable()
    {
         _knockback.OnKnockbackStart -= CanMoveFalse;
        _knockback.OnKnockbackEnd -= CanMoveTrue;
    }

    public void SetCurrentDir(float _currentDirection)
    {
        _moveX = _currentDirection;
    }

    private void CanMoveTrue()
    {
        _canMove = true;
    }

    private void CanMoveFalse()
    {
        _canMove = false;
    }



    private void Move()
    {
        if (!_canMove) return;
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidBody.linearVelocity.y);
        _rigidBody.linearVelocity = movement;

    }
}
