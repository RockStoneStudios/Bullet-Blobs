using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = .2f;

    private float _timeInAir;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;

    private bool _isGrounded = false;
    private Movement _movement;

    private Rigidbody2D _rigidBody;

    public void Awake()
    {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        GatherInput();
        Movement();
        Jump();
        HandleSpriteFlip();
        GravityDelay();
    }

     private void FixedUpdate() {
        ExtraGravity();
    }



  

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = false;
        }
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private bool CheckGrounded()
    {
        Collider2D isGround = Physics2D.OverlapBox(_feetTransform.position, _groundCheck, 0, _groundLayer);
        return isGround;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck);
    }
     
      private void GravityDelay() {
        if (!CheckGrounded()) {
            _timeInAir += Time.deltaTime;
        } else {
            _timeInAir = 0f;
        }
    }

    private void ExtraGravity() {
        if (_timeInAir > _gravityDelay) {
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
        }
    }


    private void GatherInput()
    {
        //     float moveX = Input.GetAxis("Horizontal");
        _frameInput = _playerInput.FrameInput;


    }

    private void Movement() {

        _movement.SetCurrentDir(_frameInput.Move.x);
    }

    private void Jump()
    {
        if (!_frameInput.Jump)
        {
            return;
        }
        if (CheckGrounded())
        {
            _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
        }
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    } 
}
