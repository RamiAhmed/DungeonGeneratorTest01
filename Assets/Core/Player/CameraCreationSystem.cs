using Assets.Core.Options;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(GridEnvironmentSystem))]
    public class CameraCreationSystem : SystemBase
    {
        private BlobAssetStore _blobAssetStore;

        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");

            var options = this.GetOptions<CameraOptions>();
            var camPrefab = options.cameraPrefab;

            _blobAssetStore = new BlobAssetStore();
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
            var prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(camPrefab, settings);

            var entity = EntityManager.Instantiate(prefabEntity);
            EntityManager.SetComponentData(entity, new Translation { Value = float3.zero });
            EntityManager.AddComponentData(entity, new PlayerCameraComponentData { });

            // clean up - destroy prefab entity
            EntityManager.DestroyEntity(prefabEntity);
        }

        protected override void OnUpdate()
        {
            /* NOOP */
        }

        protected override void OnDestroy()
        {
            _blobAssetStore?.Dispose();
        }
    }
}
