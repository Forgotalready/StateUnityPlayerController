using R3;
using UnityEngine;

namespace StatePlayerMovementSystem
{
  public class PlayerController
  {
    public PlayerInputAction PlayerInputAction { get; private set; } = new();

    public readonly Subject<Unit> JumpPerformed = new();

    public Vector3 MoveDirection => PlayerInputAction.Gameplay.Movement.ReadValue<Vector3>();

    public Vector2 CameraDirection => PlayerInputAction.Gameplay.Look.ReadValue<Vector2>();

    public PlayerController()
    {
      PlayerInputAction.Enable();
      PlayerInputAction.Gameplay.Jump.performed += _ => JumpPerformed.OnNext(Unit.Default);
    }
  }
}