using Systems.EntitySystem.Player;

namespace Systems.StartingSystem.Players
{
    public interface IPlayerProvider
    {
        PlayerLogic GetPlayer();
    }
}