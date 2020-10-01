using System;
using Unity.Collections;

namespace Assets.Core.Grid
{
    [Serializable]
    public class GridGeneratorOptions
    {
        public int gridRows;
        public int gridCols;
        public float sensitivity;
        public int innerloopBatchCount;
        public Allocator allocatorType;
        public int startIndex;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this);
        }
    }
}
