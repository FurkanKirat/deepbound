using Data.RegistrySystem;
using Systems.CommandSystem.Commands;

namespace Data.Registrars
{
    public class CommandRegistrar : IRegistrar
    {
        public void RegisterAll()
        {
            CommandRegistry.Register(new GiveCommand());
            CommandRegistry.Register(new HelpCommand());
            CommandRegistry.Register(new PingCommand());
            CommandRegistry.Register(new PosCommand());
            CommandRegistry.Register(new LogCommand());
            CommandRegistry.Register(new MemoryCommand());
            CommandRegistry.Register(new FpsCommand());
            CommandRegistry.Register(new SetBlockCommand());
            CommandRegistry.Register(new BreakBlockCommand());
            CommandRegistry.Register(new TeleportCommand());
            CommandRegistry.Register(new SaveCommand());
            CommandRegistry.Register(new SummonCommand());
        }
    }
}