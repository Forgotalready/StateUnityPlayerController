using UnityEngine;

public class ViewRotator
{
  private readonly float _mouseSensitivity = 100.0f;
  private float _verticalRotation = 0f;

  public void Rotate(Vector2 mouseDelta, Transform playerTransform, Camera camera)
  {
    mouseDelta *= (_mouseSensitivity * Time.deltaTime);
    
    playerTransform.Rotate(Vector3.up * mouseDelta.x);
    
    _verticalRotation -= mouseDelta.y;
    _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
    
    camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
  }
}