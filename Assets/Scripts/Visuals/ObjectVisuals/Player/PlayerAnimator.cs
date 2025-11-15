using UnityEngine;

namespace Visuals.ObjectVisuals.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerVisual player;
        [SerializeField] private HandAnimator hand;
        [SerializeField] private HandItemVisual handItem;
        public SpriteRenderer spriteRenderer;
        public Sprite[] walkFrames;
        public float frameRate = 0.25f; //4 frame per second
        private float _timer;
        private int _frame;
        private bool _facingRight;

        public void Animate(float deltaTime)
        {
            var velocity = player.Data.Velocity;

            if (_facingRight != player.Data.CharacterState.IsFacingRight)
            {
                if (player.Data.CharacterState.IsFacingRight)
                {
                    spriteRenderer.flipX = false;
                    hand.SetFacing(true);
                    handItem.SetFacing(true);
                }
                else
                {
                    spriteRenderer.flipX = true;
                    hand.SetFacing(false);
                    handItem.SetFacing(false);
                }
                _facingRight = player.Data.CharacterState.IsFacingRight;
            }
            
            if (Mathf.Abs(velocity.x) > 0.1f)
            {
                _timer += deltaTime;
                if (_timer >= frameRate)
                {
                    _timer = 0;
                    _frame = (_frame + 1) % walkFrames.Length;
                    spriteRenderer.sprite = walkFrames[_frame];
                }
                    
            }
            else
            {
                spriteRenderer.sprite = walkFrames[0]; // idle
            }
        }
    }

}