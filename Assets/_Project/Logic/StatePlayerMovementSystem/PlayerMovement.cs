using _Project.Logic.Controllers;
using _Project.Logic.DTO;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Movement
{
  [RequireComponent(typeof(CharacterController))]
  public class PlayerMovement : MonoBehaviour
  {
    private PlayerController _playerController;
    private CharacterController _characterController;
    private ViewRotator _viewRotator;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private LayerMask _groundLayerMask;

    private readonly CompositeDisposable _disposable = new();

    private PlayerFSM _playerFsm = new();
    private bool _isMoving = false;
    private bool _isGrounded = true;
    
    private bool _isJumped = false;
    private bool _isLeftGround = false;
    
    private bool _isCrouch = false;
    private Vector3 _moveDirection = Vector3.zero;

    private PlayerDTO _playerDto = new();

    [Inject]
    private void Init(Vector3 spawnPosition, PlayerController playerController, ViewRotator viewRotator)
    {
      transform.position = spawnPosition;
      _playerController = playerController;
      _viewRotator = viewRotator;
    }

    private void Start()
    {
      _characterController = GetComponent<CharacterController>();
      _playerDto.SetStat(PlayerStatsConverter.HorizontalSpeed, 0f);
      _playerDto.SetStat(PlayerStatsConverter.VerticalSpeed, 0f);
      _playerDto.SetStat(PlayerStatsConverter.PlayerGameObject, gameObject);
      BindActionsOfPlayerFSM();
      
      _playerController
          .JumpPerformed
          .Subscribe(_ => _isJumped = true)
          .AddTo(_disposable);
      _playerController
          .CrouchHold
          .Subscribe(state => _isCrouch = state)
          .AddTo(_disposable);
    }

    private void BindActionsOfPlayerFSM()
    {
      var moving = new Moving();
      var idle = new Idle();
      var jump = new Jump();
      var fall = new Fall();
      var crouchIdle = new CrouchIdle();
      var crouchMoving = new CrouchMoving();

      _playerFsm.AddTransition(
          idle,
          new[]
          {
              new ConcreteTransition(moving, new FuncPredicate(() => _isMoving)),
              new ConcreteTransition(jump, new FuncPredicate(() => _isJumped)),
              new ConcreteTransition(fall, new FuncPredicate(() => !_isGrounded && !_isJumped)),
              new ConcreteTransition(crouchIdle, new FuncPredicate(() => _isCrouch))
          }
      );

      _playerFsm.AddTransition(
          moving,
          new[]
          {
              new ConcreteTransition(idle, new FuncPredicate(() => !_isMoving )),
              new ConcreteTransition(jump, new FuncPredicate(() => _isJumped)),
              new ConcreteTransition(fall, new FuncPredicate(() => !_isGrounded)),
              new ConcreteTransition(crouchMoving, new FuncPredicate(() => _isCrouch))
          }
      );

      _playerFsm.AddTransition(
          jump,
          new[]
          {
              new ConcreteTransition(idle, new FuncPredicate(() => !_isMoving && _isGrounded && !_isJumped)),
              new ConcreteTransition(moving, new FuncPredicate(() => _isMoving && _isGrounded && !_isJumped)),
              new ConcreteTransition(fall, new FuncPredicate(() => !_isJumped && !_isGrounded))
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

      _playerFsm.AddTransition
      (
          crouchIdle,
          new[]
          {
              new ConcreteTransition(idle, new FuncPredicate(() => !_isCrouch)),
              new ConcreteTransition(crouchMoving, new FuncPredicate(() => _isMoving))
          }
      );

      _playerFsm.AddTransition
      (
          crouchMoving,
          new[]
          {
              new ConcreteTransition(moving, new FuncPredicate(() => !_isCrouch)),
              new ConcreteTransition(crouchIdle, new FuncPredicate(() => !_isMoving))
          }
      );
    }
    
    private void FixedUpdate()
    {
      _moveDirection = _playerController.MoveDirection;
      UpdatePlayerState();
      
      CheckStateTransitions();
      
      _viewRotator.Rotate(_playerController.MouseDelta, transform, _playerCamera);
      Move(transform.rotation * _moveDirection);
      FalseGravity();
    }
    
    private void UpdatePlayerState()
    {
      _isMoving = _moveDirection.magnitude > 0;

      if (_isJumped && !_isGrounded)
      {
        _isLeftGround = true;
      }
      
      if (_isJumped && _isLeftGround && _isGrounded)
      {
        _isJumped = false;
        _isLeftGround = false;
      }
      
      _isGrounded = Physics.CheckSphere(_groundCheck.transform.position, 0.1f, _groundLayerMask);
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
    
    private void Move(Vector3 moveDirection)
    {
      moveDirection.Normalize();

      Vector3 offset = _playerFsm.GetCurrentState().Update(moveDirection, _playerDto);

      _characterController.Move(offset * Time.fixedDeltaTime);
    }
    
    private void FalseGravity()
    {
      if (_isGrounded)
      {
        _characterController.Move(new Vector3(0f, -2f, 0f) * Time.fixedDeltaTime);
      }
    }
    
    private void OnDestroy()
    {
      _disposable.Dispose();
    }
  }
}