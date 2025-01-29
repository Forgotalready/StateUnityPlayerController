using _Project.Logic.DTO;
using UnityEngine;

namespace _Project.Logic.Movement
{
  public class Fall : OnAir
  {
    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto)
    {
      if (HorizontalDirection == null)
      {
        HorizontalDirection = direction;
        Gravity(playerDto);
      }
      
      float horizontalMovementSpeed = playerDto.GetData<float>(PlayerStatsConverter.HorizontalSpeed);
      return new Vector3(horizontalMovementSpeed * HorizontalDirection.Value.x, VerticalVelocity,
          horizontalMovementSpeed * HorizontalDirection.Value.z);
    }
  }
}