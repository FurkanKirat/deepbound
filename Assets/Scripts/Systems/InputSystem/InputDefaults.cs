using System.Collections.Generic;
using UnityEngine;

namespace Systems.InputSystem
{
    public static class InputDefaults
    {
        public static readonly Dictionary<InputAction, ActionBinding> Bindings = new()
        {
            {
                InputAction.MoveLeft, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.A }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton14 }) // Left
                )
            },
            {
                InputAction.MoveRight, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.D }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton15 }) // Right
                )
            },
            {
                InputAction.MoveUp, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.W }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton12 }) // Up
                )
            },
            {
                InputAction.MoveDown, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.S }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton13 }) // Down
                )
            },
            {
                InputAction.Jump, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Space }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton0 }) // A button
                )
            },
            {
                InputAction.Escape, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Escape }),
                    null)
            },
            {
                InputAction.OpenChat, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Return }),
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.KeypadEnter }))
            },
            {
                InputAction.Interact, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.E }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton2 }) // X button
                )
            },
            {
                InputAction.OpenInventory, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Tab }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton3 }) // Y button
                )
            },
            {
                InputAction.OpenCrafting, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.C }),
                    null
                    )
            },
            {
                InputAction.OpenMinimap, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.M }),
                    null
                )
            },
            {
                InputAction.PrimaryUse, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Mouse, new List<KeyCode> { KeyCode.Mouse0 }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton5 }) // RightTrigger
                )
            },
            {
                InputAction.SecondaryUse, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Mouse, new List<KeyCode> { KeyCode.Mouse1 }),
                    InputBinding.Create(InputDevice.Gamepad, new List<KeyCode> { KeyCode.JoystickButton1 }) // B button
                )
            },
            {
                InputAction.Slot1, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha1 }),
                    null
                )
            },
            {
                InputAction.Slot2, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha2 }),
                    null
                )
            },
            {
                InputAction.Slot3, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha3 }),
                    null
                )
            },
            {
                InputAction.Slot4, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha4 }),
                    null
                )
            },
            {
                InputAction.Slot5, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha5 }),
                    null
                )
            },
            {
                InputAction.Slot6, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha6 }),
                    null
                )
            },
            {
                InputAction.Slot7, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha7 }),
                    null
                )
            },
            {
                InputAction.Slot8, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha8 }),
                    null
                )
            },
            {
                InputAction.Slot9, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.Alpha9 }),
                    null
                )
            },
            {
                InputAction.Screenshot, ActionBinding.Create(
                    InputBinding.Create(InputDevice.Keyboard, new List<KeyCode> { KeyCode.F12 }),
                    null
                )
            }
            
        };
    }
}
