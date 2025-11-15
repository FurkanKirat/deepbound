using System;
using Core.Context;
using Core.Context.Spawn;
using Data.Models;
using Data.Models.Entities;
using Systems.BuffSystem;
using Systems.EffectSystem;
using Systems.EntitySystem.Interfaces;
using Systems.MovementSystem;
using Systems.Physics;
using Systems.Physics.Colliders;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.Entity;
using Systems.StatSystem;
using UnityEngine;
using Utils;

namespace Systems.EntitySystem
{
    public abstract class BaseEntity : IPhysicalEntity, ISaveable<EntitySaveData>
    {
        public int Id { get; private set; }
        public abstract EntityType Type { get; }
        public abstract EntityData EntityData { get; }
        
        public StatCollection StatCollection { get; private set; }
        public CharacterState CharacterState { get; private set; } = new();
        public EntityState State { get; private set; } = EntityState.Despawned;

        public Vector2 Velocity { get; set; }
        public EntityMovement Movement { get; protected set; }
        private CooldownHandler CooldownHandler { get; set; }
        protected EffectHandler EffectHandler { get; private set; }

        public ICollider Collider => ColliderHandler.Collider;

        public WorldPosition Position
        {
            get => ColliderHandler.Position;
            set => ColliderHandler.Position = value;
        }
        public bool IsGrounded => CharacterState.IsGrounded;

        protected ColliderHandler ColliderHandler;
        
        protected void InitializeBase(
            BaseSpawnContext spawnContext,
            EntitySaveData saveData = null)
        {
            if (saveData != null)
            {
                // Load
                Id = saveData.Id;
                Velocity = saveData.Velocity;
                CharacterState = saveData.CharacterState;
                CooldownHandler = CooldownHandler.Load(saveData.Cooldowns);
                EffectHandler = EffectHandler.Load(this, saveData.Effects);
            }
            else
            {
                // Normal spawn
                Id = IdGenerator.NewId;
                Velocity = Vector2.zero;
                CharacterState = new CharacterState();
                CooldownHandler = CooldownHandler.Create();
                EffectHandler = EffectHandler.Create(this);
            }

            StatCollection = new StatCollection(this);
        }
        
        public virtual EntitySaveData ToSaveData()
        {
            return new EntitySaveData
            {
                Id = Id,
                Type = Type,
                Position = Position,
                Velocity = Velocity,
                CharacterState = CharacterState,
                Cooldowns = CooldownHandler.ToSaveData(),
                Effects = EffectHandler.ToSaveData(),
            };
        }

        public virtual void Tick(float timeInterval, TickContext ctx)
        {
            CooldownHandler.Tick(timeInterval, ctx);
            EffectHandler.Tick(timeInterval, ctx);
            ColliderHandler.UpdateGrounded();
            Movement.Tick(timeInterval);
        }
        public virtual void OnCollisionWithTile(DirectionFlags directionFlags) {}
        public virtual void OnCollisionWithEntity(IPhysicalEntity other) {}

        
        public virtual void Dispose()
        {
            if (State == EntityState.Disposed) return;
            
            StatCollection.Dispose();
            EffectHandler.Dispose();
            State = EntityState.Disposed;
        }
        public virtual void Spawn()
        {
            if (State == EntityState.Disposed)
                throw new ObjectDisposedException(GetType().Name);

            State = EntityState.Spawned;
        }

        public virtual void Despawn()
        {
            if (State == EntityState.Disposed)
                throw new ObjectDisposedException(GetType().Name);

            State = EntityState.Despawned;
        }
        
        public void AddCooldown(CooldownType type, float cooldownTime)
            => CooldownHandler.AddCooldown(type, cooldownTime);
        
        public float GetCooldown(CooldownType type)
            => CooldownHandler.GetCooldown(type);
        
        public bool HasCooldown(CooldownType type)
            => CooldownHandler.HasCooldown(type);

        public void AddDirectEffect(EffectData data)
            => EffectHandler.AddDirectEffect(data);
    }

}