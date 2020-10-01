using System;
using Unity.Entities;

namespace Assets.Core.Grid
{
    public struct GridEntityComponentData : IComponentData, IEquatable<GridEntityComponentData>
    {
        public int Index;

        bool IEquatable<GridEntityComponentData>.Equals(GridEntityComponentData other)
        {
            return other.Index == Index;
        }
    }
}
