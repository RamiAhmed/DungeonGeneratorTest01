using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Core.Grid
{
    public class GridEnvironmentPlacerService : IDisposable
    {
        private NativeArray<byte> _gridState;
        private EntityCommandBuffer _commandBuffer;
        private readonly GridPlacerOptions _options;

        private Entity[] _prefabs;
        private BlobAssetStore _blobAssetStore;

        public GridEnvironmentPlacerService(GridPlacerOptions options, NativeArray<byte> gridState, EntityCommandBuffer commandBuffer)
        {
            _options = options;
            _gridState = gridState;
            _commandBuffer = commandBuffer;
        }

        public void Execute()
        {
            var watch = Stopwatch.StartNew();

            PreparePrefabs();

            InstantiatePrefabs();

            CleanupPrefabs();

            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }

        private void InstantiatePrefabs()
        {
            for (int i = 0; i < _gridState.Length; i++)
            {
                var prefab = GetPrefab(i);
                var position = GridUtils.GetPositionByIndex(i, _options.rows, _options.cellSize);

                var entity = _commandBuffer.Instantiate(prefab);
                _commandBuffer.AddComponent(entity, new GridEntityComponentData
                {
                    Index = i,
                });

                _commandBuffer.SetComponent(entity, new Translation { Value = position });
            }
        }

        private void CleanupPrefabs()
        {
            // Destroy the prefabs - clean up
            foreach (var prefab in _prefabs)
            {
                _commandBuffer.DestroyEntity(prefab);
            }
        }

        private void PreparePrefabs()
        {
            _blobAssetStore = new BlobAssetStore();
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);

            _prefabs = new[]
            {
                GameObjectConversionUtility.ConvertGameObjectHierarchy(_options.exitPrefab, settings),
                GameObjectConversionUtility.ConvertGameObjectHierarchy(_options.blockPrefab, settings),
                GameObjectConversionUtility.ConvertGameObjectHierarchy(_options.pathPrefab, settings)
            };
        }

        private Entity GetPrefab(int index)
        {
            if (_gridState[index] == GridStateConstants.EXIT)
                return _prefabs[0];

            if (_gridState[index] == GridStateConstants.BLOCKED)
                return _prefabs[1];

            return _prefabs[2];
        }

        public void Dispose()
        {
            _blobAssetStore?.Dispose();
        }
    }
}
