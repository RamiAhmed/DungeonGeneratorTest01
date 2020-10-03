using System;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Core.Grid
{
    public struct GridSharedSystemComponentData : ISystemStateSharedComponentData, IEquatable<GridSharedSystemComponentData>
    {
        public NativeArray<byte> GridState;

        bool IEquatable<GridSharedSystemComponentData>.Equals(GridSharedSystemComponentData other)
        {
            return GridState == other.GridState;
        }

        public override int GetHashCode()
        {
            return GridState.GetHashCode();
        }
    }
}
