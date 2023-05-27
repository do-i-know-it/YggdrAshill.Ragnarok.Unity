#nullable enable
using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using YggdrAshill.Ragnarok.Unity.Internal;
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity
{
    // TODO: add document comments.
    public static class ContainerExtension
    {
        public static IInstanceInjection RegisterComponent<T>(this IContainer container, T component)
            where T : notnull
        {
            var statement = new ComponentStatement(container, component);

            var composition = new Composition(Lifetime.Global, Ownership.External, statement);
            
            container.Register(composition);

            statement.As<T>();
            container.Register(resolver => resolver.Resolve<T>());

            return statement;
        }
        
        public static IInstanceInjection RegisterComponent<T>(this IContainer container, GameObject instance)
            where T : notnull
        {
            var componentType = typeof(T);

            var statement = new FindComponentInGameObjectStatement(container, componentType, instance);
            
            var composition = new Composition(Lifetime.Local, Ownership.External, statement);
            
            container.Register(composition);

            statement.As<T>();
            container.Register(resolver => resolver.Resolve<T>());

            return statement;
        }
        
        public static IInstanceInjection RegisterComponent<T>(this IContainer container)
            where T : notnull
        {
            var componentType = typeof(T);

            var statement = new FindComponentInHierarchyStatement(container, componentType);
            
            var composition = new Composition(Lifetime.Local, Ownership.External, statement);
            
            container.Register(composition);

            statement.As<T>();
            container.Register(resolver => resolver.Resolve<T>());

            return statement;
        }

        public static IComponentInjection RegisterTemporal<TComponent>(this IContainer container, string? objectName = null)
            where TComponent : Component
        {
            return container.Register<TComponent>(Lifetime.Temporal, objectName);
        }
        public static IComponentInjection RegisterLocal<TComponent>(this IContainer container, string? objectName = null)
            where TComponent : Component
        {
            return container.Register<TComponent>(Lifetime.Local, objectName);
        }
        public static IComponentInjection RegisterGlobal<TComponent>(this IContainer container, string? objectName = null)
            where TComponent : Component
        {
            return container.Register<TComponent>(Lifetime.Global, objectName);
        }
        private static IComponentInjection Register<TComponent>(this IContainer container, Lifetime lifetime, string? objectName)
            where TComponent : Component
        {
            var componentType = typeof(TComponent);

            var statement = new NewGameObjectStatement(container, componentType, objectName);

            var composition = new Composition(lifetime, Ownership.Internal, statement);
            
            container.Register(composition);

            return statement;
        }
        public static IComponentInjection RegisterTemporal<TInterface, TComponent>(this IContainer container, string? objectName = null)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var componentInjection = container.RegisterTemporal<TComponent>(objectName);

            componentInjection.As<TInterface>();

            return componentInjection;
        }
        public static IComponentInjection RegisterLocal<TInterface, TComponent>(this IContainer container, string? objectName = null)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var componentInjection = container.RegisterLocal<TComponent>(objectName);

            componentInjection.As<TInterface>();

            return componentInjection;
        }
        public static IComponentInjection RegisterGlobal<TInterface, TComponent>(this IContainer container, string? objectName = null)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var componentInjection = container.RegisterGlobal<TComponent>(objectName);

            componentInjection.As<TInterface>();

            return componentInjection;
        }
        
        public static IComponentInjection RegisterTemporal<TComponent>(this IContainer container, TComponent prefab)
            where TComponent : Component
        {
            return container.Register(Lifetime.Temporal, prefab);
        }
        public static IComponentInjection RegisterLocal<TComponent>(this IContainer container, TComponent prefab)
            where TComponent : Component
        {
            return container.Register(Lifetime.Local, prefab);
        }
        public static IComponentInjection RegisterGlobal<TComponent>(this IContainer container, TComponent prefab)
            where TComponent : Component
        {
            return container.Register(Lifetime.Global, prefab);
        }
        private static IComponentInjection Register<TComponent>(this IContainer container, Lifetime lifetime, TComponent prefab)
            where TComponent : Component
        {
            var statement = new NewPrefabStatement(container, prefab);

            var composition = new Composition(lifetime, Ownership.Internal, statement);
            
            container.Register(composition);

            return statement;
        }
        public static IComponentInjection RegisterTemporal<TInterface, TComponent>(this IContainer container, TComponent prefab)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var componentInjection = container.RegisterTemporal(prefab);

            componentInjection.As<TInterface>();

            return componentInjection;
        }
        public static IComponentInjection RegisterLocal<TInterface, TComponent>(this IContainer container, TComponent prefab)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var componentInjection = container.RegisterLocal(prefab);

            componentInjection.As<TInterface>();

            return componentInjection;
        }
        public static IComponentInjection RegisterGlobal<TInterface, TComponent>(this IContainer container, TComponent prefab)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var componentInjection = container.RegisterGlobal(prefab);

            componentInjection.As<TInterface>();

            return componentInjection;
        }
        
        public static void Register(this IContainer container, Action<Exception> exceptionHandler)
        {
            container.RegisterInstance(new ExceptionHandler(exceptionHandler));
        }

        public static void UseUnityEventLoop(this IContainer container)
        {
            container.RegisterLocal<UnityEventLoopDispatcher>();
            container.Register(resolver =>
            {
                resolver.Resolve<UnityEventLoopDispatcher>().Dispatch();
            });
        }
    }
}
