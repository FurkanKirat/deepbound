using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Systems.InputSystem
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InputAction : ushort
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        Jump,
        Escape,
        
        Interact,
        OpenInventory,
        OpenCrafting,
        OpenChat,
        OpenMinimap,
        PrimaryUse,
        SecondaryUse,
        
        Slot1,
        Slot2,
        Slot3,
        Slot4,
        Slot5,
        Slot6,
        Slot7,
        Slot8,
        Slot9,
        
        SlotStart = Slot1,
        SlotEnd = Slot9,
        
        Screenshot

    }

}