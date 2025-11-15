using System;
using System.Collections.Generic;
using Core;
using Core.Context.Registry;
using Core.Context.Spawn;
using Core.Events;
using Data.Database;
using Data.Models.Items;
using Generated.Ids;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.Item;
using Systems.InventorySystem;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Systems.WorldSystem;
using Utils;

namespace Data.Models.Blocks.Behaviors
{
    public class ChestBehavior : BaseBlockBehavior, IInventoryOwner
    {
        public override string BehaviorId => BlockBehaviorIds.Chest;
        
        public ChestState State { get; private set; }

        public InventoryManager InventoryManager { get; private set; }
        public InventoryOwnerType InventoryOwnerType => InventoryOwnerType.Chest;
        
        public ChestBehavior(BlockBehaviorContext ctx) : base(ctx.Position)
        {
            State = ctx.IsGenerated ?
                ChestState.Generated : 
                ChestState.PlayerPlaced;
        }

        public ChestBehavior(BlockBehaviorContext ctx, ChestBehaviorSaveData saveData): base(ctx.Position)
        {
            State = saveData.ChestState;
            if (State == ChestState.Opened)
            {
                InventoryManager = InventoryManager.Load(saveData.InventoryManagerSaveData, this);
            }
        }
        
        public override void Interact(IPlayer player, World world)
        {
            switch (State)
            {
                case ChestState.Generated:
                {
                    Generate(world);
                    break;
                }
                case ChestState.PlayerPlaced:
                    InventoryManager = InventoryManager.Create(this);
                    InventoryManager.RegisterInventory(
                        SlotCollectionType.Inventory,
                        new Inventory(Configs.GameConfig.Inventory.Chest.SlotCount, this));
                    State = ChestState.Opened;
                    break;
                case ChestState.Opened:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var playerConfig = new InventoryOpenConfig(player, new[] { SlotCollectionType.Inventory });
            var chestConfig = new InventoryOpenConfig(this, new[] { SlotCollectionType.Inventory });
            GameEventBus.Publish(new InventoryOpenRequestEvent(new []{playerConfig, chestConfig}));
        }

        private void Generate(World world)
        {
            var inventory = new Inventory(Configs.GameConfig.Inventory.Chest.SlotCount, this);
                
            var lootTable = world.CurrentDimension.LayerManager.GetLayerForPosition(Position).LootTable.Load();
            var loot = lootTable.Roll(world.Random);
            foreach (var item in loot)
            {
                inventory.AcceptItem(item);
                GameLogger.Log($"Accepting Item {item}", nameof(ChestBehavior));
            }
            inventory.Shuffle(world.Random);
            InventoryManager = InventoryManager.Create(this);
            InventoryManager.RegisterInventory(SlotCollectionType.Inventory, inventory);
            State = ChestState.Opened;
        }
        public override void Dispose()
        {
            InventoryManager?.Dispose();
        }

        public override void OnRemove(World world)
        {
            base.OnRemove(world);
            switch (State)
            {
                case ChestState.Generated:
                
                    var lootTable = world.CurrentDimension.LayerManager.GetLayerForPosition(Position).LootTable.Load();
                    var loot = lootTable.Roll(world.Random);
                    SpawnItems(loot, world);
                    break;
                
                case ChestState.PlayerPlaced:
                    break;
                
                case ChestState.Opened:
                    SpawnItems(InventoryManager.GetInventory(SlotCollectionType.Inventory).AllItems, world);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SpawnItems(IEnumerable<ItemInstance> items, World world)
        {
            foreach (var item in items)
            {
                if (item.IsEmpty)
                    continue;
                
                var offset = RandomUtils.RandomInsideCircle(world.Random,0.3f, 0.7f);
                var itemPosition = Position.ToWorldPosition() + offset;
                var spawnCtx = new ItemEntitySpawnContext
                {
                    Item = item,
                    World = world,
                    SpawnPosition = itemPosition
                };
                var entity = new ItemEntityLogic(spawnCtx);
                GameEventBus.Publish(new EntitySpawnRequest(entity));
            }
        }

        public override BlockBehaviorSaveData ToSaveData()
        {
            return new ChestBehaviorSaveData
            {
                BehaviorId = BehaviorId,
                ChestState = State,
                InventoryManagerSaveData = InventoryManager?.ToSaveData()
            };
        }

        public enum ChestState : byte
        {
            Generated,
            PlayerPlaced,
            Opened
        }

        
    }
}