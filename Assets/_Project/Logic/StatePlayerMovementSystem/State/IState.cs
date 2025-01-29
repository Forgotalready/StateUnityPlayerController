using _Project.Logic.DTO;
using UnityEngine;

namespace _Project.Logic.Movement
{
  public interface IState
  {
    public void Enter(PlayerDTO playerData);
    public Vector3 Update(Vector3 direction, PlayerDTO playerData);
    public void Exit(PlayerDTO playerData);
  }
}