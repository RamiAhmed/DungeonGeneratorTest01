using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Options.Components
{
    public sealed class GridEnvironmentOptionsAuthoringComponent : BaseOptionsAuthoringComponent<GridEnvironmentOptions>, IDeclareReferencedPrefabs
    {
        public GameObject blockPrefab;
        public GameObject pathPrefab;
        public GameObject exitPrefab;

        protected override GridEnvironmentOptions GetOptions(GameObjectConversionSystem conversionSystem)
        {
            return new GridEnvironmentOptions
            {
                blockPrefab = conversionSystem.GetPrimaryEntity(blockPrefab),
                pathPrefab = conversionSystem.GetPrimaryEntity(pathPrefab),
                exitPrefab = conversionSystem.GetPrimaryEntity(exitPrefab)
            };
        }

        void IDeclareReferencedPrefabs.DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(blockPrefab);
            referencedPrefabs.Add(pathPrefab);
            referencedPrefabs.Add(exitPrefab);
        }
    }
}
