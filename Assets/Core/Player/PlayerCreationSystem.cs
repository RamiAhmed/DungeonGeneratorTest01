using Assets.Core.Grid;
using Assets.Core.Options;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(GridEnvironmentSystem))]
    public class PlayerCreationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            Debug.Log($"{this}: OnStartRunning");

            var options = this.GetOptions<PlayerOptions>();

            var playerEntity = EntityManager.Instantiate(options.playerPrefab);
            EntityManager.AddComponentData(playerEntity, new PlayerComponentData
            {
                Index = PlayerConstants.LOCAL_PLAYER_INDEX
            });

            EntityManager.AddComponentData(playerEntity, new PlayerMovementComponentData { Velocity = float3.zero });

            var gridOptions = this.GetOptions<GridGeneratorOptions>();
            var startPosition = GridUtils.GetCellCenter(gridOptions.startX, gridOptions.startY, gridOptions.cellSize);

            // add some player height 
            startPosition = new Vector3(startPosition.x, startPosition.y + 5f, startPosition.z);

            EntityManager.SetComponentData(playerEntity, new Translation { Value = startPosition });
        }

        protected override void OnUpdate()
        {
            /* NOOP */
        }
    }
}
