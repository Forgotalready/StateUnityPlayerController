using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class JumpController : MonoBehaviour
{
  [SerializeField] private Transform _groundCheck;
  [SerializeField] private LayerMask _groundMask;
  [SerializeField] private float _gravity = 9.8f;
  [SerializeField] private float _jumpHeight = 1f;

  public ReactiveProperty<bool> IsJumped { get; } = new(false);
  public ReactiveProperty<bool> IsFalling { get; } = new(false);

  private float _verticalVelocity;
  private Vector3 _velocity;

  private void FixedUpdate()
  {
    if (!IsJumped.Value && !IsGrounded())
    {
      IsFalling.Value = true;
    }
  }

  public void Jump(Vector3 velocity, CharacterController characterController)
  {
    if (!IsJumped.Value && IsGrounded())
    {
      IsJumped.Value = true;
      _velocity = velocity;
      JumpCoroutine(characterController);
    }
  }

  private async void JumpCoroutine(CharacterController characterController)
  {
    float initialVelocity = Mathf.Sqrt(2 * _gravity * _jumpHeight);
    _verticalVelocity = initialVelocity;

    while (_verticalVelocity > 0 || !IsGrounded())
    {
      _verticalVelocity -= _gravity * Time.fixedDeltaTime;
      var move = new Vector3(_velocity.x, _verticalVelocity, _velocity.z);
      characterController.Move(move * Time.fixedDeltaTime);
      await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
    }
    
    IsJumped.Value = false;
  }
  
  public async void Fall(Vector3 velocity, CharacterController characterController)
  {
    _velocity = velocity;
    _verticalVelocity = 0.0f;
    while (!IsGrounded())
    {
      _verticalVelocity -= _gravity * Time.fixedDeltaTime;
      var move = new Vector3(_velocity.x, _verticalVelocity, _velocity.z);
      characterController.Move(move * Time.fixedDeltaTime);
      await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
    }

    IsFalling.Value = false;
  }
  
  private bool IsGrounded()
  {
    return Physics.CheckSphere(_groundCheck.position, 0.1f, _groundMask);
  }
}
