using Core.Context;
using Core.Context.Spawn;
using Data.Database;
using Data.Models.Entities;
using Data.Models.Items;
using Data.Models.Player;
using Data.Models.References;
using Interfaces;
using Systems.BuffSystem;
using Systems.CombatSystem.Armor;
using Systems.CombatSystem.Damage;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Systems.InventorySystem;
using Systems.MovementSystem;
using Systems.MovementSystem.Behaviors;
using Systems.SaveSystem.SaveData.Entity;
using Utils;

namespace Systems.EntitySystem.Player
{
    public class PlayerLogic : BaseEntity, IPlayer, IInitializable
    {
        // --- Subsystems ---
        private StateMachine<IPlayer> _stateMachine;
        private EntityHealth _health;
        private PotionHandler _potionHandler;

        private ArmorProfile _armorProfile;
        
        private PlayerItemUseHandler _playerItemUseHandler;
        private PlayerInteractionHandler _playerInteractionHandler;
        private HeldItemHandler _heldItemHandler;
        private MapDiscoverer _mapDiscoverer;
        private SlotInteractionHandler _slotInteractionHandler;
        
        public InventoryManager InventoryManager { get; private set; }
        public PlayerConfig Config { get; private set; }
        public SpriteRef Icon { get; private set; }
        public override EntityData EntityData => Config;

        private void Initialize(PlayerSpawnContext spawnContext, PlayerSaveData saveData = null)
        {
            InitializeBase(spawnContext, saveData);

            Config = Configs.PlayerConfig;
            
            StatCollection.SetBaseStats(Config.Stats);

            _potionHandler = saveData != null ? 
                new PotionHandler(this, saveData.Potions) : 
                new PotionHandler(this);
                
            StatCollection.AddProvider(_potionHandler);
            
            ColliderHandler = new ColliderHandler(this, saveData?.Position ?? spawnContext.SpawnPosition, Config.Size, spawnContext.World);
            Movement = new EntityMovement(new GravityMovement(), this, spawnContext.World);

            _armorProfile = new ArmorProfile();
            
            _heldItemHandler = new HeldItemHandler(this);
            _slotInteractionHandler = new SlotInteractionHandler(this, _heldItemHandler);
            _stateMachine = new StateMachine<IPlayer>();

            var playerBehavior = new PlayerBehavior();
            playerBehavior.Configure(_stateMachine, this);
            _stateMachine.ChangeState(playerBehavior.InitialState);
            _playerInteractionHandler = new PlayerInteractionHandler(spawnContext.World, this);
            _playerItemUseHandler = new PlayerItemUseHandler(spawnContext.World, this);
            
            _mapDiscoverer = new MapDiscoverer(spawnContext.World, this);
            if (saveData != null)
            {
                _health = new EntityHealth(this, saveData.Health);
                InventoryManager = InventoryManager.Load(saveData.InventoryManager, this);
                Icon = saveData.Icon;
            }
            else
            {
                _health = new EntityHealth(this);

                InventoryManager = InventoryManager.Create(this);
                InventoryManager.RegisterInventory(
                    SlotCollectionType.Inventory,
                    new PlayerInventory(Configs.GameConfig.Inventory.Player.MainInventorySlotCount, 
                        this)
                );
            
                InventoryManager.RegisterInventory(
                    SlotCollectionType.Equipment,
                    new Equipment(this)
                );
            
                InventoryManager.RegisterInventory(
                    SlotCollectionType.Accessory,
                    new AccessoryInventory(Configs.GameConfig.Inventory.Player.AccessoryCount, this));

                Icon = spawnContext.Icon;
            }

            InventoryManager.RegisterInventoryStats(StatCollection);
            InventoryManager.RegisterInventoryEffects(EffectHandler);
        }
        
        public PlayerLogic(PlayerSpawnContext spawnContext, PlayerSaveData saveData)
            => Initialize(spawnContext, saveData);
        public PlayerLogic(PlayerSpawnContext spawnContext)
            => Initialize(spawnContext);
        
        public InventoryOwnerType InventoryOwnerType => InventoryOwnerType.Player;
        public override EntityType Type => EntityType.Player;

        public float CurrentHealth => _health.Current;
        public float MaxHealth => _health.Max;
        public bool IsDead => _health.IsDead;

        public void TakeDamage(DamageInfo damage)
        { 
            _health.TakeDamage(damage);
            Movement.ApplyKnockback(damage.Knockback);
        }
        public void Heal(float amount) => _health.Heal(amount);
        

        public float GetResistance(DamageType type) => _armorProfile.GetArmor(type);
        
        public override void Tick(float timeInterval, TickContext ctx)
        {
            base.Tick(timeInterval, ctx);

            _stateMachine.Tick(timeInterval, ctx);
            _potionHandler.Tick(timeInterval, ctx);
            _mapDiscoverer.Tick(timeInterval, ctx);
        }
        
        public void ShowMessage(string message)
        {
            GameLogger.Log($"[Player Message]: {message}");
        }
        
        public override EntitySaveData ToSaveData()
        {
            var entitySave =  base.ToSaveData();
            return new PlayerSaveData(entitySave)
            {
                InventoryManager = InventoryManager.ToSaveData(),
                Health = _health.ToSaveData(),
                Potions = _potionHandler.ToSaveData(),
                Icon = Icon
            };
        }
        
        public void Initialize()
        {
            InventoryManager.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
            _playerInteractionHandler.Dispose();
            _playerItemUseHandler.Dispose();
            _slotInteractionHandler.Dispose();
            InventoryManager.Dispose();
        }
        public void DrinkPotion(ItemInstance potion) => _potionHandler.RegisterPotion(potion);
        
    }
}