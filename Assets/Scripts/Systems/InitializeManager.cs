using System.Collections.Generic;
using Core;
using Core.Events;
using Interfaces;

namespace Systems
{
    public class InitializeManager
    {
        private readonly List<IInitializable> _initializables = new();

        public void Register(IInitializable initializable)
        {
            _initializables.Add(initializable);
        }

        public void InitializeAll()
        {
            foreach (var init in _initializables)
            {
                init.Initialize();
            }

            GameEventBus.Publish(new AllSystemsInitializedEvent());
        }
    }
}