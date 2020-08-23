using Assets.Core;
using Assets.Core.Grid;
using Unity.Collections;
using UnityEngine;

public class DebugTestStarter : MonoBehaviour
{
    [Header("Prefabs")]
    public Transform parent;
    public GameObject blockPrefab;
    public GameObject pathPrefab;
    public GameObject playerPrefab;
    public GameObject exitPrefab;

    [Header("Grid")]
    [Range(0, 100000)]
    public int gridRows = 100;

    public int gridRowsRandom = 50;

    [Range(0, 100000)]
    public int gridCols = 100;

    public int gridColsRandom = 50;

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

    //public event Action CreateDungeonEvent; 

    private byte[] _gridState;
    private bool[] _floodState;

    private readonly DungeonLoaderService _dungeonLoader = new DungeonLoaderService();

    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    public static void CreateDungeonTest()
    {
        var obj = FindObjectOfType<DebugTestStarter>();
        obj.CreateDungeon();
    }

    public void CreateDungeon()
    {
        this.gridRows = Random.Range(this.gridRows - gridRowsRandom, this.gridRows + gridRowsRandom);
        this.gridCols = Random.Range(this.gridCols - gridColsRandom, this.gridCols + gridColsRandom);
        _dungeonLoader.Generate(new DungeonOptions
        {
            generatorOptions = new GridGeneratorOptions
            {
                gridRows = gridRows,
                gridCols = gridCols,
                sensitivity = sensitivity,
                innerloopBatchCount = innerloopBatchCount,
                allocatorType = allocatorType,
                startIndex = GridUtils.GetIndex(startX, startY, gridRows)
            },
            parent = parent,
            blockPrefab = blockPrefab,
            pathPrefab = pathPrefab,
            playerPrefab = playerPrefab,
            exitPrefab = exitPrefab,
            cellSize = cellSize,
        });

        _gridState = _dungeonLoader.gridState;
        _floodState = _dungeonLoader.floodState;
    }

    void OnDrawGizmosSelected()
    {
        if (_gridState == null)
            return;

        for (int i = 0; i < _gridState.Length; i++)
        {
            var (x, y) = GridUtils.GetCoordinates(i, gridRows);
            var position = GridUtils.GetPositionByCoordinates(x, y, cellSize);

            Gizmos.color = GetGizmoColor(i);
            Gizmos.DrawCube(position, Vector3.one * cellSize);
        }
    }

    private Color GetGizmoColor(int index)
    {
        if (_gridState[index] == GridStateConstants.START)
            return Color.yellow;

        if (_gridState[index] == GridStateConstants.EXIT)
            return Color.magenta;

        if (_floodState[index])
            return Color.green;

        if (_gridState[index] == GridStateConstants.FREE)
            return Color.gray;

        return Color.red;
    }
}
