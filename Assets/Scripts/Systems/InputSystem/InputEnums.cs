namespace Systems.InputSystem
{
    public enum JumpInputPhase : byte
    {
        None, 
        Pressed, 
        Released, 
        Held
    }

    public static class JumpInputPhaseExtensions
    {
        public static bool ShouldJump(this JumpInputPhase phase)
        {
            return phase switch
            {
                JumpInputPhase.Pressed or JumpInputPhase.Held => true,
                _ => false
            };
        }
    }
}