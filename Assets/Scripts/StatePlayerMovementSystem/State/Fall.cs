using Cysharp.Threading.Tasks;
using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public class Fall : BaseState
  {
    private Vector3? _horizontalDirection = null;
    private float _gravity = 9.8f;
    private float _verticalVelocity = 0f;
    
    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto)
    {
      if (_horizontalDirection == null)
      {
        _horizontalDirection = direction;
        Gravity();
      }
      
      float horizontalMovementSpeed = playerDto.GetData<float>("MovementSpeed");
      return new Vector3(horizontalMovementSpeed * _horizontalDirection.Value.x, _verticalVelocity,
          horizontalMovementSpeed * _horizontalDirection.Value.z);
    }

    private async void Gravity()
    {
      while (!EndPointReached)
      {
        _verticalVelocity -= _gravity * Time.fixedDeltaTime;
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }
  }
}