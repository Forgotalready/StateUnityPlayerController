using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public class Moving : IState
  {

    private float _movementSpeed;
    private float _maxMovementSpeed = 4.0f;
    private float _acceleration = 2.0f;
    
    public void Enter()
    {
      _movementSpeed = 0f;
      LerpSpeed();
    }

    public Vector3 Update(Vector3 direction)
    {
      Debug.Log("Player movement speed: " + _movementSpeed);
      return _movementSpeed * Time.fixedDeltaTime * direction;
    }

    public void Exit()
    { }
    
    private async void LerpSpeed()
    {
      while (_movementSpeed < _maxMovementSpeed)
      {
        _movementSpeed += _acceleration * Time.fixedDeltaTime;
        _movementSpeed = Mathf.Min(_movementSpeed, _maxMovementSpeed);
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }
  }
}