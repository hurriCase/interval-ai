﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Client.Scripts.Patterns.DI.Base
{
    internal static class DependencyInjector
    {
        private static readonly ConcurrentDictionary<Type, FieldInfo[]> _fieldCache = new();

        internal static void InjectDependencies(object target)
        {
            if (target == null) return;

            var targetType = target.GetType();
            var injectableFields = GetInjectableFields(targetType);

            foreach (var field in injectableFields)
            {
                var injectAttribute = field.GetCustomAttribute<InjectAttribute>();
                if (injectAttribute == null) continue;

                try
                {
                    var dependency = DIContainer.Resolve(field.FieldType);

                    field.SetValue(target, dependency);
                }
                catch (Exception e)
                {
                    Debug.LogError(
                        "Dependency Injection Failed: " +
                        $"Type: {targetType.Name}, " +
                        $"Field: {field.Name}, " +
                        $"Error: {e.Message}");
                }
            }
        }

        private static FieldInfo[] GetInjectableFields(Type type)
        {
            return _fieldCache.GetOrAdd(type, t =>
                t.GetFields(
                        BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.Instance
                    ).Where(f => f.GetCustomAttribute<InjectAttribute>() != null)
                    .ToArray()
            );
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class InjectAttribute : Attribute { }

    internal abstract class Injectable
    {
        protected Injectable() => DependencyInjector.InjectDependencies(this);
    }

    internal abstract class InjectableBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DependencyInjector.InjectDependencies(this);

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }

    internal interface IInjectable
    {
        void InjectDependencies();
    }
}