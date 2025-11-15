using Data.Models.Items;
using Data.Models.Player;
using Data.Models.References;
using Systems.InventorySystem;

namespace Systems.EntitySystem.Interfaces
{
    public interface IPlayer : IActor, IInventoryOwner
    {
        PlayerConfig Config { get; }
        void ShowMessage(string message);
        void DrinkPotion(ItemInstance potion);
        SpriteRef Icon { get; }
    }
}