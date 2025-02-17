using UnityEngine;

public class Init : MonoBehaviour
{
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void LoadGameData()
  {
    GameDataPool instance = GameDataPool.Instance;

    instance.Register<SceneDataPool>();
  }
}
