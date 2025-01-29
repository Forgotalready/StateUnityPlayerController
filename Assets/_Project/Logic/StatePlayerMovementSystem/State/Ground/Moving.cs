namespace _Project.Logic.Movement
{
  public class Moving : OnGround
  {
    protected override float MaxSpeed { get => 4.0f; }
    protected override float Acceleration { get => 2.0f; }
  }
}