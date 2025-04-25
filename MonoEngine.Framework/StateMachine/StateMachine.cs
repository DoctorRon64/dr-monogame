using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace MonoEngine.Framework
{
    public class StateMachine<T>
    {
        private IState<T> currentState;
        private readonly Dictionary<Type, IState<T>> allStates = new();
        public T Blackboard { get; private set; }

        public Signal<IState<T>> OnStateChanged { get; private set; } = new();

        public StateMachine(T blackboard, IState<T> startingState)
        {
            Blackboard = blackboard;
            currentState = startingState;

            Type type = startingState.GetType();
            allStates.TryAdd(type, currentState);

            currentState.OnInitialize(this);
            currentState.OnEnter();
        }

        public void Update(GameTime time)
        {
            currentState?.OnUpdate(time);
        }

        public IState<T> GetCurrentState() => currentState;

        public void SwitchState<U>() where U : IState<T>
        {
            if (!allStates.TryGetValue(typeof(U), out var nextState))
            {
                Console.WriteLine($"[{Blackboard}] Tried to switch to missing state: {typeof(U).Name}");
                return;
            }

            currentState?.OnExit();
            currentState = nextState;
            Console.WriteLine($"[{Blackboard}] Switched to: {typeof(U).Name}");
            OnStateChanged?.Invoke(currentState);
            currentState.OnEnter();
        }

        public void AddState<U>(bool switchTo = false) where U : IState<T>, new()
        {
            if (allStates.ContainsKey(typeof(U))) return;

            IState<T> stateInstance = new U();
            allStates[stateInstance.GetType()] = stateInstance;
            Console.WriteLine("initalize State: " + stateInstance);
            stateInstance.OnInitialize(this);

            if (switchTo) SwitchState<U>();
        }

        public void AddState<U>(U stateInstance) where U : IState<T>
        {
            Type stateType = typeof(U);
            if (allStates.ContainsKey(stateType)) return;
            allStates.Add(stateType, stateInstance);
            Console.WriteLine("initalize State: " + stateInstance);
            stateInstance.OnInitialize(this);
        }

        public void AddState<U>(Func<U> stateFactory, bool switchTo = false) where U : IState<T>
        {
            U stateInstance = stateFactory();
            AddState(stateInstance);
            if (switchTo) SwitchState<U>();
        }

        public void RemoveState<U>()
        {
            if (!allStates.ContainsKey(typeof(U))) return;
            allStates.Remove(typeof(U));
        }
    }

}
