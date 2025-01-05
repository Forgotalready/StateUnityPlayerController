using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public class Standing : IState
  {
    public void Enter()
    { }

    public Vector3 Update(Vector3 inputDirection)
    {
      return Vector3.zero;
    }

    public void Exit()
    { }
  }
}