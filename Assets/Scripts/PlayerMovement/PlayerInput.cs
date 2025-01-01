using R3;
using UnityEngine;

public class PlayerInput
{
  private readonly PlayerInputAction _playerInputAction;
  
  public readonly Subject<Unit> JumpClicked = new();
  
  public PlayerInput()
  {
    _playerInputAction = new PlayerInputAction();
    _playerInputAction.Enable();
    _playerInputAction.Gameplay.Jump.performed += _ => JumpClicked.OnNext(Unit.Default);
  }
  
  public Vector3 MoveDirection => _playerInputAction.Gameplay.Movement.ReadValue<Vector3>();
  public Vector2 CameraDirection => _playerInputAction.Gameplay.Look.ReadValue<Vector2>();
}
