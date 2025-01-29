namespace _Project.Logic.Movement
{
  public interface ITransition
  {
    public IState To { get; }
    public IPredicate Predicate { get; }
  }
}