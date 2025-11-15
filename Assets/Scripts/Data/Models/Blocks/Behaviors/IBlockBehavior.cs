using System;
using GameLoop;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Systems.WorldSystem;

namespace Data.Models.Blocks.Behaviors
{
    public interface IBlockBehavior : 
        ISaveable<BlockBehaviorSaveData>, 
        IInteractable,
        ITickable,
        ICollidable,
        IDisposable
    {
        void OnRemove(World world);
    }
}