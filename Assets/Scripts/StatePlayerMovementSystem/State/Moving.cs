using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public class Moving : IState
  {
    private float _movementSpeed = 0.0f;
    private float _maxMovementSpeed = 4.0f;
    private float _acceleration = 2.0f;
    
    private async void LerpSpeed()
    {
      while (_movementSpeed < _maxMovementSpeed)
      {
        _movementSpeed += _acceleration * Time.fixedDeltaTime;
        _movementSpeed = Mathf.Min(_maxMovementSpeed, _movementSpeed);
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }
    
    public void Enter() => LerpSpeed();

    public Vector3 Update(Vector3 inputDirection) => _movementSpeed * Time.fixedDeltaTime * inputDirection;

    public void Exit() => _movementSpeed = 0;
  }
}