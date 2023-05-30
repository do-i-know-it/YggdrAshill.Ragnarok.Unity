#nullable enable
using YggdrAshill.Ragnarok.Unity;
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
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
            
            var composition = new Composition(Lifetime.Global, Ownership.External, statement);
            
            container.Register(composition);

            statement.As<T>();
            container.Register(resolver => resolver.Resolve<T>());

            return statement;
        }
        
        public static IComponentInjection RegisterComponentOnNewGameObject<TComponent>(this IContainer container, Lifetime lifetime, string? objectName = null)
            where TComponent : Component
        {
            var componentType = typeof(TComponent);

            var statement = new NewGameObjectStatement(container, componentType, objectName);

            var composition = new Composition(lifetime, Ownership.Internal, statement);
            
            container.Register(composition);

            return statement;
        }

        public static IComponentInjection RegisterComponentOnNewGameObject<TInterface, TComponent>(this IContainer container, Lifetime lifetime, string? objectName = null)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var componentInjection = container.RegisterComponentOnNewGameObject<TComponent>(lifetime, objectName);

            componentInjection.As<TInterface>();

            return componentInjection;
        }
        
        public static IComponentInjection RegisterComponentInNewPrefab<TComponent>(this IContainer container, TComponent prefab, Lifetime lifetime)
            where TComponent : Component
        {
            var statement = new NewPrefabStatement(container, prefab);

            var composition = new Composition(lifetime, Ownership.Internal, statement);
            
            container.Register(composition);

            return statement;
        }

        public static IComponentInjection RegisterComponentInNewPrefab<TInterface, TComponent>(this IContainer container, TComponent prefab, Lifetime lifetime)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var componentInjection = container.RegisterComponentInNewPrefab(prefab, lifetime);

            componentInjection.As<TInterface>();

            return componentInjection;
        }
        
        public static void Register(this IContainer container, Action<Exception> exceptionHandler)
        {
            container.RegisterInstance(new ExceptionHandler(exceptionHandler));
        }

        public static void UseUnityEventLoop(this IContainer container)
        {
            container.Register<UnityEventLoopDispatcher>(Lifetime.Local);
            container.Register(resolver =>
            {
                resolver.Resolve<UnityEventLoopDispatcher>().Dispatch();
            });
        }
    }
}
