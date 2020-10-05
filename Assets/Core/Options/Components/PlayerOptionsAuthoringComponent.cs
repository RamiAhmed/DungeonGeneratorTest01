using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Options.Components
{
    public sealed class PlayerOptionsAuthoringComponent : BaseOptionsAuthoringComponent<PlayerOptions>, IDeclareReferencedPrefabs
    {
        public GameObject playerPrefab;
        public float playerMoveSpeed;

        protected override PlayerOptions GetOptions(GameObjectConversionSystem conversionSystem)
        {
            return new PlayerOptions
            {
                playerMoveSpeed = playerMoveSpeed,
                playerPrefab = conversionSystem.GetPrimaryEntity(playerPrefab)
            };
        }

        void IDeclareReferencedPrefabs.DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(playerPrefab);
        }
    }
}
