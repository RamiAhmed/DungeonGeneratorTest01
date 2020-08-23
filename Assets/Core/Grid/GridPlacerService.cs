using UnityEngine;

namespace Assets.Core.Grid
{
    public class GridPlacerService
    {
        private readonly GridPlacerOptions _options;

        public GridPlacerService(GridPlacerOptions options)
        {
            _options = options;
        }

        public void PlaceObjects()
        {
            var gridState = _options.gridState;
            for (int i = 0; i < gridState.Length; i++)
            {
                var prefab = GetPrefab(i);
                var position = GridUtils.GetPositionByIndex(i, _options.rows, _options.cellSize);

                var go = Object.Instantiate(prefab, position, Quaternion.identity, _options.parent);

                var scale = go.transform.localScale;
                go.transform.localScale = new Vector3(scale.x * _options.cellSize, scale.y, scale.z * _options.cellSize);

                go.name += $"(index: {i})";
            }
        }

        private GameObject GetPrefab(int index)
        {
            if (_options.gridState[index] == GridStateConstants.EXIT)
                return _options.exitPrefab;

            if (_options.gridState[index] == GridStateConstants.BLOCKED)
                return _options.blockPrefab;
                
            return _options.pathPrefab;
        }
    }

    public class GridPlacerOptions
    {
        public Transform parent;
        public GameObject blockPrefab;
        public GameObject pathPrefab;
        public GameObject exitPrefab;

        public byte[] gridState;

        public int rows;
        public int cellSize;
    }
}
