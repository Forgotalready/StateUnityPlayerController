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
    Debug.Log("SceneInstaller: Start binding modules");
    
    PlayerBindings();
    
    Debug.Log("SceneInstaller: Player Modules bind end");
    
    Container
        .Bind<GameManager>()
        .FromComponentInHierarchy()
        .AsSingle();
    
    Debug.Log("SceneInstaller: Modules bind end");
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