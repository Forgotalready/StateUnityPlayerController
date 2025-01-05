using System;
using System.Collections.Generic;
using System.Linq;
using StatePlayerMovementSystem.State;
using StatePlayerMovementSystem.Transition;

namespace StatePlayerMovementSystem
{
  public class PlayerFSM
  {
    private IState _currentState = new Idle();
    private Dictionary<String, List<ITransition>> _transitions = new();

    public IReadOnlyList<ITransition> GetCurrentStateTransitions() => _transitions[_currentState.GetType().Name];

    public void AddTransition(IState state, ITransition[] transitions)
    {
      String stateName = state.GetType().Name;
      if (!_transitions.ContainsKey(stateName))
      {
        _transitions.Add(stateName, new List<ITransition>());
      }

      foreach (ITransition transition in transitions.ToList())
      {
        _transitions[stateName].Add(transition);
      }
    }

    public IState GetCurrentState() => _currentState;
    
    public void ChangeState(IState newState)
    {
      if (newState.GetType().Name == _currentState.GetType().Name)
      {
        return;
      }

      IState oldState = _currentState;
      oldState.Exit();
      newState.Enter();
      _currentState = newState;
    }
  }
}