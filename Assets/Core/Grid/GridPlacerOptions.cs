using System;
using UnityEngine;

namespace Assets.Core.Grid
{
    [Serializable]
    public class GridPlacerOptions
    {
        public Transform parent;
        public GameObject blockPrefab;
        public GameObject pathPrefab;
        public GameObject exitPrefab;

        public int rows;
        public int cellSize;
    }
}
