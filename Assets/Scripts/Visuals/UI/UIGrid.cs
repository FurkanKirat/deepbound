using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals.UI
{
    public abstract class UIGrid<TItem, TData> : MonoBehaviour where TItem : MonoBehaviour
    {
        [Header("UI Grid")]
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private Transform contentParent;
        [SerializeField] private TItem itemPrefab;
        
        protected readonly List<TItem> SpawnedItems = new();

        public GridLayoutGroup.Corner StartCorner
        {
            get => gridLayoutGroup.startCorner;
            set => gridLayoutGroup.startCorner = value;
        }

        public GridLayoutGroup.Constraint Constraint
        {
            get => gridLayoutGroup.constraint;
            set => gridLayoutGroup.constraint = value;
        }

        public int ConstraintCount
        {
            get => gridLayoutGroup.constraintCount;
            set => gridLayoutGroup.constraintCount = value;
        }
        
        public virtual void SetItems(IEnumerable<TData> items)
        {
            Clear();

            foreach (var item in items)
            {
                var go = Instantiate(itemPrefab, contentParent);
                var component = go.GetComponent<TItem>();
                SetupAction(component, item);
                SpawnedItems.Add(component);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
        }

        public void SetItem(int index, TData data)
        {
            if (index < 0 || index >= SpawnedItems.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            var uiItem = SpawnedItems[index];
            SetupAction(uiItem, data);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
        }
        
        public void Clear()
        {
            foreach (var item in SpawnedItems)
            {
                Destroy(item.gameObject);
            }
            SpawnedItems.Clear();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
        }
        
        protected abstract void SetupAction(TItem item, TData data);
    }
}