using Data.Models.Blocks.Behaviors;
using Data.RegistrySystem;
using Generated.Ids;
using Systems.SaveSystem.SaveData.BlockBehavior;

namespace Data.Registrars
{
    public class BlockBehaviorRegistrar : IRegistrar
    {
        public void RegisterAll()
        {
            Registries.BlockBehaviorFactory.RegisterMany(
                (BlockBehaviorIds.Chest, createCtx => new ChestBehavior(createCtx), (ctx, saveData) => new ChestBehavior(ctx, (ChestBehaviorSaveData)saveData)),
                (BlockBehaviorIds.Crafting, createCtx => new CraftingBehavior(createCtx), (loadCtx,_) => new CraftingBehavior(loadCtx)),
                (BlockBehaviorIds.Crop, createCtx => new CropBehavior(createCtx), (loadCtx, saveData) => new CropBehavior(loadCtx, (CropBehaviorSaveData)saveData)),
                (BlockBehaviorIds.Portal, createCtx => new PortalBehavior(createCtx), (loadCtx,saveData) => new PortalBehavior(loadCtx, (PortalBehaviorSaveData)saveData)),
                (BlockBehaviorIds.Bed, createCtx => new BedBehavior(createCtx), (loadCtx,_) => new BedBehavior(loadCtx))

                    );
        }
    }
}