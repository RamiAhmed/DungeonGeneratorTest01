using Assets.Core.Grid;
using Assets.Core.Options;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(GridEnvironmentSystem))]
    public class PlayerCreationSystem : BaseGridDependentSystem
    {
        private BlobAssetStore _blobAssetStore;

        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");

            var options = this.GetOptions<PlayerOptions>();

            _blobAssetStore = new BlobAssetStore();
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
            var prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(options.playerPrefab, settings);

            var playerEntity = EntityManager.Instantiate(prefabEntity);
            EntityManager.AddComponentData(playerEntity, new PlayerComponentData
            {
                Index = 1 // TODO: use indices for multiplayer support
            });

            EntityManager.AddComponentData(playerEntity, new PlayerMovementComponentData { Velocity = float3.zero });

            var gridOptions = this.GetOptions<GridGeneratorOptions>();
            var startIndex = GetGridState().Single(state => state == GridStateConstants.START);
            var (x, y) = GridUtils.GetCoordinates(startIndex, gridOptions.gridRows);
            var startPosition = GridUtils.GetCellCenter(x, y, gridOptions.cellSize);

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
