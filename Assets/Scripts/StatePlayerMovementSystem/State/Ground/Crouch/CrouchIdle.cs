using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State.Ground.Crouch
{
  public class CrouchIdle : OnGround
  {
    private GameObject _player;
    
    protected override float MaxSpeed { get => 0f; }
    protected override float Acceleration { get => 0f; }
    
    public override void Enter(PlayerDTO playerDto)
    {
      base.Enter(playerDto);
      SetUpPlayerCollider(playerDto, 0.5f, 1f);
    }
    
    private void SetUpPlayerCollider(PlayerDTO playerDto, float localHeight, float controllerHeight)
    {
      _player = playerDto.GetData<GameObject>("Player");
      _player.transform.localScale = new Vector3(_player.transform.localScale.x, localHeight, _player.transform.localScale.z);
      _player.GetComponent<CharacterController>().height = controllerHeight;
    }
    
    public override void Exit(PlayerDTO playerDto)
    {
      base.Exit(playerDto);
      SetUpPlayerCollider(playerDto, 1f, 2f);
    }
  }
}