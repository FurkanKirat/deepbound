using System;
using System.Collections.Generic;
using System.Text;
using Core.Events;
using UnityEngine;
using Utils;

namespace Core
{
    public static class GameEventBus
    {
        private class ListenerWrapper<T> where T : IEvent
        {
            public Action<T> Listener;
            public Func<T, bool> Filter;
            public int Priority;
        }

        private static readonly Dictionary<Type, List<object>> Listeners = new();

        public static void Subscribe<T>(Action<T> listener, Func<T, bool> filter = null, int priority = 0) where T : IEvent
        {
            var list = GetListeners<T>();
            var wrapper = new ListenerWrapper<T>
            {
                Listener = listener,
                Filter = filter ?? (_ => true),
                Priority = priority
            };
            list.Add(wrapper);
            list.Sort((a, b) => ((ListenerWrapper<T>)b).Priority.CompareTo(((ListenerWrapper<T>)a).Priority));
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : IEvent
        {
            var list = GetListeners<T>();
            list.RemoveAll(w => ((ListenerWrapper<T>)w).Listener == listener);
        }

        public static void Publish<T>(in T evt) where T : IEvent
        {
            var listenersCopy = new List<object>(GetListeners<T>());
            foreach (var wrapperObj in listenersCopy)
            {
                var wrapper = (ListenerWrapper<T>)wrapperObj;
                if (wrapper.Filter(evt))
                    wrapper.Listener.Invoke(evt);
            }
        }

        public static void ClearListeners()
        {
            Listeners.Clear();
        }

        public static void LogListeners(bool includeUnity)
        {
            #if UNITY_EDITOR
            int totalListeners = 0;
            var sb = new StringBuilder();
    
            foreach (var listenerList in Listeners.Values)
            {
                foreach (var listenerObj in listenerList)
                {
                    var wrapperType = listenerObj.GetType();
            
                    var listenerField = wrapperType.GetField("Listener");
                    if (listenerField != null)
                    {
                        var actualListener = listenerField.GetValue(listenerObj);
                        var delegateInfo = actualListener as Delegate;
                        var target = delegateInfo?.Target;
                        var isUnityListener = target is MonoBehaviour;

                        if (!includeUnity && isUnityListener)
                            continue;
                        
                        totalListeners++;
                        var listenerType = actualListener.GetType();
                        var methodInfo = delegateInfo?.Method;
                        sb.AppendLine($"{listenerType.Name} -> {methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}, isUnity: {isUnityListener}");
                    }
                }
            }

            if (totalListeners > 0)
                GameLogger.Warn($"Possible memory Leak! ListenerCount: {totalListeners}, includeUnity {includeUnity}, \n Listener types: {sb}", nameof(GameEventBus));
            #endif
        }

        private static List<object> GetListeners<T>() where T : IEvent
        {
            var type = typeof(T);
            if (!Listeners.ContainsKey(type))
                Listeners[type] = new List<object>();
            return Listeners[type];
        }
    }

}