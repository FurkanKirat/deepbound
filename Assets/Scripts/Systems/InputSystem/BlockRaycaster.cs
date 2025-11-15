using Data.Models;
using UnityEngine;

namespace Systems.InputSystem
{
    public class BlockRaycaster
    {
        private readonly Camera _camera;

        public BlockRaycaster(Camera camera)
        {
            _camera = camera;
        }

        public WorldPosition GetWorldPosition(Vector2 screenPos)
        {
            var worldPos = _camera.ScreenToWorldPoint(screenPos);
            return new WorldPosition(worldPos.x, worldPos.y);
        }
        
        public WorldPosition GetWorldPosition(Vector3 screenPos)
        {
            var worldPos = _camera.ScreenToWorldPoint(screenPos);
            return WorldPosition.FromVector3(worldPos);
        }
        
        public Vector3 GetWorldPositionAsVec3(Vector3 screenPos)
        {
            var worldPos = _camera.ScreenToWorldPoint(screenPos);
            return new Vector3(worldPos.x, worldPos.y);
        }
    }

}