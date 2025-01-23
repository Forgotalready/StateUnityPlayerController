using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public interface IState
  {
    public void Enter(PlayerDTO playerData);
    public Vector3 Update(Vector3 direction, PlayerDTO playerData);
    public void Exit(PlayerDTO playerData);
  }
}