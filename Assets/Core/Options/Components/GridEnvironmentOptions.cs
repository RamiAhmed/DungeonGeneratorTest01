using System;
using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Options
{
    public struct GridEnvironmentOptions : ISystemStateSharedComponentData, IEquatable<GridEnvironmentOptions>
    {
        public Entity blockPrefab;
        public Entity pathPrefab;
        public Entity exitPrefab;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }

        public override int GetHashCode()
        {
            return blockPrefab.GetHashCode() + pathPrefab.GetHashCode() + exitPrefab.GetHashCode();
        }

        bool IEquatable<GridEnvironmentOptions>.Equals(GridEnvironmentOptions other)
        {
            return blockPrefab == other.blockPrefab &&
                   pathPrefab == other.pathPrefab &&
                   exitPrefab == other.exitPrefab;
        }
    }
}
