using Data.Models;
using Systems.EntitySystem.Interfaces;
using UnityEngine;

namespace Core.Context.Spawn
{
    
    public class ProjectileSpawnContext : BaseSpawnContext
    {
        public IPhysicalEntity Owner { get; set; }
        public DamageContext DamageContext { get; set; }
        public Vector2 Direction { get; set; }
        public WorldPosition? TargetPosition { get; set; }
    }
}