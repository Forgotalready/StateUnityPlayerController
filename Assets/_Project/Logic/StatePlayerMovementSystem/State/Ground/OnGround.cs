using _Project.Logic.DTO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Logic.Movement
{
  public abstract class OnGround : BaseState
  {
    
    protected float MovementSpeed;
    protected abstract float MaxSpeed { get; }
    protected abstract float Acceleration { get; }

    public override void Enter(PlayerDTO playerDto)
    {
      base.Enter(playerDto);
      MovementSpeed = playerDto.GetData<float>(PlayerStatsConverter.HorizontalSpeed);

      if (MovementSpeed > MaxSpeed)
      {
        MovementSpeed = MaxSpeed;
        playerDto.SetStat(PlayerStatsConverter.HorizontalSpeed, MovementSpeed);
      }

      LerpSpeed(playerDto);
    }

    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto) =>
        playerDto.GetData<float>(PlayerStatsConverter.HorizontalSpeed) * direction;

    protected async void LerpSpeed(PlayerDTO playerDto)
    {
      MovementSpeed = playerDto.GetData<float>(PlayerStatsConverter.HorizontalSpeed);

      while (MovementSpeed < MaxSpeed && !EndPointReached)
      {
        //Debug.Log(_movementSpeed);

        MovementSpeed = playerDto.GetData<float>(PlayerStatsConverter.HorizontalSpeed);

        MovementSpeed += Acceleration * Time.fixedDeltaTime;
        MovementSpeed = Mathf.Min(MovementSpeed, MaxSpeed);

        playerDto.SetStat(PlayerStatsConverter.HorizontalSpeed, MovementSpeed);

        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }
  }
}