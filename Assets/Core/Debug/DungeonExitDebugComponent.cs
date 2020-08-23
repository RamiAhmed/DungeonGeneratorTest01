using UnityEngine;

namespace Assets.Core.Debug
{
    public sealed class DungeonExitDebugComponent : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (!enabled)
                return;

            if (!other.CompareTag("Player"))
                return;

            enabled = false;
            DebugTestStarter.CreateDungeonTest();
        }
    }
}
