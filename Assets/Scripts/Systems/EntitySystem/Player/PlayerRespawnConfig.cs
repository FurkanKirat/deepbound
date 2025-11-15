using Systems.EntitySystem.Interfaces;

namespace Systems.EntitySystem.Player
{
    public class PlayerRespawnConfig
    {
        public readonly IPlayer Player;
        public float RemainingTime;

        public PlayerRespawnConfig(IPlayer player, float remainingTime)
        {
            Player = player;
            RemainingTime = remainingTime;
        }
    }
}