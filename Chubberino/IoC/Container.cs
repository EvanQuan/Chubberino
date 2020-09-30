using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chubberino.IoC
{
    /// <summary>
    /// Inversion of control container.
    /// </summary>
    public sealed class Container
    {
        private Dictionary<Type, Func<Object>> Registrations { get; } = new Dictionary<Type, Func<Object>>();

        public void Register<TService, TImplementation>() where TImplementation : TService
        {
            Registrations.Add(typeof(TService), () => GetInstance(typeof(TImplementation)));
        }

        public void Register<TService>(Func<TService> instanceCreator)
        {
            Registrations.Add(typeof(TService), () => instanceCreator());
        }

        public void RegisterSingleton<TService>(TService instance)
        {
            Registrations.Add(typeof(TService), () => instance);
        }

        public void RegisterSingleton<TService>(Func<TService> instanceCreator)
        {
            var lazy = new Lazy<TService>(instanceCreator);
            Register(() => lazy.Value);
        }

        public Object GetInstance(Type serviceType)
        {
            if (Registrations.TryGetValue(serviceType, out Func<Object> creator))
            {
                return creator();
            }
            else if (!serviceType.IsAbstract)
            {
                return CreateInstance(serviceType);
            }
            else
            {
                throw new InvalidOperationException("No registration for " + serviceType);
            }
        }

        private Object CreateInstance(Type implementationType)
        {
            ConstructorInfo ctor = implementationType.GetConstructors().Single();
            IEnumerable<Type> parameterTypes = ctor.GetParameters().Select(p => p.ParameterType);
            Object[] dependencies = parameterTypes.Select(t => GetInstance(t)).ToArray();
            return Activator.CreateInstance(implementationType, dependencies);
        }
    }
}
