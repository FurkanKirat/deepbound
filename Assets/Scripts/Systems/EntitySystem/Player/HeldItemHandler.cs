using Core;
using Core.Events;
using Data.Models.Items;
using Systems.EntitySystem.Interfaces;

namespace Systems.EntitySystem.Player
{
    public class HeldItemHandler
    {
        public ItemInstance HeldItem { get; private set; } = ItemInstance.Empty;
        
        private readonly IPlayer _player;

        public HeldItemHandler(IPlayer player)
        {
            _player = player;
        }
        public bool IsEmpty => HeldItem == null || HeldItem.IsEmpty;

        public void Set(ItemInstance item)
        {
            HeldItem = item;
            PublishHeldItemChanged();
        }

        public void Clear()
        {
            HeldItem = ItemInstance.Empty;
            PublishHeldItemChanged();
        }

        public void Decrease(int amount)
        {
            HeldItem.Count -= amount;
            if (HeldItem.Count <= 0)
                Clear();
            else
                PublishHeldItemChanged();
        }
        
        public void UpdateRemaining(int remaining)
        {
            if (remaining > 0)
            {
                HeldItem.Count = remaining;
                PublishHeldItemChanged();
            }
            else Clear();
        }

        private void PublishHeldItemChanged()
        {
            GameEventBus.Publish(new HeldItemChangedEvent(_player, HeldItem));
        }
    }

}