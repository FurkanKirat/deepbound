using Core.Context;
using Core.Context.Registry;
using Core.Context.Spawn;
using Data.Database;
using Data.Models.Entities;
using Systems.CombatSystem.Armor;
using Systems.CombatSystem.Damage;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Systems.MovementSystem;
using Systems.SaveSystem.SaveData.Entity;

namespace Systems.EntitySystem.Npc
{
    public class NpcLogic : BaseEntity, INpc
    {
        public NpcData NpcData { get; private set; }
        public override EntityType Type => EntityType.Npc;
        private StateMachine<INpc> StateMachine { get; set; }
        
        private EntityHealth _health;
        private ArmorProfile _armorProfile;
        public override EntityData EntityData => NpcData;

        public float CurrentHealth => _health.Current;
        public float MaxHealth => _health.Max;
        public bool IsDead => _health.IsDead;

        private void Initialize(NpcSpawnContext spawnContext, NpcSaveData saveData = null)
        {
            InitializeBase(spawnContext, saveData);
            NpcData = Databases.Npcs[saveData?.NpcId ?? spawnContext.SubTypeId];
            StatCollection.SetBaseStats(NpcData.BaseStats);
            
            _health = saveData != null ? 
                new EntityHealth(this, saveData.Health) : 
                new EntityHealth(this);
            
            var spawnPos = saveData?.Position ?? spawnContext.SpawnPosition;
            ColliderHandler = new ColliderHandler(this, spawnPos, NpcData.Size, spawnContext.World);
            _armorProfile = new ArmorProfile();

            var movementCtx = new MovementBehaviorContext
            {
                BehaviorOwner = this
            };
            
            var movementBehavior = NpcData.MovementBehavior.Create(movementCtx);
            Movement = new EntityMovement(movementBehavior, this, spawnContext.World);
            
            StateMachine = new StateMachine<INpc>();
            var stateContext = new NpcStateContext
            {
                Npc = this,
                StateMachine = StateMachine,
            };
            var firstState = NpcData.AiBehavior.Create(stateContext);
            StateMachine.ChangeState(firstState);
            
        }
        public NpcLogic(NpcSpawnContext spawnContext)
            => Initialize(spawnContext);
        
        public NpcLogic(NpcSpawnContext spawnContext, NpcSaveData saveData)
            => Initialize(spawnContext, saveData);

        public override EntitySaveData ToSaveData()
        {
            var baseSave = base.ToSaveData();
            return new NpcSaveData(baseSave)
            {
                NpcId = NpcData.Id,
                Health = _health.ToSaveData(),
            };
        }

        public override void Tick(float timeInterval, TickContext ctx)
        {
            base.Tick(timeInterval, ctx);
            StateMachine.Tick(timeInterval, ctx);
        }
        
        public void TakeDamage(DamageInfo damage)
        { 
            _health.TakeDamage(damage);
            Movement.ApplyKnockback(damage.Knockback);
        }
        public void Heal(float amount) => _health.Heal(amount);

        public float GetResistance(DamageType type) => _armorProfile.GetArmor(type);
    }
}