using System;
using System.Collections.Generic;
using Core.Context;
using GameLoop;
using Interfaces;

namespace Systems.EntitySystem.State
{
    public class StateMachine<T> : ITickable
    {
        private readonly Dictionary<IState<T>, List<Transition<T>>> _transitions = new();
        private readonly List<Transition<T>> _anyTransitions = new();
        private IState<T> _currentState;

        public void ChangeState(IState<T> newState)
        {
            if (_currentState == newState) return;
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
        
        public void AddTransition(IState<T> from, IState<T> to, Func<TickContext, bool> condition)
        {
            if (!_transitions.TryGetValue(from, out var transitions))
            {
                transitions = new List<Transition<T>>();
                _transitions[from] = transitions;
            }
            transitions.Add(new Transition<T>(to, condition));
        }

        public void AddAnyTransition(IState<T> to, Func<TickContext, bool> condition)
        {
            _anyTransitions.Add(new Transition<T>(to, condition));
        }

        public void Tick(float timeInterval, TickContext ctx)
        {
            // 1. Global (any) transitions
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition(ctx))
                {
                    ChangeState(transition.To);
                    return;
                }
            }

            // 2. State-specific transitions
            if (_currentState != null && _transitions.TryGetValue(_currentState, out var transitions))
            {
                foreach (var transition in transitions)
                {
                    if (transition.Condition(ctx))
                    {
                        ChangeState(transition.To);
                        return;
                    }
                }
            }

            _currentState?.Tick(timeInterval, ctx);
        }
    }

}