using System;
using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Options
{
    [Serializable]
    public struct GridEnvironmentOptions : ISystemStateSharedComponentData, IEquatable<GridEnvironmentOptions>
    {
        public GameObject blockPrefab;
        public GameObject pathPrefab;
        public GameObject exitPrefab;

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
