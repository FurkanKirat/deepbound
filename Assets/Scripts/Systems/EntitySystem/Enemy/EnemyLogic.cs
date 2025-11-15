using System;
using System.Collections.Generic;
using Core;
using Core.Context;
using Core.Context.Registry;
using Core.Context.Spawn;
using Core.Events;
using Data.Database;
using Data.Models;
using Data.Models.Entities;
using Data.Models.Items;
using Systems.CombatSystem.Armor;
using Systems.CombatSystem.Behaviors;
using Systems.CombatSystem.Damage;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Systems.MovementSystem;
using Systems.SaveSystem.SaveData.Entity;
using Systems.StatSystem;
using Systems.WorldSystem;

namespace Systems.EntitySystem.Enemy
{
    public abstract class EnemyLogic : BaseEntity, IEnemy
    {
        #region Fields

        private float _timer;
        private EntityHealth _health;
        private ArmorProfile _armorProfile;
        private World _world;

        #endregion

        #region Properties
        public override EntityType Type => EntityType.Enemy;
        public EnemyData EnemyData { get; private set; }
        public IAttackBehavior AttackBehavior { get; private set; }
        public StateMachine<IEnemy> StateMachine { get; private set; }
        public override EntityData EntityData => EnemyData;

        public float CurrentHealth => _health.Current;
        public float MaxHealth => _health.Max;
        public bool IsDead => _health.IsDead;

        #endregion

        #region Constructor & Initialization

        private void Initialize(EnemySpawnContext spawnContext, EnemySaveData saveData = null)
        {
            InitializeBase(spawnContext, saveData);
            _world = spawnContext.World;
            
            var enemyId = saveData?.EnemyId ?? spawnContext.SubTypeId;
            EnemyData = Databases.Enemies[enemyId];
            
            StatCollection.SetBaseStats(EnemyData.BaseStats);
            
            WorldPosition spawnPosition;

            if (saveData != null)
            {
                _health = new EntityHealth(this, saveData.Health);
                spawnPosition = saveData.Position;
            }
            else
            {
                _health = new EntityHealth(this);
                spawnPosition = spawnContext.SpawnPosition;
            }
            
            ColliderHandler = new ColliderHandler(this, spawnPosition, EnemyData.Size, spawnContext.World);
            _armorProfile = new ArmorProfile();
            
            var movementCtx = new MovementBehaviorContext
            {
                BehaviorOwner = this
            };
            var movementBehavior = EnemyData.MovementBehavior.Create(movementCtx);
            Movement = new EntityMovement(movementBehavior, this, spawnContext.World);

            AttackBehavior = EnemyData.AttackBehavior.Create(new AttackBehaviorContext());
            
            StateMachine = new StateMachine<IEnemy>();
            var stateCtx = new EnemyStateContext
            {
                Enemy = this,
                StateMachine = StateMachine,
                Random = spawnContext.World.Random,
            };
            var firstState = EnemyData.AiBehavior.Create(stateCtx);
            StateMachine.ChangeState(firstState);
            
        }
        protected EnemyLogic(EnemySpawnContext spawnContext)
            => Initialize(spawnContext);

        protected EnemyLogic(EnemySpawnContext spawnContext, EnemySaveData saveData)
            => Initialize(spawnContext, saveData);

        public override EntitySaveData ToSaveData()
        {
            var entitySave =  base.ToSaveData();
            return new EnemySaveData(entitySave)
            {
                EnemyId = EnemyData.Id,
                Health = _health.ToSaveData(),
            };
        }

        #endregion
        
        #region Update Loop

        public override void Tick(float timeInterval, TickContext ctx)
        {
            if (State != EntityState.Spawned)
                return;
            
            if (IsDead)
            {
                GameEventBus.Publish(new EntityDestroyRequest(this));
                return;
            }
            
            _timer += timeInterval;
            base.Tick(timeInterval, ctx);
            StateMachine.Tick(timeInterval, ctx);
            Movement.Tick(timeInterval);
        }

        #endregion

        #region Combat

        public void TakeDamage(DamageInfo damage) => _health.TakeDamage(damage);
        public void Heal(float amount) => _health.Heal(amount);
        public float GetResistance(DamageType type) => _armorProfile.GetArmor(type);

        #endregion
        
        #region Collision

        public override void OnCollisionWithEntity(IPhysicalEntity other)
        {
            if (State != EntityState.Spawned) 
                return;
            
            if (_timer < 1f)
                return;

            if (other is not ITargetEntity target)
                return;

            if (target.IsDead)
                return;

            var attackContext = new AttackContext
            {
                AttackingEntity = this,
                TargetEntity = target,
                TargetFilter = entity => !entity.IsDead && entity.Type == EntityType.Player,
                Random = _world.Random,
                
                DamageContext = new DamageContext(
                    StatCollection.GetStat(StatType.Damage),
                    StatCollection.GetStat(StatType.CritRate),
                    StatCollection.GetStat(StatType.CritDamage)
                    )
            };

            this.AttackIfNeeded(attackContext);
        }
        #endregion

        #region Loot

        public IEnumerable<ItemInstance> GetDroppedItems(Random random)
        {
            return EnemyData.LootTable.Load().Roll(random);
        }

        #endregion
    }
}
