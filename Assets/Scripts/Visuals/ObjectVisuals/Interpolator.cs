using System.Collections.Generic;
using UnityEngine;

namespace Visuals.ObjectVisuals
{
    public class Interpolator
    {
        private struct Snapshot
        {
            public float Time;
            public Vector3 Position;
        }

        private readonly Queue<Snapshot> _snapshots = new();
        private readonly float _interpolationBackTime;
        private const int MaxSnapshots = 32;

        private Vector3 _lastInterpolated;

        public Interpolator(float interpolationBackTime = 0.1f)
        {
            _interpolationBackTime = interpolationBackTime;
        }

        public void Reset(Vector3 start)
        {
            _snapshots.Clear();
            _snapshots.Enqueue(new Snapshot { Time = Time.time, Position = start });
            _lastInterpolated = start;
        }

        public void Tick(Vector3 newPos)
        {
            _snapshots.Enqueue(new Snapshot { Time = Time.time, Position = newPos });
            
            while (_snapshots.Count > MaxSnapshots)
                _snapshots.Dequeue();
        }

        public Vector3 GetInterpolated()
        {
            if (_snapshots.Count == 0)
                return _lastInterpolated;

            float renderTime = Time.time - _interpolationBackTime;
            var arr = _snapshots.ToArray();

            if (arr.Length < 2)
            {
                _lastInterpolated = arr[^1].Position;
                return _lastInterpolated;
            }

            if (renderTime <= arr[0].Time)
            {
                _lastInterpolated = arr[0].Position;
                return _lastInterpolated;
            }

            for (int i = 0; i < arr.Length - 1; i++)
            {
                var older = arr[i];
                var newer = arr[i + 1];

                if (renderTime >= older.Time && renderTime <= newer.Time)
                {
                    float t = Mathf.InverseLerp(older.Time, newer.Time, renderTime);
                    _lastInterpolated = Vector3.Lerp(older.Position, newer.Position, t);
                    return _lastInterpolated;
                }
            }
            
            var lastOlder = arr[^2];
            var lastNewer = arr[^1];

            float extrapolationT = Mathf.InverseLerp(lastOlder.Time, lastNewer.Time, renderTime);
    
            _lastInterpolated = Vector3.LerpUnclamped(lastOlder.Position, lastNewer.Position, extrapolationT);
            return _lastInterpolated;
        }
    }
}
