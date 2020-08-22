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
                var prefab = gridState[i] ? _options.blockPrefab : _options.pathPrefab;
                var position = GridUtils.GetPositionByIndex(i, _options.rows, _options.cellSize);
                var go = Object.Instantiate(prefab, position, Quaternion.identity, _options.parent);

                var scale = go.transform.localScale;
                go.transform.localScale = new Vector3(scale.x * _options.cellSize, scale.y, scale.z * _options.cellSize);

                go.name += $"(index: {i})";
            }
        }
    }

    public class GridPlacerOptions
    {
        public Transform parent;
        public GameObject blockPrefab;
        public GameObject pathPrefab;

        public bool[] gridState;

        public int rows;
        public int cellSize;
    }
}
