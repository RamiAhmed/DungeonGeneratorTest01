//using Assets.Core.Grid;
//using System;
//using System.Linq;
//using UnityEngine;

//namespace Assets.Core
//{
//    public class DungeonLoaderService
//    {
//        private DungeonOptions _options;

//        public byte[] gridState { get; private set; }

//        public bool[] floodState { get; private set; }

//        private GameObject _player;

//        public void Generate(DungeonOptions options)
//        {
//            _options = options;
//            UnityEngine.Debug.Log($"Loading dungeon with options: {options}");

//            ResetPreviousPlayer();
//            ResetPreviousDungeon();
//            CreateGrid();
//            PlaceObjects();
//            CreatePlayer();
//        }

//        private void ResetPreviousPlayer()
//        {
//            var resetService = new PlayerResetService();
//            resetService.ResetPlayer(_player);
//        }

//        private void ResetPreviousDungeon()
//        {
//            var resetService = new DungeonResetService();
//            resetService.DestroyDungeon(new DungeonResetOptions
//            {
//                parent = _options?.parent,
//            });
//        }

//        private void CreateGrid()
//        {
//            using (var service = new GridGeneratorService())
//            {
//                service.Generate(_options.generatorOptions);

//                var (gridState, floodState) = service.Complete();
//                this.gridState = gridState
//                    .ToArray();

//                this.floodState = floodState
//                    .Select(s => s == GridStateConstants.FLOODED)
//                    .ToArray();
//            }
//        }

//        private void PlaceObjects()
//        {
//            // Place objects representing blocked cells and open paths
//            var placerService = new GridPlacerService(new GridPlacerOptions
//            {
//                parent = _options.parent,
//                blockPrefab = _options.blockPrefab,
//                pathPrefab = _options.pathPrefab,
//                exitPrefab = _options.exitPrefab,
//                cellSize = _options.cellSize,
//                rows = _options.generatorOptions.gridRows,
//                gridState = gridState
//            });

//            placerService.PlaceObjects();
//        }

//        private void CreatePlayer()
//        {
//            var playerCreationService = new PlayerCreationService();
//            _player = playerCreationService.CreatePlayer(new PlayerCreationOptions
//            {
//                playerPrefab = _options.playerPrefab,
//                cellSize = _options.cellSize,
//                gridRows = _options.generatorOptions.gridRows,
//                startIndex = _options.generatorOptions.startIndex
//            });
//        }
//    }

//}
