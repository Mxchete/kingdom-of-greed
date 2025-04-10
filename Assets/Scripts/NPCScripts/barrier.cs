using UnityEngine;
using Yarn;
using Yarn.Unity;
public class Gate : MonoBehaviour
{
    [YarnCommand("OpenVillageBarrier")]
    public void OpenVillageBarrier()
    {
        gameObject.SetActive(false);

    }
}
