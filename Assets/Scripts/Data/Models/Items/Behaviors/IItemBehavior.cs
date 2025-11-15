using Core.Context;

namespace Data.Models.Items.Behaviors
{
    public interface IItemBehavior
    {
        /// <summary>
        /// Called when item is used. Returns true if use was successful.
        /// </summary>
        bool TryUse(ItemUseContext context, out string failReason);
        
        /// <summary>
        /// Called after TryUse returns true. Can be used to reduce inventory or trigger other side effects.
        /// </summary>
        void OnSuccess(ItemUseContext context);
        
        /// <summary>
        /// Called after TryUse returns false. Generally triggers events
        /// </summary>
        void OnFail(ItemUseContext context, string failReason);
    }

}