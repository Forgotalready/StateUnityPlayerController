using R3;
using UnityEngine;

namespace StatePlayerMovementSystem
{
  public class PlayerController
  {
    public PlayerInputAction InputAction { get; private set; } = new();
    
    public readonly Subject<Unit> JumpPerformed = new();

    private readonly ReactiveProperty<bool> _crouchHold = new(false);
    public ReadOnlyReactiveProperty<bool> CrouchHold;
    
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
      CrouchHold = _crouchHold.ToReadOnlyReactiveProperty();
      
      InputAction.Gameplay.Jump.performed += _ => JumpPerformed.OnNext(Unit.Default);

      InputAction.Gameplay.Crouch.started += _ => _crouchHold.Value = true;
      InputAction.Gameplay.Crouch.canceled += _ => _crouchHold.Value = false;
    }

    ~PlayerController()
    {
      InputAction.Disable();
    }
  }
}