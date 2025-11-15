using Data.Models;
using Systems.InputSystem;
using UnityEngine;

namespace Core.Events
{
    public readonly struct UIPrimaryUseStarted : IEvent
    {
        public readonly Vector2 ScreenPosition;

        public UIPrimaryUseStarted(Vector2 screenPosition)
        {
            ScreenPosition = screenPosition;
        }
    }
    public readonly struct PrimaryUseStarted : IEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly WorldPosition WorldPosition;

        public PrimaryUseStarted(Vector2 screenPosition, WorldPosition worldPosition)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
        }
    }

    public readonly struct PrimaryUseHeld : IEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly WorldPosition WorldPosition;

        public PrimaryUseHeld(Vector2 screenPosition, WorldPosition worldPosition)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
        }
    }
    
    public readonly struct UIPrimaryUseHeld : IEvent
    {
        public readonly Vector2 ScreenPosition;

        public UIPrimaryUseHeld(Vector2 screenPosition)
        {
            ScreenPosition = screenPosition;
        }
    }

    public readonly struct PrimaryUseEnded : IEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly WorldPosition WorldPosition;

        public PrimaryUseEnded(Vector2 screenPosition, WorldPosition worldPosition)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
        }
    }
    
    public readonly struct UIPrimaryUseEnded : IEvent
    {
        public readonly Vector2 ScreenPosition;

        public UIPrimaryUseEnded(Vector2 screenPosition)
        {
            ScreenPosition = screenPosition;
        }
    }

    public readonly struct PrimaryUseClicked : IEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly WorldPosition WorldPosition;

        public PrimaryUseClicked(Vector2 screenPosition, WorldPosition worldPosition)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
        }
    }
    
    public readonly struct UIPrimaryUseClicked : IEvent
    {
        public readonly Vector2 ScreenPosition;

        public UIPrimaryUseClicked(Vector2 screenPosition)
        {
            ScreenPosition = screenPosition;
        }
    }
    
    public readonly struct SecondaryUseRequested : IEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly WorldPosition WorldPosition;

        public SecondaryUseRequested(Vector2 screenPosition, WorldPosition worldPosition)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
        }
    }

    public readonly struct MoveInputEvent : IEvent
    {
        public readonly Vector2 Direction;
        
        public MoveInputEvent(Vector2 direction)
        {
            Direction = direction;
        }
    }
    public readonly struct JumpInputEvent : IEvent
    {
        public readonly JumpInputPhase Phase;

        public JumpInputEvent(JumpInputPhase phase)
        {
            Phase = phase;
        }
    }
    
    public readonly struct EscapePressedEvent : IEvent{ }

    public readonly struct ChatSubmitOrToggleEvent : IEvent { }

    public readonly struct MouseScrolledEvent : IEvent
    {
        public float ScrollValue { get; }

        public MouseScrolledEvent(float scrollValue)
        {
            ScrollValue = scrollValue;
        }
    }

    

}