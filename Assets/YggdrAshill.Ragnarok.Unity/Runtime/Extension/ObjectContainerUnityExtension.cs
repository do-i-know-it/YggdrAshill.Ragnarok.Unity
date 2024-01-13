#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContainerUnityExtension
    {
        public static IInstanceInjection RegisterComponent<T>(this IObjectContainer container, T component)
            where T : notnull
        {
            container.Register(resolver => resolver.Resolve<T>());

            return container.RegisterInstance(() => component);
        }
        
        public static ISearchedComponentInjection RegisterComponent<T>(this IObjectContainer container, GameObject instance, SearchOrder order)
            where T : notnull
        {
            var statement = new ReturnComponentInGameObjectStatement(container.Compilation, typeof(T), instance, order);
            
            container.Register(statement);
            
            container.Register(resolver => resolver.Resolve<T>());

            return statement;
        }

        public static INamedComponentInjection RegisterComponentOnNewGameObject<TComponent>(this IObjectContainer container, Lifetime lifetime)
            where TComponent : Component
        {
            var statement = new CreateComponentOnNewGameObjectStatement(container.Compilation, typeof(TComponent), lifetime);
            
            container.Register(statement);

            statement.As<TComponent>();

            return statement;
        }

        public static INamedComponentInjection RegisterComponentOnNewGameObject<TInterface, TComponent>(this IObjectContainer container, Lifetime lifetime)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var injection = container.RegisterComponentOnNewGameObject<TComponent>(lifetime);

            injection.As<TInterface>();

            return injection;
        }
        
        public static ICreatedComponentInjection RegisterComponentInNewPrefab<TComponent>(this IObjectContainer container, TComponent prefab, Lifetime lifetime)
            where TComponent : Component
        {
            var statement = new CreateComponentInNewPrefabStatement(container.Compilation, lifetime, prefab);
            
            container.Register(statement);
            
            statement.As<TComponent>();

            return statement;
        }

        public static ICreatedComponentInjection RegisterComponentInNewPrefab<TInterface, TComponent>(this IObjectContainer container, TComponent prefab, Lifetime lifetime)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var injection = container.RegisterComponentInNewPrefab(prefab, lifetime);

            injection.As<TInterface>();

            return injection;
        }
        
        public static IDependencyInjection RegisterEntryPoint<T>(this IObjectContainer container, Lifetime lifetime = Lifetime.Global)
            where T : notnull
        {
            container.UseUnityEventLoop();

            var injection = container.Register<T>(lifetime);
            
            injection.AsImplementedInterfaces();

            return injection;
        }
        
        // TODO: use Lifecycle, instead of IAnchor?
        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, IAnchorTransform? anchor = null)
            where T : notnull
        {
            var statement = new ResolveFromUnitySubContainerStatement(container, typeof(T), prefab, anchor);
            
            container.Register(statement);
            
            return statement.TypeAssignment;
        }
        
        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, Func<Transform> anchor)
            where T : notnull
        {
            return container.RegisterFromSubContainer<T>(prefab, new AnchorTransform(anchor));
        }

        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, Transform parent)
            where T : notnull
        {
            return container.RegisterFromSubContainer<T>(prefab, new AnchorTransform(parent));
        }
        
        public static void RegisterHandler(this IObjectContainer container, Action<Exception> exceptionHandler)
        {
            container.RegisterInstance(new ExceptionHandler(exceptionHandler));
        }

        public static void UseUnityEventLoop(this IObjectContainer container)
        {
            if (container.Count(statement => statement.ImplementedType == typeof(UnityEventLoopDispatcher)) > 0)
            {
                return;
            }
            
            container.Register<UnityEventLoopDispatcher>(Lifetime.Local);
            
            container.Register(resolver =>
            {
                resolver.Resolve<UnityEventLoopDispatcher>().Dispatch();
            });
        }
    }
}
