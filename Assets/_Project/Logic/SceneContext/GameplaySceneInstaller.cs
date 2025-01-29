using _Project.Logic.Controllers;
using _Project.Logic.Factory;
using _Project.Logic.Managers;
using _Project.Logic.Movement;
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
        .BindInterfacesAndSelfTo<PlayerController>()
        .AsSingle();
    Container
        .Bind<ViewRotator>()
        .FromNew()
        .AsSingle();
    Container
        .BindFactory<Vector3, PlayerMovement, PlayerFactory>()
        .FromComponentInNewPrefab(_player)
        .AsSingle();
  }
}