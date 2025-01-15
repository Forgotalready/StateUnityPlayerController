using DTO;
using Helpers.Timer;
using R3;
using StatePlayerMovementSystem.Predicate;
using StatePlayerMovementSystem.State;
using StatePlayerMovementSystem.State.Ground;
using StatePlayerMovementSystem.Transition;
using UnityEngine;
using Zenject;

namespace StatePlayerMovementSystem
{
  [RequireComponent(typeof(CharacterController))]
  public class Player : MonoBehaviour
  {
    private PlayerController _playerController;
    private CharacterController _characterController;
    private ViewRotator _viewRotator;

    private Timer _jumpTimer = new(0.5f);

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private LayerMask _groundLayerMask;

    private readonly CompositeDisposable _disposable = new();

    private PlayerFSM _playerFsm = new();
    private bool _isMoving = false;
    private bool _isGrounded = true;
    private bool _isJumped = false;
    private PlayerDTO _playerDto = new();

    [Inject]
    private void Init(Vector3 spawnPosition, PlayerController playerController, ViewRotator viewRotator)
    {
      transform.position = spawnPosition;
      _playerController = playerController;
      _viewRotator = viewRotator;
      _characterController = GetComponent<CharacterController>();

      _playerDto.SetStat("MovementSpeed", 0f);

      BindActionsOfPlayerFSM();
    }

    private void BindActionsOfPlayerFSM()
    {
      var moving = new Moving();
      var idle = new Idle();
      var jump = new Jump();
      var fall = new Fall();

      _playerFsm.AddTransition(
          idle,
          new[]
          {
              new ConcreteTransition(moving, new FuncPredicate(() => (_isMoving && _isGrounded))),
              new ConcreteTransition(jump, new FuncPredicate(() => _isJumped)),
              new ConcreteTransition(fall, new FuncPredicate(() => !_isGrounded))
          }
      );

      _playerFsm.AddTransition(
          moving,
          new[]
          {
              new ConcreteTransition(idle, new FuncPredicate(() => !_isMoving && _isGrounded)),
              new ConcreteTransition(jump, new FuncPredicate(() => _isJumped)),
              new ConcreteTransition(fall, new FuncPredicate(() => !_isGrounded))
          }
      );

      _playerFsm.AddTransition(
          jump,
          new[]
          {
              new ConcreteTransition(idle, new FuncPredicate(() => !_isMoving && _isGrounded)),
              new ConcreteTransition(moving, new FuncPredicate(() => _isMoving && _isGrounded))
          }
      );
      
      _playerFsm.AddTransition(
          fall,
          new[]
          {
              new ConcreteTransition(idle, new FuncPredicate(() => !_isMoving && _isGrounded)),
              new ConcreteTransition(moving, new FuncPredicate(() => _isMoving && _isGrounded))
          }
      );
    }

    private void OnEnable()
    {
      _playerController
          .JumpPerformed
          .Subscribe(_ =>
          {
            _isJumped = true;
            _jumpTimer.StartTimer();
          })
          .AddTo(_disposable);
      _jumpTimer
          .TimerTick
          .Subscribe(_ => _isJumped = false)
          .AddTo(_disposable);
    }

    private void FixedUpdate()
    {
      Vector3 moveDirection = _playerController.MoveDirection;
      _isMoving = moveDirection.magnitude > 0;
      _isGrounded = Physics.CheckSphere(_groundCheck.transform.position, 0.1f, _groundLayerMask);

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
          _playerFsm.ChangeState(transition.To, _playerDto);
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

      Vector3 offset = _playerFsm.GetCurrentState().Update(moveDirection, _playerDto);

      _characterController.Move(offset * Time.fixedDeltaTime);
    }
  }
}