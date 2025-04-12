using System;
using System.Collections.Generic;

namespace MonoEngine.Framework {
    public class Observer {
        private readonly Dictionary<Delegate, SignalBase> alertListeners = new();

        public void AddListener(SignalBase signal, SignalBase.SignalDelegate signalDelegate) {
            signal.AddListener(signalDelegate);
            alertListeners.Add(signalDelegate, signal);
        }

        public void AddListener<T>(Signal<T> signal, SignalBase.SignalDelegate<T> signalDelegate) {
            signal.AddListener(signalDelegate);
            alertListeners.Add(signalDelegate, signal);
        }

        public void AddListener<T, TU>(Signal<T, TU> signal, SignalBase.SignalDelegate<T, TU> signalDelegate) {
            signal.AddListener(signalDelegate);
            alertListeners.Add(signalDelegate, signal);
        }

        public void RemoveListener(Signal signal, SignalBase.SignalDelegate signalDelegate) {
            signal.RemoveListener(signalDelegate);
            alertListeners.Remove(signalDelegate);
        }

        public void RemoveListener<T>(Signal<T> signal, SignalBase.SignalDelegate<T> signalDelegate) {
            signal.RemoveListener(signalDelegate);
            alertListeners.Remove(signalDelegate);
        }

        public void RemoveListener<T, TU>(Signal<T, TU> signal, SignalBase.SignalDelegate<T, TU> signalDelegate) {
            signal.RemoveListener(signalDelegate);
            alertListeners.Remove(signalDelegate);
        }

        public void Clear() {
            foreach (var alertListener in alertListeners) {
                alertListener.Value.Clear();
            }

            alertListeners.Clear();
        }
    }
}