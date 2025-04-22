using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Very basic implementation of room class, should definitely be improved
public class Rooms : MonoBehaviour
{

  public enum direction
  {
    up = 0,
    down = 1,
    right = 2,
    left = 3
  }

  public GameObject[] walls;

  public void UpdateRoom(bool[] status)
  {
    for (int i = 0; i < status.Length; i++)
    {
      walls[i].SetActive(!status[i]);
    }
  }
}
