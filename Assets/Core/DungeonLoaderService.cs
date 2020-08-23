using Assets.Core.Grid;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Assets.Core
{
    public class DungeonLoaderService
    {
        private DungeonOptions _options;

        public byte[] gridState { get; private set; }

        public bool[] floodState { get; private set; }

        public void Generate(DungeonOptions options)
        {
            _options = options;

            CreateGrid();
            PlaceObjects();
            CreatePlayer();
        }

        private void CreateGrid()
        {
            using (var service = new DungeonGeneratorService())
            {
                service.Generate(_options.generatorOptions);

                var (gridState, floodState) = service.Complete();
                this.gridState = gridState
                    .ToArray();

                this.floodState = floodState
                    .Select(s => s == GridStateConstants.FLOODED)
                    .ToArray();
            }
        }

        private void PlaceObjects()
        {
            // Place objects representing blocked cells and open paths
            var placerService = new GridPlacerService(new GridPlacerOptions
            {
                parent = _options.parent,
                blockPrefab = _options.blockPrefab,
                pathPrefab = _options.pathPrefab,
                exitPrefab = _options.exitPrefab,
                cellSize = _options.cellSize,
                rows = _options.generatorOptions.gridRows,
                gridState = gridState
            });

            placerService.PlaceObjects();
        }

        private void CreatePlayer()
        {
            // Instantiate and place the player prefab
            var (startX, startY) = GridUtils.GetCoordinates(_options.generatorOptions.startIndex, _options.generatorOptions.gridRows);
            var startPos = GridUtils.GetPositionByIndex(startX, startY, _options.cellSize) +
                new Vector3(_options.cellSize * 0.5f, 1f, _options.cellSize * 0.5f);
            var playerGo = GameObject.Instantiate(_options.playerPrefab, startPos, Quaternion.identity);

            /// DEBUG STUFF
            GameObject.FindObjectOfType<FreeLookCam>().Target = playerGo.transform;
        }
    }

    public class DungeonOptions
    {
        public DungeonGeneratorOptions generatorOptions;

        public Transform parent;

        public GameObject blockPrefab;
        public GameObject pathPrefab;
        public GameObject playerPrefab;
        public GameObject exitPrefab;

        public int cellSize;
    }
}
