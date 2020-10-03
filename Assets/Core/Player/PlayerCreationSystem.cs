using Assets.Core.Grid;
using Assets.Core.Options;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(GridEnvironmentSystem))]
    public class PlayerCreationSystem : SystemBase
    {
        private BlobAssetStore _blobAssetStore;

        protected override void OnCreate()
        {
            Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            Debug.Log($"{this}: OnStartRunning");

            var options = this.GetOptions<PlayerOptions>();

            _blobAssetStore = new BlobAssetStore();
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
            var prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(options.playerPrefab, settings);

            var playerEntity = EntityManager.Instantiate(prefabEntity);
            EntityManager.AddComponentData(playerEntity, new PlayerComponentData
            {
                Index = PlayerConstants.LOCAL_PLAYER_INDEX
            });

            EntityManager.AddComponentData(playerEntity, new PlayerMovementComponentData { Velocity = float3.zero });

            var gridOptions = this.GetOptions<GridGeneratorOptions>();
            var startPosition = GridUtils.GetCellCenter(gridOptions.startX, gridOptions.startY, gridOptions.cellSize);

            // add some player height 
            startPosition = new Vector3(startPosition.x, startPosition.y + 5f, startPosition.z);

            EntityManager.SetComponentData(playerEntity, new Translation { Value = startPosition });
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
