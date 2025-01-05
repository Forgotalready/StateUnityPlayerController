using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public class Idle : IState
  {
    public void Enter()
    { }

    public Vector3 Update(Vector3 direction) => Vector3.zero;

    public void Exit()
    { }
  }
}