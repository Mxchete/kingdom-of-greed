using System;
using UnityEngine;

public class InputDataPool : DataPoolBase
{
  // Input flags
  public bool IsJumpPressed { get; private set; }
  public bool IsFirePressed { get; private set; }
  public Vector2 MoveDirection { get; private set; }

  // Update method to be called every frame
  public void UpdateInputs()
  {
    IsJumpPressed = Input.GetKey(KeyCode.Space);
    IsFirePressed = Input.GetMouseButton(0);
    MoveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
  }
}

