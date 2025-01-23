namespace StatePlayerMovementSystem.State.Ground
{
  public class Idle : OnGround
  {
    protected override float MaxSpeed { get => 0.0f; }
    protected override float Acceleration { get => 0.0f; }
  }
}