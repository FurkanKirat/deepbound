using System.Collections.Generic;
using Data.Models.Items;
using Utils.Extensions;

namespace Data.Models.Crafting
{
    public class CraftingRecipe : 
        ICategorizeable<CraftingStation>
    {
        public List<ItemAmount> Requires { get; set; }
        public ItemAmount Output { get; set; }
        public CraftingStation Station { get; set; }
        public CraftingStation Category => Station;
        
        public override string ToString()
        {
            return $"CraftingRecipe({nameof(Requires)}: {Requires.ToDebugString()}, {nameof(Output)}: {Output})";
        }
    }
}