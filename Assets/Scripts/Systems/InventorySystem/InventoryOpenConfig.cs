namespace Systems.InventorySystem
{
    public class InventoryOpenConfig
    {
        public readonly IInventoryOwner Owner;
        public readonly SlotCollectionType[] InventoryTypes;

        public InventoryOpenConfig(IInventoryOwner owner, SlotCollectionType[] inventoryTypes)
        {
            Owner = owner;
            InventoryTypes = inventoryTypes;
        }
    }
}