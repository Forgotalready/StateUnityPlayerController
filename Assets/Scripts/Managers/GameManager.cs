using Factory;
using UnityEngine;
using Zenject;

namespace Managers
{
  public class GameManager : MonoBehaviour
  {
    [SerializeField] private GameObject spawnPosition;
    private PlayerFactory _playerFactory;

    [Inject]
    private void Init(PlayerFactory playerFactory)
    {
      _playerFactory = playerFactory;
    }

    private void Start()
    {
      _playerFactory.Create(spawnPosition.transform.position);
    }
  }
}