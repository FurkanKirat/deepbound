using System;
using System.Collections.Generic;
using Data.Models.Items;

namespace Systems.EntitySystem.Interfaces
{
    public interface IDropsItems
    {
        IEnumerable<ItemInstance> GetDroppedItems(Random random);
    }

}