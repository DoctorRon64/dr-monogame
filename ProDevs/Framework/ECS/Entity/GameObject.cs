﻿using System;
using System.Collections.Generic;
using ProDevs.Framework.ECS.Components;

namespace ProDevs.Framework.ECS.Entity {
    public class GameObject {
        private static int nextId = 0;
        public int Id { get; }

        private readonly Dictionary<Type, Component> components = new();

        public GameObject() {
            Id = nextId++;
            Console.WriteLine("Object created with Id: " + Id);
        }

        public void AddComponent<T>(T component) where T : Component => components[typeof(T)] = component;
        public void RemoveComponent<T>() where T : Component => components.Remove(typeof(T));
        public void GetComponent<T>(out T component) where T : Component => component = (T)components[typeof(T)];

        public bool TryGetComponent<T>(out T component) where T : Component {
            if (components.TryGetValue(typeof(T), out Component storedComponent)) {
                component = (T)storedComponent;
                return true;
            }

            component = null;
            return false;
        }
    }
}