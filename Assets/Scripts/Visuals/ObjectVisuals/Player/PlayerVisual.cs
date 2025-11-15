using Systems.EntitySystem.Interfaces;
using UnityEngine;

namespace Visuals.ObjectVisuals.Player
{
    public class PlayerVisual : BaseEntityVisual<IPlayer>
    {
        [SerializeField] private PlayerAnimator animator;
        
        public override void ClientTick(float deltaTime)
        {
            if (!IsActive)
                return;
            base.ClientTick(deltaTime);

            animator.Animate(deltaTime);
        }
    }
}