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
        [ReadOnly]
        private readonly NativeArray<byte> _floodState;

        private NativeArray<byte> _gridState;

        public GridFillerJob(NativeArray<byte> gridState, NativeArray<byte> floodState)
        {
            _gridState = gridState;
            _floodState = floodState;
        }

        public void Execute(int index)
        {
            if (_floodState[index] == GridStateConstants.FLOODED || _gridState[index] != GridStateConstants.FREE)
                return;

            //UnityEngine.Debug.Log($"Blocking cell at index: {index}");
            _gridState[index] = GridStateConstants.BLOCKED;
        }
    }
}
