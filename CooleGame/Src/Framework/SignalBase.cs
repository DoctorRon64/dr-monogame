using System;
using System.Collections.Generic;

namespace CooleGame.Framework
{
    public abstract class SignalBase
    {
        public delegate void SignalDelegate();
        public delegate void SignalDelegate<T>(T data);
        public delegate void SignalDelegate<T, TU>(T data, TU data2);
        
        protected readonly List<Delegate> Listeners = [];

        internal void AddListener(SignalDelegate listener)
        {
            if (Listeners.Contains(listener)) return;
            Listeners.Add(listener);
        }
        
        internal void RemoveListener(Delegate listener) => Listeners.Remove(listener);
        internal void Clear() => Listeners.Clear();
    }
}