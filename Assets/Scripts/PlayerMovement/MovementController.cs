using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class MovementController
{
  private readonly float _maxMovementSpeed = 5.0f;
  private readonly float _acceleration = 2.0f;

  public ReactiveProperty<bool> IsMoving { get; } = new(false);

  public float MovementSpeed { get; private set; } = 0f;
  
  private async void LerpSpeed()
  {
    while (MovementSpeed < _maxMovementSpeed && IsMoving.Value)
    {
      MovementSpeed += _acceleration * Time.fixedDeltaTime;
      MovementSpeed = Mathf.Min(MovementSpeed, _maxMovementSpeed);
      await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
    }
  }

  public void Move(CharacterController characterController, Vector3 moveDirection)
  {
    if (moveDirection.magnitude == 0)
    {
      IsMoving.Value = false;
      MovementSpeed = 0f;
      return;
    }

    if (!IsMoving.Value)
    {
      IsMoving.Value = true;
      LerpSpeed();
    }
    
    moveDirection.Normalize();
    characterController.Move((MovementSpeed * Time.fixedDeltaTime) * moveDirection);
  }
}
