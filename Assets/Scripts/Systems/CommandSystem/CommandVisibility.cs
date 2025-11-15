namespace Systems.CommandSystem
{
    public enum CommandVisibility : byte
    {
        Public,         // Anyone can use it
        Developer,      // Developers only (ctx.Player.IsDeveloper)
        DebugOnly,      // Only in debug build
        Admin,          // High authority players
        Moderator,      // Mid-level players
        BetaTester,     // Test team
        InternalOnly    // No one can use it in the game (automation/debug only)
    }

}