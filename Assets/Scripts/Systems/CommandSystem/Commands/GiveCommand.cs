using Core.Context;
using Data.Database;
using Data.Models;
using Data.Models.Items;
using Systems.InventorySystem;

namespace Systems.CommandSystem.Commands
{
    public class GiveCommand : ICommand
    {
        public string Name => "give";
        public string Description => "Gives item to the player.";
        public string Usage => "/give <itemId> <amount>";
        
        private string _itemId;
        private int _amount;

        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            result = "";

            if (args.Length < 2)
            {
                result = $"Usage: {Usage}";
                return false;
            }

            _itemId = args[0];

            if (!int.TryParse(args[1], out _amount) || _amount <= 0)
            {
                result = "Amount must be a positive number.";
                return false;
            }
            
            if (!Databases.Items.Exists(_itemId))
            {
                result = $"Item not found: <b>{_itemId}</b>";
                return false;
            }

            result = $"Gave {_amount}x <b>{_itemId}</b> to player.";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx)
        {
            var item = ItemInstance.Create(_itemId, _amount);
            ctx.Player.InventoryManager
                .GetPlayerInventory().AcceptItem(item);
        }

        public void OnFailure(string result, ClientContext ctx) { }
    }

}