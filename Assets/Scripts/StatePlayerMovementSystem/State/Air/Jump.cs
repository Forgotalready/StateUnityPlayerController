using Cysharp.Threading.Tasks;
using DTO;
using UnityEngine;

namespace StatePlayerMovementSystem.State
{
  public class Jump : OnAir
  {
    private float _jumpHeight = 1f;
    
    public override Vector3 Update(Vector3 direction, PlayerDTO playerDto)
    {
      if (HorizontalDirection == null)
      {
        HorizontalDirection = direction;
        float initialVelocity = Mathf.Sqrt(2 * GravityAcceleration * _jumpHeight);
        VerticalVelocity = initialVelocity;
        Gravity();
      }

      float horizontalMovementSpeed = playerDto.GetData<float>("MovementSpeed");
      return new Vector3(horizontalMovementSpeed * HorizontalDirection.Value.x, VerticalVelocity,
          horizontalMovementSpeed * HorizontalDirection.Value.z);
    }
  }
}