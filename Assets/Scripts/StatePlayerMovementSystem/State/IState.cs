using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public interface IState
  {
    public void Enter();
    public Vector3 Update(Vector3 direction);
    public void Exit();
  }
}