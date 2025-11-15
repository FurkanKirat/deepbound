using Data.Models.Crafting;
using Data.Models.Items;
using Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;

namespace Core.Events
{
    public readonly struct ItemCraftedEvent : IEvent
    {
        public readonly ItemInstance Item;
        public readonly IInventoryOwner Owner;

        public ItemCraftedEvent(ItemInstance item, IInventoryOwner owner)
        {
            Item = item;
            Owner = owner;
        }
    }

    public readonly struct CraftingRequest : IEvent
    {
        public readonly IPlayer Player;
        public readonly CraftingRecipe Recipe;

        public CraftingRequest(CraftingRecipe recipe, IPlayer player)
        {
            Player = player;
            Recipe = recipe;
        }
    }
    
    public readonly struct CraftingSlotClickedEvent : IEvent
    {
        public readonly IInventoryOwner User;
        public readonly CraftingRecipe Recipe;
        
        public CraftingSlotClickedEvent(CraftingRecipe recipe, IInventoryOwner owner)
        {
            Recipe = recipe;
            User = owner;
        }
    }
}