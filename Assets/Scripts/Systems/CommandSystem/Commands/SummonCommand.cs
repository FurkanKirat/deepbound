using System;
using Core;
using Core.Context;
using Core.Context.Spawn;
using Core.Events;
using Data.Models;
using Data.RegistrySystem;
using Systems.EntitySystem;
using Systems.EntitySystem.Interfaces;

namespace Systems.CommandSystem.Commands
{
    public class SummonCommand : ICommand
    {
        public string Name => "summon";
        public string Description => "Summons the entity at the given position";
        public string Usage => "/summon <type> <id> <x> <y>";
        
        private float _x;
        private float _y;
        private EntityType _type;
        private string _id;
        private IFactory _factory;

        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            if (args.Length < 4)
            {
                result = $"Usage: {Usage}";
                return false;
            }
            
            if (!float.TryParse(args[2], out _x) || _x < 0)
            {
                result = "<x> must be a positive number.";
                return false;
            }
            
            if (!float.TryParse(args[3], out _y) || _y < 0)
            {
                result = "<y> must be a positive number.";
                return false;
            }

            var baseId = args[1];
            _id = "entity:" + args[1];
            
            switch (args[0])
            {
                case "enemy":
                    _type = EntityType.Enemy;
                    _factory = Registries.EnemyFactory;
                    break;
                case "npc":
                    _type = EntityType.Npc;
                    _factory = Registries.NpcFactory;
                    break;
                default:
                    result = "Unknown entity type.";
                    return false;
                    
            }
            
            if (!_factory.HasFactory(_id))
            {
                result = $"Given entity with ID {_id} could not found in the {_factory.GetType()} factory.";
                return false;
            }

            result = $"Summoned  {_type} {baseId} at ({_x}, {_y}).";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx)
        {
            IPhysicalEntity entity;
            var spawnPos = new WorldPosition(_x, _y);
            switch (_type)
            {
                case EntityType.Enemy:
                    var enemyCtx = new EnemySpawnContext
                    {
                        World = ctx.World,
                        SpawnPosition = spawnPos,
                        SubTypeId = _id
                    };
                    entity = Registries.EnemyFactory.Create(_id, enemyCtx);
                    break;
                case EntityType.Npc:
                    var npcCtx = new NpcSpawnContext
                    {
                        World = ctx.World,
                        SpawnPosition = spawnPos,
                        SubTypeId = _id
                    };
                    entity = Registries.NpcFactory.Create(_id, npcCtx);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            GameEventBus.Publish(new EntitySpawnRequest(entity));
        }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}