﻿using Assets.Core;
using System.Linq;
using UnityEngine;

public class DebugTestStarter : MonoBehaviour
{
    [Range(0, 100000)]
    public int gridRows = 100;

    [Range(0, 100000)]
    public int gridCols = 100;

    [Range(-0.9f, 0.9f)]
    public float sensitivity = 0.3f;

    [Range(0, 10)]
    public int cellSize = 1;

    [Range(1, 20)]
    public int innerloopBatchCount = 1;

    private bool[] _gridState;
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
                innerloopBatchCount = innerloopBatchCount
            });

            var (gridState, floodState) = service.Complete();
            _gridState = gridState
                .Select(s => s > 0)
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
    }

    void OnDrawGizmos()
    {
        if (_gridState == null)
            return;

        for (int i = 0; i < _gridState.Length; i++)
        {
            var (x, y) = GridUtils.GetCoordinates(i, gridRows);
            var position = GridUtils.GetPositionByCoordinates(x, y, cellSize);

            Gizmos.color = x == 1 && y == 1 ? Color.yellow : _floodState[i] ? Color.blue : (_gridState[i] ? Color.red : Color.green);
            Gizmos.DrawCube(position, Vector3.one * cellSize);
        }
    }
}
