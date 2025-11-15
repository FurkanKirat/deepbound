using System;
using Core;
using Core.Context.Spawn;
using Core.Events;
using Data.Models;
using Data.Models.Crafting;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.Item;
using Systems.InventorySystem;
using Systems.WorldSystem;
using Utils;
using Random = UnityEngine.Random;

namespace Systems.CraftingSystem
{
    public class CraftManager : IDisposable
    {
        private readonly World _world;

        public CraftManager(World world)
        {
            _world = world;
            GameEventBus.Subscribe<CraftingRequest>(OnCraftingRequest);
        }
        
        public void Dispose()
        {
            GameEventBus.Unsubscribe<CraftingRequest>(OnCraftingRequest);
        }
        
        public bool CraftItem(IPlayer player, CraftingRecipe recipe)
        {
            var inventory = player.InventoryManager.GetPlayerInventory();
            if (!inventory.CanCraft(recipe))
            {
                GameLogger.Warn($"Can't craft recipe {recipe}.", nameof(CraftManager));
                return false;
            }
                

            foreach (var itemAmount in recipe.Requires)
            {
                if (!inventory.RemoveItem(itemAmount.Id, itemAmount.Count))
                {
                    GameLogger.Warn("Somehow failed to remove item after CanCraft check.", nameof(CraftManager));
                    return false; 
                }
            }
            
            var output = recipe.Output;
            var item = output.ToItemInstance();
            bool inventoryAccepted = inventory.AcceptItem(item);

            if (!inventoryAccepted)
            {
                var position = player.Position + WorldPosition.FromVector2(Random.insideUnitCircle * 0.3f);
                var itemEntitySpawnCtx = new ItemEntitySpawnContext
                {
                    Item = item,
                    SpawnPosition = position,
                    World = _world
                };
                var itemDrop = new ItemEntityLogic(itemEntitySpawnCtx);
                GameEventBus.Publish(new EntitySpawnRequest(itemDrop));
            }
            GameEventBus.Publish(new ItemCraftedEvent(item, player));
            
            return true;
        }
        
        private void OnCraftingRequest(CraftingRequest e)
        {
            CraftItem(e.Player, e.Recipe);
        }

        
    }
}