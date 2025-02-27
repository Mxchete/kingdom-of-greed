using UnityEngine;

public class Init : MonoBehaviour
{
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void LoadGameData()
  {
    GameDataPool instance = GameDataPool.Instance;

    // Register DataPools with GameDataPool
    instance.Register<SceneDataPool>();
    // instance.Register<MenuDataPool>();
    // instance.Register<RoomDataPool>();

    // Create scene for title screen
    SceneDataPool scenes = instance.Get<SceneDataPool>();
    // Scene title = scenes.Register<TitleScene>();
    // scenes.CreateScene("dungeon1");
    scenes.LoadScene("dungeon1");
    scenes.UnloadScene("Init");
  }
}
