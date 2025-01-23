using Cysharp.Threading.Tasks;
using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public abstract class OnAir : BaseState
  {
    protected Vector3? HorizontalDirection;
    protected float GravityAcceleration  = 9.8f;
    protected float VerticalVelocity;

    public override void Enter(PlayerDTO playerDto)
    {
      base.Enter(playerDto);
      HorizontalDirection = null;
      VerticalVelocity = 0f;
    }

    protected async void Gravity()
    {
      while (!EndPointReached)
      {
        VerticalVelocity -= GravityAcceleration * Time.fixedDeltaTime;
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }
  }
}