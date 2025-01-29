using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Logic.Controllers
{
  public class PlayerController : IInitializable, IDisposable
  {
    #region Actions

    public readonly Subject<Unit> JumpPerformed = new();
    private readonly ReactiveProperty<bool> _crouchHold = new(false);
    public ReadOnlyReactiveProperty<bool> CrouchHold;

    #endregion

    #region Properties

    public PlayerInputAction InputAction { get; private set; } = new();
    public Vector3 MoveDirection
    {
      get
      {
        Vector2 inputDirection = InputAction.Gameplay.Movement.ReadValue<Vector2>();
        return new Vector3(inputDirection.x, 0f, inputDirection.y);
      }
    }

    public Vector2 MouseDelta => InputAction.Gameplay.Look.ReadValue<Vector2>();

    #endregion

    public void Initialize()
    {
      InputAction.Enable();
      CrouchHold = _crouchHold.ToReadOnlyReactiveProperty();

      InputAction.Gameplay.Jump.performed += OnJumpOnPerformed;
      InputAction.Gameplay.Crouch.started += OnCrouchOnStarted;
      InputAction.Gameplay.Crouch.canceled += OnCrouchOnCanceled;
    }

    private void OnCrouchOnCanceled(InputAction.CallbackContext _) => _crouchHold.Value = false;
    private void OnCrouchOnStarted(InputAction.CallbackContext _) => _crouchHold.Value = true;
    private void OnJumpOnPerformed(InputAction.CallbackContext _) => JumpPerformed.OnNext(Unit.Default);

    public void Dispose()
    {
      InputAction.Gameplay.Jump.performed -= OnJumpOnPerformed;
      InputAction.Gameplay.Crouch.started -= OnCrouchOnStarted;
      InputAction.Gameplay.Crouch.canceled -= OnCrouchOnCanceled;
      InputAction.Disable();
    }
  }
}