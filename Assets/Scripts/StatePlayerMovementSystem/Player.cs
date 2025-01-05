using R3;
using StatePlayerMovementSystem.Predicate;
using StatePlayerMovementSystem.State;
using StatePlayerMovementSystem.Transition;
using UnityEngine;
using Zenject;

namespace StatePlayerMovementSystem
{
  [RequireComponent(typeof(CharacterController))]
  public class Player : MonoBehaviour
  {
    private PlayerController _playerController;
    private readonly CompositeDisposable _disposable = new();
    private CharacterController _characterController;
    private ViewRotator _viewRotator;
    [SerializeField] private Camera _playerCamera;

    private PlayerFSM _playerFsm = new();
    private bool _isMoving = false;

    [Inject]
    private void Init(Vector3 spawnPosition, PlayerController playerController, ViewRotator viewRotator)
    {
      transform.position = spawnPosition;
      _playerController = playerController;
      _viewRotator = viewRotator;
      _characterController = GetComponent<CharacterController>();

      BindActionsOfPlayerFSM();
    }

    private void BindActionsOfPlayerFSM()
    {
      _playerFsm.AddTransition(
          new Idle(),
          new[]
          {
              new ConcreteTransition(new Moving(), new FuncPredicate(() => _isMoving))
          }
      );

      _playerFsm.AddTransition(
          new Moving(),
          new[]
          {
              new ConcreteTransition(new Idle(), new FuncPredicate(() => !_isMoving))
          }
      );
    }

    private void OnEnable()
    {
    }

    private void FixedUpdate()
    {
      Vector3 moveDirection = _playerController.MoveDirection;
      _isMoving = moveDirection.magnitude > 0;
      
      _viewRotator.Rotate(_playerController.MouseDelta, transform, _playerCamera);
      
      Move(transform.rotation * moveDirection);
      CheckStateTransitions();
    }

    private void CheckStateTransitions()
    {
      foreach (ITransition transition in _playerFsm.GetCurrentStateTransitions())
      {
        if (transition.Predicate.Evaluate())
        {
          _playerFsm.ChangeState(transition.To);
          break;
        }
      }

      Debug.Log(_playerFsm.GetCurrentState().GetType());
    }

    private void OnDisable()
    {
      _disposable.Dispose();
    }

    private void Move(Vector3 moveDirection)
    {
      moveDirection.Normalize();

      Vector3 offset = _playerFsm.GetCurrentState().Update(moveDirection);

      _characterController.Move(offset);
    }
  }
}