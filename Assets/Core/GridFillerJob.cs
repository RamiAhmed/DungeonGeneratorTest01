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
    public struct GridFillerJob : IJobParallelFor
    {
        public const byte FREE = 0;
        public const byte BLOCKED = 1;
        public const byte FLOODED = byte.MaxValue - 1;

        private NativeArray<byte> _gridState;
        private NativeArray<byte> _floodState;

        public GridFillerJob(NativeArray<byte> gridState, NativeArray<byte> floodState)
        {
            _gridState = gridState;
            _floodState = floodState;
        }

        public void Execute(int index)
        {
            if (_floodState[index] == FLOODED || _gridState[index] != FREE)
                return;

            //UnityEngine.Debug.Log($"Blocking cell at index: {index}");
            _gridState[index] = BLOCKED;
        }
    }
}
