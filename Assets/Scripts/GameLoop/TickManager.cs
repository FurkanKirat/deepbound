using System.Collections.Generic;
using Core.Context;

namespace GameLoop
{
    public class TickManager
    {
        private readonly List<ITickable> _listeners = new();
        private const float TickInterval = 0.05f;
        private float _tickTimer = 0f;
        
        private bool _isRunning = false;

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
        }
        
        public void Register(ITickable listener)
        {
            _listeners.Add(listener);
        }

        public void Unregister(ITickable listener)
        {
            _listeners.Remove(listener);
        }

        public void Stop()
        {
            _isRunning = false;
            _listeners.Clear();
            _tickTimer = 0f;
        }

        public void Pause()
        {
            _isRunning = false;
        }

        public void Tick(float deltaTime, TickContext context)
        {
            if (!_isRunning) return;
            _tickTimer += deltaTime;
            while (_tickTimer >= TickInterval)
            {
                foreach (var listener in _listeners)
                    listener.Tick(TickInterval, context);
                _tickTimer -= TickInterval;
            }
        }
    }

}