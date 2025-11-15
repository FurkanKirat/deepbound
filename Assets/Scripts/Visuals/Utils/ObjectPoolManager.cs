using System;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals.Utils
{
    public class ObjectPoolManager<TKey>
    {
        private readonly Dictionary<TKey, IObjectPool> _pools = new();

        public void RegisterPool<T>(TKey key, ObjectPool<T> pool) where T : Component
        {
            _pools.TryAdd(key, pool);
        }

        public T Get<T>(TKey key) where T : Component
        {
            if (_pools.TryGetValue(key, out var pool))
            {
                return ((ObjectPool<T>)pool).Get();
            }
            throw new Exception($"Pool not found for {key}");
        }

        public void Release<T>(TKey key, T obj) where T : Component
        {
            if (_pools.TryGetValue(key, out var pool))
            {
                ((ObjectPool<T>)pool).Release(obj);
            }
            else
            {
                throw new Exception($"Pool not found for {key}");
            }
        }
    }

}