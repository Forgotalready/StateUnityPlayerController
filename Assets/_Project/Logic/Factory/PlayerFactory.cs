using _Project.Logic.Movement;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Factory
{
  public class PlayerFactory : PlaceholderFactory<Vector3, PlayerMovement>
  { }
}