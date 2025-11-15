using System;
using Core;
using Core.Events;

namespace GameLoop
{
    public class GameStateManager : IDisposable
    {
        private GameFlags Flags { get; set; } = GameFlags.None;

        public GameStateManager()
        {
            GameEventBus.Subscribe<UIPanelOpenedEvent>(OnUIPanelOpened);
            GameEventBus.Subscribe<UIPanelClosedEvent>(OnUIPanelClosed);
        }
        
        public void Dispose()
        {
            GameEventBus.Unsubscribe<UIPanelOpenedEvent>(OnUIPanelOpened);
            GameEventBus.Unsubscribe<UIPanelClosedEvent>(OnUIPanelClosed);
        }
        
        public void SetFlag(GameFlags flag, bool enabled)
        {
            if (enabled)
                Flags |= flag;
            else
                Flags &= ~flag;
        }

        public void ReverseFlag(GameFlags flag)
        {
            if (Flags.HasFlag(flag))
                Flags ^= flag;
            else
                Flags ^= ~flag;
        }

        public bool HasFlag(GameFlags flag) => Flags.HasFlag(flag);
        
        
        private void OnUIPanelClosed(UIPanelClosedEvent e)
        {
            if (e.PausesGame)
                SetFlag(GameFlags.Paused, false);
        }

        private void OnUIPanelOpened(UIPanelOpenedEvent e)
        {
            if (e.PausesGame)
                SetFlag(GameFlags.Paused, true);
        }
    }
}