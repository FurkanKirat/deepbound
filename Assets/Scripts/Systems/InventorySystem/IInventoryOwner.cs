namespace Systems.InventorySystem
{
    public interface IInventoryOwner
    {
        InventoryManager InventoryManager { get; }
        InventoryOwnerType InventoryOwnerType { get; }
    }
}