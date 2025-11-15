using Data.Models;

namespace Systems.InputSystem
{
    public static class InputCache
    {
        public static PlayerInput Current;
        
        public static void Update(bool isUiInterrupt)
        {
            if (isUiInterrupt)
            {
                Current = new PlayerInput(0, JumpInputPhase.None);
                return;
            }
            float x = 0;
            if (InputBindingManager.Bindings.GetBinding(InputAction.MoveLeft).IsPressed) x -= 1;
            if (InputBindingManager.Bindings.GetBinding(InputAction.MoveRight).IsPressed) x += 1;

            JumpInputPhase jumpPhase = JumpInputPhase.None;

            var jumpKey = InputBindingManager.Bindings.GetBinding(InputAction.Jump);
            if (jumpKey.IsPressed)
                jumpPhase = JumpInputPhase.Pressed;
            else if (jumpKey.IsReleased)
                jumpPhase = JumpInputPhase.Released;
            else if (jumpKey.IsHeld)
                jumpPhase = JumpInputPhase.Held;

            Current = new PlayerInput(x, jumpPhase);
            
        }

    }
}