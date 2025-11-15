using System;
using System.Collections.Generic;
using Core;
using Data.Models;
using Data.Models.Blocks;
using Systems.WorldSystem;
using Visuals.Interfaces;

namespace Visuals.Rendering
{
    public class BlockAnimator : IClientTickable, IDisposable
    {
        private readonly BlockRenderer _renderer;
        private readonly World _world;

        // tilePosition → animation state
        private readonly HashSet<AnimationState> _animationStates = new(new AnimationStateComparer());

        public BlockAnimator(BlockRenderer renderer, World world)
        {
            _renderer = renderer;
            _world = world;
            ClientGameLoop.Instance.Register(this);
        }

        public void Dispose()
        {
            ClientGameLoop.Instance?.Unregister(this);
        }
        
        public void ClientTick(float deltaTime)
        {
            FindBlocks();
            Animate(deltaTime);
        }

        private void Animate(float deltaTime)
        {
            foreach (var state in _animationStates)
            {
                state.ElapsedTime += deltaTime;
                
                if (state.ElapsedTime < state.Animation.Speed)
                    continue;

                state.ElapsedTime = 0f;
                state.CurrentFrame++;

                if (state.CurrentFrame >= state.Animation.Frames.Length)
                {
                    state.CurrentFrame = state.Animation.Loop
                        ? 0
                        : state.Animation.Frames.Length - 1;
                }

                _renderer.SetTileSprite(state.Position, state.CurrentFrame);
            }
        }

        private void FindBlocks()
        {
            var player = _world.Player;
            List<AnimationState> toRemove = null;

            foreach (var state in _animationStates)
            {
                if ((player.Position - state.Position.ToWorldPosition()).SqrMagnitude > 225)
                {
                    toRemove ??= new List<AnimationState>();
                    toRemove.Add(state);
                }
            }

            var bounds = player.Collider.Bounds.ToBounds2DIntInclusive();
            for (int x = bounds.MinX - 8; x < bounds.MaxX + 8; x++)
            for (int y = bounds.MinY - 8; y < bounds.MaxY + 8; y++)
            {
                var animation = _world.BlockManager.GetBlockAt(x, y).GetBlockData().IdleAnimation;
                if (animation.Frames.Length <= 1)
                    continue;
                
                _animationStates.Add(new AnimationState
                {
                    Animation = animation,
                    Position = new TilePosition(x,y)
                });
            }

            if (toRemove != null)
            {
                foreach (var pos in toRemove)
                    _animationStates.Remove(pos);
            }
        }


        private class AnimationState
        {
            public BlockAnimation Animation;
            public TilePosition Position;
            public int CurrentFrame;
            public float ElapsedTime;
        }
        
        private class AnimationStateComparer : IEqualityComparer<AnimationState>
        {
            public bool Equals(AnimationState x, AnimationState y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;

                return x.Position == y.Position;
            }

            public int GetHashCode(AnimationState obj)
            {
                return HashCode.Combine(obj.Position);
            }
        }

    }
}
