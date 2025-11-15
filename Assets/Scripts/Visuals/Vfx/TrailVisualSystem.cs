using Core;
using Core.Events;
using UnityEngine;

namespace Visuals.Vfx
{
    public class TrailVisualSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<PlayerMeleeAttackEvent>(OnMeleeAttack);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<PlayerMeleeAttackEvent>(OnMeleeAttack);
        }

        private void OnMeleeAttack(PlayerMeleeAttackEvent e)
        {
            var userPos = e.User.Position;
            var offset = e.TargetPosition.Normalized * (e.AttackRange * 0.5f);
            var spawnPos = e.User.CharacterState.IsFacingRight ?
                userPos + offset :
                userPos - offset;
            
            var trail = e.Item.ItemData.WeaponData.Trail;
            GameEventBus.Publish(new TrailSpawnRequest
            {
                SpawnContext = trail.ToTrailSpawnContext(
                    spawnPos.ToVector3(), 
                    Quaternion.Euler(0, 0, Mathf.Atan2(e.Direction.y, e.Direction.x) * Mathf.Rad2Deg))
            });
        }
    }

}