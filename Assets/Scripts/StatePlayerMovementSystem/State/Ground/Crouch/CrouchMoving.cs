namespace StatePlayerMovementSystem.State.Ground.Crouch
{
  public class CrouchMoving : CrouchIdle
  {
    protected override float MaxSpeed { get => 2.0f; }
    protected override float Acceleration { get => 1.0f; }
  }
}