using Cysharp.Threading.Tasks;
using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public class Fall : OnAir
  {
    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto)
    {
      if (HorizontalDirection == null)
      {
        HorizontalDirection = direction;
        Gravity();
      }
      
      float horizontalMovementSpeed = playerDto.GetData<float>("MovementSpeed");
      return new Vector3(horizontalMovementSpeed * HorizontalDirection.Value.x, VerticalVelocity,
          horizontalMovementSpeed * HorizontalDirection.Value.z);
    }
  }
}