using System.Collections.Generic;
using Core.Context.Registry;
using Core.Context.Spawn;
using Data.Models.Blocks.Behaviors;
using Data.Models.Items.Behaviors;
using Data.Registrars;
using Interfaces;
using Systems.CombatSystem.Behaviors;
using Systems.EffectSystem;
using Systems.EntitySystem.Interfaces;
using Systems.MovementSystem.Behaviors;
using Systems.SaveSystem.SaveData;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Systems.SaveSystem.SaveData.Entity;

namespace Data.RegistrySystem
{
    public static class Registries
    {
        public static readonly Registry<IItemBehavior> ItemBehaviors = new();
        public static readonly DualFactoryRegistry<BlockBehaviorContext, BlockBehaviorSaveData, IBlockBehavior> BlockBehaviorFactory = new();
        public static readonly FactoryRegistry<EnemyStateContext, IState<IEnemy>> EnemyBehaviorFactory = new();
        public static readonly FactoryRegistry<NpcStateContext, IState<INpc>> NpcStateFactory = new();
        public static readonly DualFactoryRegistry<EnemySpawnContext, EnemySaveData, IEnemy> EnemyFactory = new();
        public static readonly DualFactoryRegistry<NpcSpawnContext, NpcSaveData, INpc> NpcFactory = new();
        public static readonly FactoryRegistry<MovementBehaviorContext, IMovementBehavior> MovementBehaviorFactory = new();
        public static readonly FactoryRegistry<AttackBehaviorContext, IAttackBehavior> AttackBehaviorFactory = new();
        public static readonly DualFactoryRegistry<EffectContext, EffectSaveData, IEffectBehavior> EffectBehaviorFactory = new();
        
        private static readonly List<IRegistrar> Registrars = new();
        public static bool HasRegistered { get; private set; } = false;
        public static void RegisterAll()
        {
            if (HasRegistered)
                return;

            RegisterInitials();

            foreach (var registrar in Registrars)
            {
                registrar.RegisterAll();
            }

            Registrars.Clear();
            HasRegistered = true;
        }

        public static void Register(IRegistrar registrar)
        {
            if (HasRegistered)
            {
                registrar.RegisterAll();
            }
            else
            {
                Registrars.Add(registrar);
            }
        }

        private static void RegisterInitials()
        {
            Register(new ItemBehaviorRegistrar());
            Register(new BlockBehaviorRegistrar());
            Register(new EnemyRegistrar());
            Register(new EnemyBehaviorRegistrar());
            Register(new MovementBehaviorRegistrar());
            Register(new AttackBehaviorRegistrar());
            Register(new EffectBehaviorRegistrar());
            Register(new CommandRegistrar());
        }

        
    }

}