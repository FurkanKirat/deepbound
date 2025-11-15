using System;
using Core.Context.Spawn;
using Data.RegistrySystem;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.Item;
using Systems.EntitySystem.Player;
using Systems.EntitySystem.Projectile;
using Systems.SaveSystem.SaveData.Entity;
using Systems.WorldSystem;

namespace Systems.EntitySystem
{
    public static class EntityLoadFactory
    {
        public static IPhysicalEntity GetEntityFromSaveData(EntitySaveData saveData, World world)
        {
            switch (saveData.Type)
            {
                case EntityType.Player:
                    var playerCtx = new PlayerSpawnContext
                    {
                        World = world,
                    };
                    if (saveData is PlayerSaveData playerSave)
                        return new PlayerLogic(playerCtx, playerSave);
                    else
                        throw new InvalidOperationException($"Expected PlayerSaveData but got {saveData.GetType().Name}");

                
                case EntityType.Enemy:
                    var enemySaveData = (EnemySaveData)saveData;
                    var enemyCtx = new EnemySpawnContext
                    {
                        World = world,
                        SubTypeId = enemySaveData.EnemyId
                    };
                    
                    return Registries.EnemyFactory.Load(enemySaveData.EnemyId, enemyCtx, enemySaveData);
                
                case EntityType.Item:
                    var itemEntityCtx = new ItemEntitySpawnContext
                    {
                        World = world,
                    };
                    return new ItemEntityLogic(itemEntityCtx, (ItemEntitySaveData)saveData);
                
                case EntityType.Projectile:
                    var projectileCtx = new ProjectileSpawnContext
                    {
                        World = world,
                    };
                    return new ProjectileLogic(projectileCtx, (ProjectileSaveData)saveData);
            }

            return null;
        }
    }
}