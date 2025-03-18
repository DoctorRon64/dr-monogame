using System;
using Microsoft.Xna.Framework.Input;

namespace ProDevs.Framework
{
    public class Signal : SignalBase
    {
        public Signal(SignalDelegate @delegate) {
            AddListener(@delegate);
        }
        
        public void AddListener(SignalDelegate signalDelegate) => Listeners.Add(signalDelegate);
        public void RemoveListener (SignalDelegate signalDelegate) => Listeners.Remove(signalDelegate);

        public void Invoke()
        {
            foreach (Delegate listener in Listeners)
            {
                listener.DynamicInvoke();
            }
        }
    }

    public class Signal<T> : SignalBase
    {
        public void AddListener (SignalDelegate<T> signalDelegate) => Listeners.Add(signalDelegate);
        public void RemoveListener (SignalDelegate<T> signalDelegate) => Listeners.Remove(signalDelegate);

        public void Invoke(T parameter)
        {
            foreach (Delegate listener in Listeners)
            {
                listener.DynamicInvoke(parameter);
            }
        }
    }
    
    public class Signal<T, TU> : SignalBase
    {
        public void AddListener (SignalDelegate<T, TU> signalDelegate) => Listeners.Add(signalDelegate);
        public void RemoveListener (SignalDelegate<T, TU> signalDelegate) => Listeners.Remove(signalDelegate);

        public void Invoke(T parameter, TU parameter2)
        {
            foreach (Delegate listener in Listeners)
            {
                listener.DynamicInvoke(parameter, parameter2);
            }
        }
    }
}