using Core.Context;
using UnityEngine;

namespace GameLoop
{
    public class LogicGameLoop : MonoBehaviour
    {
        public TickManager TickManager { get; private set; }

        private TickContext _tickContext;
        private bool _isInitialized = false;

        public void Initialize(TickManager tickManager, TickContext tickContext)
        {
            _tickContext = tickContext;
            TickManager = tickManager;
            TickManager.Start();
            _isInitialized = true;
        }

        private void Update()
        {
            if(!_isInitialized) return;
            
            TickManager.Tick(Time.deltaTime, _tickContext);
        }
    }
}