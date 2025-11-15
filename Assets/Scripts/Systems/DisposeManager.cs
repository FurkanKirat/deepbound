using System;
using System.Collections.Generic;

namespace Systems
{
    public class DisposeManager
    {
        private readonly List<IDisposable> _disposables = new(); 
        public void Register(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }
        
        public void DisposeAll()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _disposables.Clear();
        }
    }
}