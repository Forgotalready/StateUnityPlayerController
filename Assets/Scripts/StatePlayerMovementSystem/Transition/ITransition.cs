using StatePlayerMovementSystem.Predicate;
using StatePlayerMovementSystem.State;

namespace StatePlayerMovementSystem.Transition
{
  public interface ITransition
  {
    public IState To { get; }
    public IPredicate Predicate { get; }
  }
}