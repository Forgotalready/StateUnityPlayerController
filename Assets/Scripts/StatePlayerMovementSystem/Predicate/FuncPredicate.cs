using System;

namespace StatePlayerMovementSystem.Predicate
{
  public class FuncPredicate : IPredicate
  {
    private Func<bool> _predicate; 
    
    public FuncPredicate(Func<bool> predicate)
    {
      _predicate = predicate;
    }
    
    public bool Evaluate()
    {
      return _predicate.Invoke();
    }
  }
}