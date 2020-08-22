using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core
{
    public struct GridFloodGateJob : IJob
    {
        public const byte FREE = 0;
        public const byte UNSET = byte.MaxValue;
        public const byte FLOODED = byte.MaxValue - 1;

        private readonly int _cols;

        [ReadOnly]
        private readonly NativeArray<byte> _floodState;

        private NativeArray<byte> _gridState;

        public GridFloodGateJob(NativeArray<byte> gridState, NativeArray<byte> floodState, int cols)
        {
            _cols = cols;
            _gridState = gridState;
            _floodState = floodState;
        }

        public void Execute()
        {
            var watch = Stopwatch.StartNew();
            FloodGate();
            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }

        private void FloodGate()
        { 
            for (var index = _cols; index < _gridState.Length - _cols; index++)
            {
                if (_floodState[index] != FLOODED)
                    continue;

                var topLeft = index - 1 + _cols;
                var topRight = index + 1 + _cols;

                var freeNeighbourIdx = -1;
                if (_gridState[topLeft] == FREE)
                {
                    freeNeighbourIdx = topLeft;
                }
                else if (_gridState[topRight] == FREE)
                {
                    freeNeighbourIdx = topRight;
                }

                if (freeNeighbourIdx == -1)
                {
                    continue;
                }

                //UnityEngine.Debug.Log($"index: {index} has {freeNeighbours} free neighbours, delta: {freeNeighbourIdx - index}");
                _gridState[index + (freeNeighbourIdx - index) + 1] = FREE;
            }
        }
    }
}
