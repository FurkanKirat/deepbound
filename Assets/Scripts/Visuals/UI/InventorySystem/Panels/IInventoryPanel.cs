using Interfaces;
using Systems.InventorySystem;
using UnityEngine;

namespace Visuals.UI.InventorySystem.Panels
{
    public interface IInventoryPanel : 
        IUIPanel,
        IInitializable<IInventoryOwner>
    {
        IItemSlotCollection Inventory { get; }
    }
}