using Core.Context;

namespace Systems.CommandSystem
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        string Usage { get; }
        bool TryExecute(string[] args, ClientContext ctx, out string result);
        void OnSuccess(string result, ClientContext ctx);
        void OnFailure(string result, ClientContext ctx);
    }
}