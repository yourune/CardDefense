using System;
using System.Collections.Generic;

namespace BG_Games.Card_Game_Core.Systems.EventsBus
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> eventListeners = new();

        /// <summary>
        /// Subscribes a listener to a specific event type.
        /// </summary>
        public static void Subscribe<T>(Action<T> listener)
        {
            if (!eventListeners.ContainsKey(typeof(T)))
            {
                eventListeners[typeof(T)] = new List<Delegate>();
            }

            eventListeners[typeof(T)].Add(listener);
        }

        /// <summary>
        /// Unsubscribes a listener from a specific event type.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> listener)
        {
            if (eventListeners.TryGetValue(typeof(T), out var listeners))
            {
                listeners.Remove(listener);
                if (listeners.Count == 0)
                {
                    eventListeners.Remove(typeof(T));
                }
            }
        }

        /// <summary>
        /// Publishes an event to all subscribed listeners.
        /// </summary>
        public static void Publish<T>(T eventData)
        {
            if (eventListeners.TryGetValue(typeof(T), out var listeners))
            {
                foreach (var listener in listeners)
                {
                    ((Action<T>)listener)?.Invoke(eventData);
                }
            }
        }
    }
}