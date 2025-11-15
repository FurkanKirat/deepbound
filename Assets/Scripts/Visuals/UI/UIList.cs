using System;
using System.Collections.Generic;
using UnityEngine;
using Visuals.Utils;

namespace Visuals.UI
{
    public class UIList : MonoBehaviour
    {
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private int initialSize = 10;

        private ObjectPool<Component> _pool;
        private List<Component> SpawnedItems { get; } = new();
        public IReadOnlyList<Component> Items => SpawnedItems;
        
        public void SetItems<TData, TComponent>(IEnumerable<TData> items, Action<TComponent, TData> setupAction) 
            where TComponent : Component
        {
            _pool ??= new ObjectPool<Component>(() =>
            {
                var go = Instantiate(itemPrefab, contentParent);
                return go.GetComponent<TComponent>();
            }, initialSize);
            
            Clear();

            foreach (var item in items)
            {
                var comp = (TComponent)_pool.Get();
                setupAction?.Invoke(comp, item);
                SpawnedItems.Add(comp);
            }
        }

        public T Get<T>(int index) where T : Component
        {
            return (T)SpawnedItems[index];
        }
      
        public void Clear()
        {
            foreach (var comp in SpawnedItems)
                _pool.Release(comp);
            SpawnedItems.Clear();
        }
        
        public void SetItemPrefab(GameObject prefab)
        {
            itemPrefab = prefab;
        }
    }
}