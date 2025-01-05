using R3;
using UnityEngine;

namespace StatePlayerMovementSystem
{
  public class PlayerController
  {
    public PlayerInputAction InputAction { get; private set; } = new();
    
    public readonly Subject<Unit> JumpPerformed = new();

    public Vector3 MoveDirection
    {
      get => InputAction.Gameplay.Movement.ReadValue<Vector3>();
    }

    public Vector2 MouseDelta
    {
      get => InputAction.Gameplay.Look.ReadValue<Vector2>();
    }

    public PlayerController()
    {
      InputAction.Enable();
      InputAction.Gameplay.Jump.performed += _ => JumpPerformed.OnNext(Unit.Default);
    }

    ~PlayerController()
    {
      InputAction.Disable();
    }
  }
}