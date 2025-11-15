using Data.Database;
using UnityEngine;
using UnityEngine.UI;
using Visuals.UI.InventorySystem.Slots;

namespace Visuals.UI.InventorySystem.Panels
{
    public abstract class GridInventoryPanel<TSlot> : BaseInventoryPanel<TSlot>
        where TSlot : IItemSlotUI
    {
        [Header("Grid References")]
        [SerializeField] protected RectTransform slotContainer;
        [SerializeField] protected GameObject slotPrefab;
        [SerializeField] protected GridLayoutGroup gridLayoutGroup;
        
        protected virtual int SlotsPerRow => Configs.GameConfig.Inventory.Player.SlotsPerRow;
        protected virtual GridLayoutGroup.Corner StartCorner => GridLayoutGroup.Corner.UpperLeft;
        protected virtual GridLayoutGroup.Constraint Constraint => GridLayoutGroup.Constraint.FixedColumnCount;
        protected override void GenerateSlots()
        {
            Slots.Clear();

            if (gridLayoutGroup != null)
            {
                gridLayoutGroup.constraintCount = SlotsPerRow;
                gridLayoutGroup.startCorner = StartCorner;
                gridLayoutGroup.constraint = Constraint;
            }
            
            for (int i = 0; i < SlotCount; i++)
            {
                var obj = Instantiate(slotPrefab, slotContainer);
                var slotUI = obj.GetComponent<TSlot>();
                slotUI.Init(InventoryOwner, i + SlotOffset, UIInventoryType);
                Slots.Add(slotUI);
            }

            SetSlotContainerHeight();
            LayoutRebuilder.ForceRebuildLayoutImmediate(slotContainer);
        }
        
        private void SetSlotContainerHeight()
        {
            if (gridLayoutGroup == null) return;

            int rowCount = Mathf.CeilToInt((float)SlotCount / SlotsPerRow);
            float cellHeight = gridLayoutGroup.cellSize.y;
            float spacing = gridLayoutGroup.spacing.y;

            float totalHeight = rowCount * cellHeight + (rowCount - 1) * spacing;

            var layoutElement = slotContainer.GetComponent<LayoutElement>();
            if (layoutElement == null)
                layoutElement = slotContainer.gameObject.AddComponent<LayoutElement>();

            layoutElement.preferredHeight = totalHeight;
            
        }

    }

}