using System;
using System.Collections.Generic;
using _Project.Logic.DTO;

namespace _Project.Logic.Movement
{
  public class PlayerFSM
  {
    private IState _currentState = new Idle();
    private Dictionary<Type, List<ITransition>> _transitions = new();

    public IReadOnlyList<ITransition> GetCurrentStateTransitions() => _transitions[_currentState.GetType()];

    public void AddTransition(IState state, ITransition[] transitions)
    {
      Type stateType = state.GetType();
      if (!_transitions.ContainsKey(stateType))
      {
        _transitions.Add(stateType, new List<ITransition>());
      }

      foreach (ITransition transition in transitions)
      {
        _transitions[stateType].Add(transition);
      }
    }

    public IState GetCurrentState() => _currentState;
    
    public void ChangeState(IState newState, PlayerDTO playerDto)
    {
      if (newState.GetType() == _currentState.GetType())
      {
        return;
      }

      IState oldState = _currentState;
      oldState.Exit(playerDto);
      newState.Enter(playerDto);
      _currentState = newState;
    }
  }
}