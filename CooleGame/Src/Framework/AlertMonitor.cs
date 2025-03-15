using System;
using System.Collections.Generic;

namespace CooleGame.Framework
{
    public class AlertMonitor
    {
        private readonly Dictionary<Delegate, AlertBase> alertListeners = new();

        public void AddListener(AlertBase alert, AlertBase.AlertDelegate alertDelegate)
        {
            alert.AddListener(alertDelegate);
            alertListeners.Add(alertDelegate, alert);
        }
        
        public void AddListener<T>(Alert<T> alert, AlertBase.AlertDelegate<T> alertDelegate)
        {
            alert.AddListener(alertDelegate);
            alertListeners.Add(alertDelegate, alert);
        }

        public void AddListener<T, TU>(Alert<T, TU> alert, AlertBase.AlertDelegate<T, TU> alertDelegate)
        {
            alert.AddListener(alertDelegate);
            alertListeners.Add(alertDelegate, alert);
        }

        public void RemoveListener(Alert alert, AlertBase.AlertDelegate alertDelegate)
        {
            alert.RemoveListener(alertDelegate);
            alertListeners.Remove(alertDelegate);
        }

        public void RemoveListener<T>(Alert<T> alert, AlertBase.AlertDelegate<T> alertDelegate)
        {
            alert.RemoveListener(alertDelegate);
            alertListeners.Remove(alertDelegate);
        }

        public void RemoveListener<T, TU>(Alert<T, TU> alert, AlertBase.AlertDelegate<T, TU> alertDelegate)
        {
            alert.RemoveListener(alertDelegate);
            alertListeners.Remove(alertDelegate);
        }

        public void Clear()
        {
            foreach (var alertListener in alertListeners)
            {
                alertListener.Value.Clear();
            }
            alertListeners.Clear();
        }
    }
}