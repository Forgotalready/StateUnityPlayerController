using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State.Ground
{
  public class Idle : BaseState
  {
    public override void Enter(PlayerDTO playerDto)
    {
      base.Enter(playerDto);
      playerDto.SetStat("MovementSpeed", 0f);
    }

    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto) => Vector3.zero;
  }
}