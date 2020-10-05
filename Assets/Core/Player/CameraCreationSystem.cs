using Assets.Core.Options;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(GridEnvironmentSystem))]
    public class CameraCreationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");

            var options = this.GetOptions<CameraOptions>();
            var camPrefab = options.cameraPrefab;

            var entity = EntityManager.Instantiate(camPrefab);
            EntityManager.SetComponentData(entity, new Translation { Value = float3.zero });
            EntityManager.AddComponentData(entity, new PlayerCameraComponentData { });
        }

        protected override void OnUpdate()
        {
            /* NOOP */
        }
    }
}
