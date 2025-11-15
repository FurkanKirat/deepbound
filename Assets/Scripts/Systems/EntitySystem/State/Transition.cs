using System;
using Core.Context;
using Interfaces;

namespace Systems.EntitySystem.State
{
    public class Transition<T>
    {
        public IState<T> To { get; }
        public Func<TickContext, bool> Condition { get; }

        public Transition(IState<T> to, Func<TickContext, bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

}