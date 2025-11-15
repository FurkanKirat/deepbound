namespace Config
{
    public class InventoryConfig
    {
        public PlayerInventoryConfig Player { get; set; }
        public ChestInventoryConfig Chest { get; set; }
    }

    public class PlayerInventoryConfig
    {
        public int HotbarSlotCount { get; set; }
        public int MainInventorySlotCount { get; set; }
        public int SlotsPerRow { get; set; }
        public int HotbarStartIndex { get; set; }
        public int AccessoryCount { get; set; }
    }

    public class ChestInventoryConfig
    {
        public int SlotCount { get; set; }
        public int SlotsPerRow { get; set; }
    }
}