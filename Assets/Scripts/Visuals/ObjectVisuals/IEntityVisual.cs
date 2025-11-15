using UnityEngine;

namespace Visuals.ObjectVisuals
{
    public interface IEntityVisual
    {
        GameObject Object { get; }
        SpriteRenderer Renderer { get; }
        void OnSpawn();
        void OnDespawn(EntityVisualizer entityVisualizer);
    }
}