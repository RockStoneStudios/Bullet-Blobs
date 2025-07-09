using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustVFX;
    [SerializeField] private float _tileAngle = 20f;
    [SerializeField] private float _tilSpeed = 5f;
    [SerializeField] private Transform _characterSprietTransform;
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
        _characterSprietTransform.rotation = Quaternion.Lerp(currentCharcaterRotation, targetCharacterRotation,_tilSpeed* Time.deltaTime);
    }
}
