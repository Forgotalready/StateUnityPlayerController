using _Project.Logic.DTO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Logic.Movement
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

    protected async void Gravity(PlayerDTO playerDto)
    {
      while (!EndPointReached)
      {
        VerticalVelocity -= GravityAcceleration * Time.fixedDeltaTime;
        playerDto.SetStat(PlayerStatsConverter.VerticalSpeed, VerticalVelocity);
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }
  }
}