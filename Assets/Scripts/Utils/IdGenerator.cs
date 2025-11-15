using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    using System.Threading;

    public static class IdGenerator
    {
        private static int _currentId = 0;

        public static int NewId => Interlocked.Increment(ref _currentId) - 1;

        public static void RegisterId(int id)
        {
            _currentId = Mathf.Max(++id, _currentId);
        }
        
        public static void InitializeFromSave(IEnumerable<int> existingIds)
        {
            var existingIdArr = existingIds as int[] ?? existingIds.ToArray();
            if (existingIdArr.Length == 0)
            {
                _currentId = 1;
                return;
            }

            _currentId = existingIdArr.Max() + 1;
        }


        public static void Reset()
        {
            _currentId = 0;
        }
    }

}