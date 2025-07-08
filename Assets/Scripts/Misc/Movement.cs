using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    private float _moveX;
    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetCurrentDir(float _currentDirection)
    {
        _moveX = _currentDirection;
    }

    private void Move()
    {
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidBody.linearVelocity.y);
        _rigidBody.linearVelocity = movement;

    }
}
