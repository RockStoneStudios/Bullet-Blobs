using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustVFX;
    [SerializeField] private ParticleSystem _poofDustVFX;
    [SerializeField] private float _tileAngle = 20f;
    [SerializeField] private float _tilSpeed = 5f;
    [SerializeField] private Transform _characterSprietTransform;
    [SerializeField] private Transform _cowboyHatTransform;
    [SerializeField] private float _cowboyBoyHatTiltModifer = 2f;
    [SerializeField] private float yLandVelocityCheck = -10f;

    private Vector2 _velocityBeforePhysicsUpdate;
    private Rigidbody2D _rigidBody;
    private CinemachineImpulseSource _impulseSource;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        DetectMoveDust();
        ApplyTilt();
    }

    private void DetectMoveDust()
    {

        bool isGrounded = PlayerController.Instance.CheckGrounded();
        float horizontalInput = PlayerController.Instance.GetCurrentMoveX();

        if (isGrounded && Mathf.Abs(horizontalInput) > 0.01f)
        {
            if (!_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Play();
            }
        }
        else
        {
            if (_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Stop();
            }
        }
    }

    void OnEnable()
    {
        PlayerController.OnJump += PlayPoofDustVFX;
    }

    void OnDisable()
    {
        PlayerController.OnJump -= PlayPoofDustVFX;
    }

    private void FixedUpdate()
    {
        _velocityBeforePhysicsUpdate = _rigidBody.linearVelocity;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_velocityBeforePhysicsUpdate.y < yLandVelocityCheck)
        {
            PlayPoofDustVFX();
            _impulseSource.GenerateImpulse();
        }
    }

    private void PlayPoofDustVFX()
    {
        _poofDustVFX.Play();
    }

    private void ApplyTilt()
    {
        float targetAngle;
        if (PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAngle = _tileAngle;
        }
        else if (PlayerController.Instance.MoveInput.x > 0f)
        {
            targetAngle = -_tileAngle;
        }
        else
        {
            targetAngle = 0f;
        }

        Quaternion currentCharcaterRotation = _characterSprietTransform.rotation;
        quaternion targetCharacterRotation = Quaternion.Euler(currentCharcaterRotation.eulerAngles.x, currentCharcaterRotation.eulerAngles.y, targetAngle);
        _characterSprietTransform.rotation = Quaternion.Lerp(currentCharcaterRotation, targetCharacterRotation, _tilSpeed * Time.deltaTime);


        Quaternion currentHatRotation = _cowboyHatTransform.rotation;
        quaternion targetHatRotation = Quaternion.Euler(currentHatRotation.eulerAngles.x, currentCharcaterRotation.eulerAngles.y, -targetAngle / _cowboyBoyHatTiltModifer);
        _cowboyHatTransform.rotation = Quaternion.Lerp(currentHatRotation, targetHatRotation, _cowboyBoyHatTiltModifer * Time.deltaTime);
    }
}
