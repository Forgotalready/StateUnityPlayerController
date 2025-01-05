using StatePlayerMovementSystem.Predicate;
using StatePlayerMovementSystem.State;

namespace StatePlayerMovementSystem.Transition
{
  public class ConcreteTransition : ITransition
  {
    public ConcreteTransition(IState to, IPredicate predicate)
    {
      _to = to;
      _predicate = predicate;
    }

    private readonly IState _to;
    private readonly IPredicate _predicate;

    public IState To => _to;

    public IPredicate Predicate => _predicate;
  }
}