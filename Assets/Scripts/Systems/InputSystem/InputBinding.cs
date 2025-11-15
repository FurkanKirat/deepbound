using System;
using System.Collections.Generic;
using System.Linq;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.Input;
using UnityEngine;

namespace Systems.InputSystem
{
    public class InputBinding : ISaveable<InputBindingSaveData>
    {
        public InputDevice Device { get; }
        public List<KeyCode> Keys { get; }

        private InputBinding(InputDevice device, List<KeyCode> keys)
        {
            Device = device;
            Keys = keys;
        }

        public static InputBinding Create(InputDevice device, List<KeyCode> keys)
        {
            return new InputBinding(device, keys);
        }

        public static InputBinding Load(InputBindingSaveData saveData)
        {
            if (saveData?.Keys == null)
                return null;
            return new InputBinding(saveData.Device, saveData.Keys.Select(keyStr => 
                    Enum.TryParse<KeyCode>(keyStr, out var result) 
                        ? result 
                        : KeyCode.None)
                .ToList()
            );
        }
        public InputBindingSaveData ToSaveData()
        {
            return new InputBindingSaveData
            {
                Device = Device,
                Keys = Keys.Select((code, _) => code.ToString()).ToList()
            };
        }

        public bool IsPressed
        {
            get
            {
                foreach (var key in Keys)
                {
                    if (!Input.GetKey(key))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsReleased
        {
            get
            {
                foreach (var key in Keys)
                {
                    if (Input.GetKeyUp(key))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsHeld
        {
            get
            {
                foreach (var key in Keys)
                {
                    if (!Input.GetKeyDown(key))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }

}