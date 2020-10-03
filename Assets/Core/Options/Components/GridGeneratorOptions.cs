using System;
using Unity.Entities;

namespace Assets.Core.Options
{
    [Serializable]
    public struct GridGeneratorOptions : ISystemStateSharedComponentData, IEquatable<GridGeneratorOptions>
    {
        public int gridRows;
        public int gridCols;
        public int cellSize;
        public float sensitivity;
        public int innerloopBatchCount;
        public int startIndex;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this);
        }

        public override int GetHashCode()
        {
            return gridRows.GetHashCode() + gridCols.GetHashCode() + cellSize.GetHashCode() + startIndex.GetHashCode();
        }

        bool IEquatable<GridGeneratorOptions>.Equals(GridGeneratorOptions other)
        {
            return gridRows == other.gridRows &&
                   gridCols == other.gridCols &&
                   cellSize == other.cellSize &&
                   startIndex == other.startIndex;
        }
    }
}
