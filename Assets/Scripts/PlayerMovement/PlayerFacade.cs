using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerFacade : MonoBehaviour
{
  private MovementController _movementController;
  private ViewRotator _viewRotator;
  private JumpController _jumpController;
  private CharacterController _characterController;
  
  private PlayerInput _playerInput;
  private Camera _playerCamera;

  private bool _isMovementLocked = false;

  private readonly CompositeDisposable _disposable = new();
  
  [Inject]
  private void Init(MovementController movementController, ViewRotator viewRotator, PlayerInput playerInput, JumpController jumpController)
  {
    _movementController = movementController;
    _viewRotator = viewRotator;
    _playerInput = playerInput;
    _jumpController = jumpController;
    _characterController = GetComponent<CharacterController>();
    _playerCamera = GetComponentInChildren<Camera>();
  }

  private void OnEnable()
  {
    _jumpController
        .IsJumped
        .Subscribe(LockMovement)
        .AddTo(_disposable);
    _jumpController
        .IsFalling
        .Subscribe(OnFalling)
        .AddTo(_disposable);
    _playerInput
        .JumpClicked
        .Subscribe(_ => OnJumpClicked())
        .AddTo(_disposable);
  }

  private void OnFalling(bool state)
  {
    LockMovement(state);
    if (state)
    {
      _jumpController.Fall(transform.rotation * _playerInput.MoveDirection * _movementController.MovementSpeed, _characterController);
    }
  }

  private void OnJumpClicked()
  {
    _jumpController.Jump(transform.rotation * _playerInput.MoveDirection * _movementController.MovementSpeed, _characterController);
  }

  private void OnDisable()
  {
    _disposable.Dispose();
  }

  private void FixedUpdate()
  {
    Vector2 mouseDelta = _playerInput.CameraDirection;
    _viewRotator.Rotate(mouseDelta, transform, _playerCamera);
    if (!_isMovementLocked)
    {
      Vector3 moveDirection = transform.rotation * _playerInput.MoveDirection;
      _movementController.Move(_characterController, moveDirection);
    }
  }

  private void LockMovement(bool jumpState)
  {
    _isMovementLocked = jumpState;
  }
}
