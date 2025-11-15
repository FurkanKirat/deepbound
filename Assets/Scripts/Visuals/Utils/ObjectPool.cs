using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Visuals.Utils
{
    public class ObjectPool<T> : IObjectPool
        where T : Component 
    {
        private readonly List<T> _all = new();
        private readonly Func<T> _factory;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _factory = () =>
            {
                var obj = Object.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                return obj;
            };

            for (int i = 0; i < initialSize; i++)
                CreateNew();
        }
        
        public ObjectPool(Func<T> factory, int initialSize, Transform parent = null)
        {
            _factory = () =>
            {
                var obj = factory();
                if (parent != null) obj.transform.SetParent(parent, false);
                obj.gameObject.SetActive(false);
                return obj;
            };

            for (int i = 0; i < initialSize; i++)
                CreateNew();
        }
        
        private T CreateNew()
        {
            var obj = _factory();
            _all.Add(obj);
            return obj;
        }

        public T Get()
        {
            T getObject = null;
            foreach (var obj in _all)
            {
                if (!obj.gameObject.activeSelf)
                {
                    getObject = obj;              
                    break;
                }
            }
            getObject ??= CreateNew();
            getObject.gameObject.SetActive(true);
            return getObject;
        }
        
        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        public void ReleaseAll()
        {
            foreach (var obj in _all)
            {
                if (obj.gameObject.activeSelf)
                    Release(obj);
            }
        }
    }

}