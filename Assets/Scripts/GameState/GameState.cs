using UnityEngine;

public class GameState : MonoBehaviour
{
  IGameState currentGameState = new GameStart();

  public bool ChangeState(IGameState nextState)
  {
    bool success = true;
    if (currentGameState != null)
    {
      success &= currentGameState.Exit();
    }
    currentGameState = nextState;
    success &= currentGameState.Enter();

    return success;
  }

  public void Update()
  {
    if (currentGameState != null)
    {
      currentGameState.Running();
    }
  }
}
