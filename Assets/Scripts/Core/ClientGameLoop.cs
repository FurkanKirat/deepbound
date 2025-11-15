using Visuals.Interfaces;

namespace Core
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ClientGameLoop : MonoBehaviour
    {
        public static ClientGameLoop Instance { get; private set; }

        private readonly List<IClientTickable> _tickables = new ();
        private readonly List<IClientLateTickable> _lateTickables = new ();

        public void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
        
        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
        
        public void Register(IClientTickable tickable)
        {
            _tickables.Add(tickable);
        }

        public void Unregister(IClientTickable tickable)
        {
            _tickables.Remove(tickable);
        }

        public void Register(IClientLateTickable tickable)
        {
            _lateTickables.Add(tickable);
        }

        public void Unregister(IClientLateTickable tickable)
        {
            _lateTickables.Remove(tickable);
        }
        
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            var tickablesSnapshot = new List<IClientTickable>(_tickables);
            foreach (var tickable in tickablesSnapshot)
            {
                tickable.ClientTick(deltaTime);
            }
        }

        private void LateUpdate()
        {
            float deltaTime = Time.deltaTime;
            var lateTickablesSnapshot = new List<IClientLateTickable>(_lateTickables);
            foreach (var tickable in lateTickablesSnapshot)
            {
                tickable.ClientLateTick(deltaTime);
            }
        }
        
    }

}