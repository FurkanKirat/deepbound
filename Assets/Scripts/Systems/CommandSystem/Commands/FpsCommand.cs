using Core.Context;
using Data.Models;
using Utils;

namespace Systems.CommandSystem.Commands
{
    public class FpsCommand : ICommand
    {
        public string Name => "fps";
        public string Description => "Displays the current frame rate.";
        public string Usage => "/fps";
        
        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            result = $"Current FPS: {FPSMonitor.CurrentFPS:F1}";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx) { }

        public void OnFailure(string result, ClientContext ctx) { }
    }
    
}