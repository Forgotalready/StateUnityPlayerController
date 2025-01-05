using StatePlayerMovementSystem;
using UnityEngine;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
  public override void InstallBindings()
  {
    PlayerBindings();
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
        .Bind<Player>()
        .FromComponentInHierarchy()
        .AsSingle();
  }
}