#nullable enable
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContainerUnityExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IInstanceInjection RegisterComponent<TComponent>(this IObjectContainer container, TComponent component)
            where TComponent : Component
        {
            var statement = new ReturnComponentStatement(component, container);
            container.Registration.Register(statement);
            
            var source = statement.Source;
            source.ResolvedImmediately().As<TComponent>();

            return source;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISearchedComponentInjection RegisterComponentInHierarchy<T>(this IObjectContainer container, GameObject instance, SearchOrder order)
            where T : notnull
        {
            var statement = new ReturnComponentInHierarchyStatement(container, typeof(T), instance, order);
            
            container.Registration.Register(statement);

            statement.ResolvedImmediately();

            return statement;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static INamedComponentInjection RegisterComponentOnNewGameObject<TComponent>(this IObjectContainer container, Lifetime lifetime)
            where TComponent : Component
        {
            var statement = new CreateComponentOnNewGameObjectStatement(container, typeof(TComponent), lifetime);
            
            container.Registration.Register(statement);

            statement.As<TComponent>();

            return statement;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static INamedComponentInjection RegisterComponentOnNewGameObject<TInterface, TComponent>(this IObjectContainer container, Lifetime lifetime)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var injection = container.RegisterComponentOnNewGameObject<TComponent>(lifetime);

            injection.As<TInterface>();

            return injection;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICreatedComponentInjection RegisterComponentInNewPrefab<TComponent>(this IObjectContainer container, TComponent prefab, Lifetime lifetime)
            where TComponent : Component
        {
            var statement = new CreateComponentInNewPrefabStatement(container, lifetime, prefab);
            
            container.Registration.Register(statement);
            
            statement.As<TComponent>();

            return statement;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICreatedComponentInjection RegisterComponentInNewPrefab<TInterface, TComponent>(this IObjectContainer container, TComponent prefab, Lifetime lifetime)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var injection = container.RegisterComponentInNewPrefab(prefab, lifetime);

            injection.As<TInterface>();

            return injection;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDependencyInjection RegisterEntryPoint<T>(this IObjectContainer container, Lifetime lifetime = Lifetime.Global)
            where T : notnull
        {
            container.UseUnityEventLoop();

            var injection = container.Register<T>(lifetime);
            
            injection.AsImplementedInterfaces();

            return injection;
        }
        
        // TODO: use Lifecycle, instead of IAnchor?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, IAnchorTransform? anchor = null)
            where T : notnull
        {
            var statement = new ResolveFromUnitySubContainerStatement(container.Registration, typeof(T), prefab, anchor);
            
            container.Registration.Register(statement);
            
            return statement.TypeAssignment;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, Func<Transform> anchor)
            where T : notnull
        {
            return container.RegisterFromSubContainer<T>(prefab, new AnchorTransform(anchor));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, Transform parent)
            where T : notnull
        {
            return container.RegisterFromSubContainer<T>(prefab, new AnchorTransform(parent));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterHandler(this IObjectContainer container, Action<Exception> exceptionHandler)
        {
            container.RegisterInstance(new ExceptionHandler(exceptionHandler));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UseUnityEventLoop(this IObjectContainer container)
        {
            if (container.Count(statement => statement.ImplementedType == typeof(UnityEventLoopDispatcher)) > 0)
            {
                return;
            }
            
            container.Register<UnityEventLoopDispatcher>(Lifetime.Local);
            
            container.RegisterCallback<UnityEventLoopDispatcher>(instance => instance.Dispatch());
        }
    }
}
