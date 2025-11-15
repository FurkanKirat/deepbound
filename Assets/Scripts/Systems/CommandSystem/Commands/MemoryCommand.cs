using System;
using Core.Context;
using Data.Models;

namespace Systems.CommandSystem.Commands
{
    public class MemoryCommand : ICommand
    {
        public string Name => "mem";
        public string Description => "Displays current memory usage of the game.";
        public string Usage => "/mem";
        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            long managed = GC.GetTotalMemory(false);
            long mono = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
            long total = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
            long process = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
            
            result = 
                $"<b>Memory Usage (MB)</b>\n" +
                $"• Managed: {managed / 1048576f:F2}\n" +
                $"• Mono Heap: {mono / 1048576f:F2}\n" +
                $"• Unity Total: {total / 1048576f:F2}\n" +
                $"• Process RAM: {process / 1048576f:F2}";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx) { }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}