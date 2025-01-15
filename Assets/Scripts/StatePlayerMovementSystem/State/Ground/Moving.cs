using Cysharp.Threading.Tasks;
using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State.Ground
{
  public class Moving : BaseState
  {
    private float _maxMovementSpeed = 4.0f;
    private float _acceleration = 2.0f;
    private float _movementSpeed;
    
    public override void Enter(PlayerDTO playerDto)
    {
      base.Enter(playerDto);
      LerpSpeed(playerDto);
    }

    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto) => playerDto.GetData<float>("MovementSpeed") * direction;

    private async void LerpSpeed(PlayerDTO playerDto)
    {
      _movementSpeed = playerDto.GetData<float>("MovementSpeed");
      
      while (_movementSpeed < _maxMovementSpeed && !EndPointReached)
      {
        Debug.Log(_movementSpeed);

        _movementSpeed = playerDto.GetData<float>("MovementSpeed");
        
        _movementSpeed += _acceleration * Time.fixedDeltaTime;
        _movementSpeed = Mathf.Min(_movementSpeed, _maxMovementSpeed);
        
        playerDto.SetStat("MovementSpeed", _movementSpeed);
        
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }
  }
}