using Assets.Core.Grid;
using System.Linq;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(GridEnvironmentSystem))]
    public class PlayerCreationSystem : BaseGridDependentSystem
    {
        private EntityArchetype _playerArchetype;

        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
            _playerArchetype = EntityManager.CreateArchetype(
                typeof(Translation),
                typeof(Rotation), 
                typeof(PlayerComponentData), 
                typeof(PlayerMovementComponentData));
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");

            var playerEntity = EntityManager.CreateEntity(_playerArchetype);
            EntityManager.SetComponentData(playerEntity, new PlayerComponentData
            {
                Index = 1 // TODO: use indices for multiplayer support
            });

            var options = DebugTestStarter.GetOptions().GetGridPlacerOptions(); // TODO: get options in proper way
            var startIndex = GetGridState().Single(state => state == GridStateConstants.START);
            var startPosition = GridUtils.GetPositionByIndex(startIndex, options.rows, options.cellSize);

            // add some player height 
            startPosition = new Vector3(startPosition.x, startPosition.y + 1.5f, startPosition.z);

            EntityManager.SetComponentData(playerEntity, new Translation { Value = startPosition });
        }

        protected override void OnUpdate()
        {
            /* NOOP */
        }
    }
}
