using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Systems.InputSystem
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InputDevice
    {
        Keyboard,
        Mouse,
        Gamepad
    }
}