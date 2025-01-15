using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public abstract class BaseState : IState
  {
    protected bool EndPointReached { get; private set; }

    public virtual void Enter(PlayerDTO playerData)
    {
      EndPointReached = false;
    }
    
    public abstract Vector3 Update(Vector3 direction, PlayerDTO playerDto);

    public virtual void Exit(PlayerDTO playerData)
    {
      EndPointReached = true;
    }
  }
}