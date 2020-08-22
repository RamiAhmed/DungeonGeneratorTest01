using Assets.Core.Grid;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Assets.Core
{
    public class DungeonLoaderService
    {
        public byte[] gridState { get; private set; }

        public bool[] floodState { get; private set; }

        public void Generate(DungeonOptions options)
        {
            using (var service = new DungeonGeneratorService())
            {
                service.Generate(options.generatorOptions);

                var (gridState, floodState) = service.Complete();
                this.gridState = gridState
                    .ToArray();

                this.floodState = floodState
                    .Select(s => s == GridStateConstants.FLOODED)
                    .ToArray();
            }

            Debug.Log($"Gridstate length: {gridState.Length:N0}");

            // Place objects representing blocked cells and open paths
            var placerService = new GridPlacerService(new GridPlacerOptions
            {
                parent = options.parent,
                blockPrefab = options.blockPrefab,
                pathPrefab = options.pathPrefab,
                exitPrefab = options.exitPrefab,
                cellSize = options.cellSize,
                rows = options.generatorOptions.gridRows,
                gridState = gridState
            });

            placerService.PlaceObjects();

            // Instantiate and place the player prefab
            var (startX, startY) = GridUtils.GetCoordinates(options.generatorOptions.startIndex, options.generatorOptions.gridRows);
            var startPos = GridUtils.GetPositionByIndex(startX, startY, options.cellSize) + 
                new Vector3(options.cellSize * 0.5f, 1f, options.cellSize * 0.5f);
            var playerGo = GameObject.Instantiate(options.playerPrefab, startPos, Quaternion.identity);

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
