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
        .Bind<PlayerInput>()
        .FromNew()
        .AsSingle();

    Container
        .Bind<MovementController>()
        .FromNew()
        .AsSingle();

    Container
        .Bind<CameraController>()
        .FromNew()
        .AsSingle();

    Container
        .Bind<JumpController>()
        .FromComponentInHierarchy()
        .AsSingle();
  }
}