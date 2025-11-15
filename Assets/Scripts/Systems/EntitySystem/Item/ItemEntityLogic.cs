using Core;
using Core.Context;
using Core.Context.Spawn;
using Core.Events;
using Data.Database;
using Data.Models;
using Data.Models.Entities;
using Data.Models.Items;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using Systems.MovementSystem;
using Systems.MovementSystem.Behaviors;
using Systems.SaveSystem.SaveData.Entity;

namespace Systems.EntitySystem.Item
{
    public class ItemEntityLogic : BaseEntity, IItemEntity
    {
        public override EntityType Type => EntityType.Item;
        public ItemInstance ItemInstance { get; private set; }
        public float LifeTime { get; private set; }
        
        public ItemEntityConfig Config { get; private set; }
        public override EntityData EntityData => Config;

        private void Initialize(ItemEntitySpawnContext spawnContext, ItemEntitySaveData saveData = null)
        {
            // Base init
            InitializeBase(spawnContext, saveData);

            Config = Configs.ItemEntityConfig;
            // Entity-specific init
            LifeTime = Config.LifeTime ?? 0f;
            Movement = new EntityMovement(new GravityMovement(), this, spawnContext.World);
            ColliderHandler = new ColliderHandler(this, saveData?.Position ?? spawnContext.SpawnPosition, Config.Size, spawnContext.World);

            // Item instance
            ItemInstance = saveData != null 
                ? ItemInstance.Load(saveData.ItemSaveData) 
                : spawnContext.Item;
        }

        public ItemEntityLogic(ItemEntitySpawnContext spawnContext)
            => Initialize(spawnContext);

        public ItemEntityLogic(ItemEntitySpawnContext spawnContext, ItemEntitySaveData saveData)
            => Initialize(spawnContext, saveData);

        public override EntitySaveData ToSaveData()
        {
            var entitySave =  base.ToSaveData();
            return new ItemEntitySaveData(entitySave)
            {
                ItemSaveData = ItemInstance.ToSaveData()
            };
            
        }

        public void TryPickup(IPlayer player)
        {
            float distanceSqr = WorldPosition.SquaredDistance(Position, player.Position);
            const float maxRange = 1.0f;
            if (distanceSqr < maxRange * maxRange)
            {
                if (player.InventoryManager.GetPlayerInventory().AcceptItem(ItemInstance))
                {
                    GameEventBus.Publish(new EntityDestroyRequest(this));
                }
            }
        }

        public override void OnCollisionWithEntity(IPhysicalEntity other)
        {
            if (other is IPlayer player)
                TryPickup(player);
        }

        public override void Tick(float timeInterval, TickContext ctx)
        {
            base.Tick(timeInterval, ctx);
            LifeTime -= timeInterval;
            if (LifeTime <= 0f)
            {
                GameEventBus.Publish(new EntityDestroyRequest(this));
                return;
            }
        }
    }
}