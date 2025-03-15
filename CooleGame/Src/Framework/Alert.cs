using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace CooleGame.Framework
{
    public abstract class AlertBase
    {
        public delegate void AlertDelegate();
        public delegate void AlertDelegate<T>(T data);
        public delegate void AlertDelegate<T, TU>(T data, TU data2);
        
        protected readonly List<Delegate> Listeners = [];

        internal void AddListener(AlertDelegate listener)
        {
            if (Listeners.Contains(listener)) return;
            Listeners.Add(listener);
        }
        
        internal void RemoveListener(Delegate listener) => Listeners.Remove(listener);
        internal void Clear() => Listeners.Clear();
    }
    
    public class Alert : AlertBase
    {
        public void AddListener (AlertDelegate alertDelegate) => Listeners.Add(alertDelegate);
        public void RemoveListener (AlertDelegate alertDelegate) => Listeners.Remove(alertDelegate);

        public void Invoke()
        {
            foreach (Delegate listener in Listeners)
            {
                listener.DynamicInvoke();
            }
        }
    }

    public class Alert<T> : AlertBase
    {
        public void AddListener (AlertDelegate<T> alertDelegate) => Listeners.Add(alertDelegate);
        public void RemoveListener (AlertDelegate<T> alertDelegate) => Listeners.Remove(alertDelegate);

        public void Invoke(T parameter)
        {
            foreach (Delegate listener in Listeners)
            {
                listener.DynamicInvoke(parameter);
            }
        }
    }
    
    public class Alert<T, TU> : AlertBase
    {
        public void AddListener (AlertDelegate<T, TU> alertDelegate) => Listeners.Add(alertDelegate);
        public void RemoveListener (AlertDelegate<T, TU> alertDelegate) => Listeners.Remove(alertDelegate);

        public void Invoke(T parameter, TU parameter2)
        {
            foreach (Delegate listener in Listeners)
            {
                listener.DynamicInvoke(parameter, parameter2);
            }
        }
    }
    
    
}