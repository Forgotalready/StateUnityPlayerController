using Cysharp.Threading.Tasks;
using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State.Ground
{
  public abstract class OnGround : BaseState
  {
    
    protected float MovementSpeed;
    protected abstract float MaxSpeed { get; }
    protected abstract float Acceleration { get; }

    public override void Enter(PlayerDTO playerDto)
    {
      base.Enter(playerDto);
      MovementSpeed = playerDto.GetData<float>("MovementSpeed");

      if (MovementSpeed > MaxSpeed)
      {
        MovementSpeed = MaxSpeed;
        playerDto.SetStat("MovementSpeed", MovementSpeed);
      }

      LerpSpeed(playerDto);
    }

    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto) =>
        playerDto.GetData<float>("MovementSpeed") * direction;

    protected async void LerpSpeed(PlayerDTO playerDto)
    {
      MovementSpeed = playerDto.GetData<float>("MovementSpeed");

      while (MovementSpeed < MaxSpeed && !EndPointReached)
      {
        //Debug.Log(_movementSpeed);

        MovementSpeed = playerDto.GetData<float>("MovementSpeed");

        MovementSpeed += Acceleration * Time.fixedDeltaTime;
        MovementSpeed = Mathf.Min(MovementSpeed, MaxSpeed);

        playerDto.SetStat("MovementSpeed", MovementSpeed);

        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
      }
    }
  }
}