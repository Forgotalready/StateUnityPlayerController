using Cysharp.Threading.Tasks;
using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public class Jump : BaseState
  {
    private Vector3? _horizontalDirection;
    private float _verticalVelocity;

    private float _jumpHeight = 1f;
    private float _gravity = 9.8f;

    public override void Enter(PlayerDTO playerDto)
    {
      base.Enter(playerDto);
      _horizontalDirection = null;
    }

    private async void Gravity()
    {
      while (!EndPointReached)
      {
        _verticalVelocity -= _gravity * Time.fixedDeltaTime;
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }

    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto)
    {
      if (_horizontalDirection == null)
      {
        _horizontalDirection = direction;
        float initialVelocity = Mathf.Sqrt(2 * _gravity * _jumpHeight);
        _verticalVelocity = initialVelocity;
        Gravity();
      }

      float horizontalMovementSpeed = playerDto.GetData<float>("MovementSpeed");
      return new Vector3(horizontalMovementSpeed * _horizontalDirection.Value.x, _verticalVelocity,
          horizontalMovementSpeed * _horizontalDirection.Value.z);
    }
  }
}