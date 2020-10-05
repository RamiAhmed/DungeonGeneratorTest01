using System;
using Unity.Entities;

namespace Assets.Core.Options
{
    public struct GridGeneratorOptions : IComponentData, IEquatable<GridGeneratorOptions>
    {
        public int gridRows;
        public int gridCols;
        public int cellSize;
        public float sensitivity;
        public int innerloopBatchCount;
        public int startX;
        public int startY;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this);
        }

        public override int GetHashCode()
        {
            return gridRows.GetHashCode() + gridCols.GetHashCode() + cellSize.GetHashCode() + startX.GetHashCode() + startY.GetHashCode();
        }

        bool IEquatable<GridGeneratorOptions>.Equals(GridGeneratorOptions other)
        {
            return gridRows == other.gridRows &&
                   gridCols == other.gridCols &&
                   cellSize == other.cellSize &&
                   startX == other.startX &&
                   startY == other.startY;
        }
    }
}
