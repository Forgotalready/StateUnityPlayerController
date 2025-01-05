using Factory;
using Managers;
using StatePlayerMovementSystem;
using UnityEngine;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
  [SerializeField] private GameObject _player;
  
  public override void InstallBindings()
  {
    PlayerBindings();
    Container
        .Bind<GameManager>()
        .FromComponentInHierarchy()
        .AsSingle();
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void PlayerBindings()
  {
    Container
        .Bind<PlayerController>()
        .FromNew()
        .AsSingle();
    Container
        .Bind<ViewRotator>()
        .FromNew()
        .AsSingle();
    Container
        .BindFactory<Vector3, Player, PlayerFactory>()
        .FromComponentInNewPrefab(_player)
        .AsSingle();
  }
}