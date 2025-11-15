using Systems.EntitySystem.Interfaces;
using Systems.WorldSystem;

namespace Data.Models
{
    public interface IInteractable
    {
        void Interact(IPlayer player, World world);
    }

}