using Assets.Core.Grid;
using System;
using UnityEngine;

namespace Assets.Core
{
    [Serializable]
    public class DungeonOptions
    {
        public GridGeneratorOptions generatorOptions;

        public Transform parent;

        public GameObject blockPrefab;
        public GameObject pathPrefab;
        public GameObject playerPrefab;
        public GameObject exitPrefab;

        public GameObject cameraPrefab;

        public int cellSize;

        public GridPlacerOptions GetGridPlacerOptions()
        {
            return new GridPlacerOptions
            {
                parent = parent,
                blockPrefab = blockPrefab,
                pathPrefab = pathPrefab,
                exitPrefab = exitPrefab,
                cellSize = cellSize,
                rows = generatorOptions.gridRows
            };
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
