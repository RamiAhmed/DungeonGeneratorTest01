using Assets.Core;
using Assets.Core.Grid;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class DebugTestStarter : MonoBehaviour
{
    [Header("Prefabs")]
    public Transform parent;
    public GameObject blockPrefab;
    public GameObject pathPrefab;
    public GameObject playerPrefab;

    [Header("Grid")]
    [Range(0, 100000)]
    public int gridRows = 100;

    [Range(0, 100000)]
    public int gridCols = 100;

    [Range(-0.9f, 0.9f)]
    public float sensitivity = 0.3f;

    [Range(0, 10)]
    public int cellSize = 1;

    public int startX = 1;
    public int startY = 1;

    [Header("Jobs")]
    [Range(1, 20)]
    public int innerloopBatchCount = 1;

    public Allocator allocatorType = Allocator.TempJob;

    private byte[] _gridState;
    private bool[] _floodState;

    // Start is called before the first frame update
    void Start()
    {
        using (var service = new DungeonGeneratorService())
        {
            service.Generate(new DungeonGeneratorOptions
            {
                gridRows = gridRows,
                gridCols = gridCols,
                sensitivity = sensitivity,
                innerloopBatchCount = innerloopBatchCount,
                allocatorType = allocatorType,
                startIndex = GridUtils.GetIndex(startX, startY, gridRows)
            });

            var (gridState, floodState) = service.Complete();
            _gridState = gridState
                .ToArray();

            _floodState = floodState
                .Select(s => s == GridStateConstants.FLOODED)
                .ToArray();
        }

        //var sb = new StringBuilder(_gridState.Length * 10);
        //for (int i = 0; i < _gridState.Length; i++)
        //{
        //    sb.Append(_gridState[i] ? "1" : "0");
        //    sb.Append(", ");
        //}

        //Debug.Log("Grid state: " + sb.ToString());
        Debug.Log($"Gridstate length: {_gridState.Length:N0}");

        // Place objects representing blocked cells and open paths
        var placerService = new GridPlacerService(new GridPlacerOptions
        {
            parent = parent,
            blockPrefab = blockPrefab,
            pathPrefab = pathPrefab,
            cellSize = cellSize,
            rows = gridRows,
            gridState = _gridState
        });

        placerService.PlaceObjects();

        // Instantiate and place the player prefab
        var startPos = GridUtils.GetPositionByIndex(startX, startY, cellSize) + new Vector3(cellSize * 0.5f, 1f, cellSize * 0.5f);
        var playerGo = GameObject.Instantiate(playerPrefab, startPos, Quaternion.identity);

        /// DEBUG STUFF
        FindObjectOfType<FreeLookCam>().Target = playerGo.transform;
    }

    void OnDrawGizmosSelected()
    {
        if (_gridState == null)
            return;

        for (int i = 0; i < _gridState.Length; i++)
        {
            var (x, y) = GridUtils.GetCoordinates(i, gridRows);
            var position = GridUtils.GetPositionByCoordinates(x, y, cellSize);

            Gizmos.color = _gridState[i] == GridStateConstants.START ? Color.yellow : _floodState[i] ? Color.blue : (_gridState[i] == GridStateConstants.BLOCKED ? Color.red : Color.green);
            Gizmos.DrawCube(position, Vector3.one * cellSize);
        }
    }
}
