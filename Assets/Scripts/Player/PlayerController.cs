using UnityEngine;
using System;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public static Action OnJump;
    public static Action OnJetpack;
    public static PlayerController Instance;

    [SerializeField] private TrailRenderer _jetPackTrailerRender;
    public Vector2 MoveInput => _frameInput.Move;
    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = .2f;
    [SerializeField] private float _coyoteTime = .5f;
    [SerializeField] private float _jetPackTime = .6f;
    [SerializeField] private float _jetpackStrenght = 11.2f;

    private Coroutine _jetPackCourutine;
    private float _timeInAir;
    private float _coyoteTimer;
    private bool _doubleJumpAvailable;

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

    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
        OnJetpack += StartJetPack;
    }

    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
        OnJetpack -= StartJetPack;
    }

    private void Update()
    {
        GatherInput();
        Movement();
        CoyoteTimer();
        HandleJump();
        HandleSpriteFlip();
        GravityDelay();
        JetPack();
    }

    private void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            _coyoteTimer = _coyoteTime;
            _doubleJumpAvailable = true;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
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

    public bool CheckGrounded()
    {
        Collider2D isGround = Physics2D.OverlapBox(_feetTransform.position, _groundCheck, 0, _groundLayer);
        return isGround;
    }
    public float GetCurrentMoveX()
    {
        return _frameInput.Move.x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck);
    }

    private void GravityDelay()
    {
        if (!CheckGrounded())
        {
            _timeInAir += Time.deltaTime;
        }
        else
        {
            _timeInAir = 0f;
        }
    }

    private void ExtraGravity()
    {
        if (_timeInAir > _gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
        }
    }


    private void GatherInput()
    {
        //     float moveX = Input.GetAxis("Horizontal");
        _frameInput = _playerInput.FrameInput;


    }

    private void Movement()
    {

        _movement.SetCurrentDir(_frameInput.Move.x);
    }

    private void HandleJump()
    {
        if (!_frameInput.Jump)
        {
            return;
        }
        if (CheckGrounded())
        {

            OnJump?.Invoke();
        }
        else if (_coyoteTimer > 0f)
        {
            OnJump?.Invoke();
        }
        else if (_doubleJumpAvailable)
        {
            _doubleJumpAvailable = false;
            OnJump?.Invoke();
        }

    }

    private void ApplyJumpForce()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        _timeInAir = 0f;
        _coyoteTimer = 0f;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
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

    private void JetPack()
    {
        if (!_frameInput.Jetpack || _jetPackCourutine != null) return;
        OnJetpack?.Invoke();
    }

    private void StartJetPack()
    {
        _jetPackTrailerRender.emitting = true;
        _jetPackCourutine = StartCoroutine(JetpackRoutine());
    }

    private IEnumerator JetpackRoutine()
    {
        float jetTime = 0f;
        while (jetTime < _jetPackTime)
        {
            jetTime += Time.deltaTime;
            _rigidBody.linearVelocity = Vector2.up * _jetpackStrenght;
            yield return null;
        }
        _jetPackTrailerRender.emitting = false;
        _jetPackCourutine = null;

    }
}
