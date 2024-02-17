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
        public static IDependencyInjection RegisterEntryPoint<T>(this IObjectContainer container)
            where T : notnull
        {
            container.UseUnityEventLoop();

            var injection = container.Register<T>(Lifetime.Global);
            
            injection.AsImplementedInterfaces();

            return injection;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnitySubContainerResolution RegisterFromSubContainerInNewPrefab<T>(this IObjectContainer container, GameObjectLifecycle prefab)
            where T : notnull
        {
            var statement = new ResolveFromSubContainerInNewPrefabStatement(typeof(T), prefab, container);
            
            container.Registration.Register(statement);
            
            return statement;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnitySubContainerResolution RegisterFromSubContainerOnNewGameObject<T>(this IObjectContainer container, IInstallation installation)
            where T : notnull
        {
            var statement = new ResolveFromSubContainerOnNewGameObjectStatement(typeof(T), container);
            
            container.Registration.Register(statement);

            statement.With(installation);
            
            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnitySubContainerResolution RegisterFromSubContainerOnNewGameObject<T>(this IObjectContainer container, Action<IObjectContainer> installation)
            where T : notnull
        {
            var statement = new ResolveFromSubContainerOnNewGameObjectStatement(typeof(T), container);
            
            container.Registration.Register(statement);

            statement.With(installation);

            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnitySubContainerResolution RegisterFromSubContainerOnNewGameObject<TInstance, TInstallation>(this IObjectContainer container)
            where TInstance : notnull
            where TInstallation : IInstallation
        {
            var statement = new ResolveFromSubContainerOnNewGameObjectStatement(typeof(TInstance), container);
            
            container.Registration.Register(statement);

            statement.With<TInstallation>();

            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnityFactoryResolution RegisterFactoryInNewPrefab<T>(this IObjectContainer container, GameObjectLifecycle prefab, Ownership ownership)
            where T : notnull
        {
            var statement = new CreateFactoryInNewPrefabStatement<T>(container, prefab, ownership);
            
            container.Registration.Register(statement);

            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnityFactoryResolution RegisterFactoryOnNewGameObject<T>(this IObjectContainer container, IInstallation installation, Ownership ownership)
            where T : notnull
        {
            var statement = new CreateFactoryOnNewGameObjectStatement<T>(container, ownership);
            
            container.Registration.Register(statement);

            statement.With(installation);

            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnityFactoryResolution RegisterFactoryOnNewGameObject<T>(this IObjectContainer container, Action<IObjectContainer> installation, Ownership ownership)
            where T : notnull
        {
            var statement = new CreateFactoryOnNewGameObjectStatement<T>(container, ownership);
            
            container.Registration.Register(statement);

            statement.With(installation);

            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnityFactoryResolution RegisterFactoryOnNewGameObject<TInstance, TInstallation>(this IObjectContainer container, Ownership ownership)
            where TInstance : notnull
            where TInstallation : IInstallation
        {
            var statement = new CreateFactoryOnNewGameObjectStatement<TInstance>(container, ownership);
            
            container.Registration.Register(statement);

            statement.With<TInstallation>();

            return statement;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnityFactoryResolution RegisterFactoryInNewPrefab<TInput, TOutput>(this IObjectContainer container, GameObjectLifecycle prefab, Ownership ownership)
            where TInput : notnull
            where TOutput : notnull
        {
            var statement = new CreateFactoryInNewPrefabStatement<TInput, TOutput>(container, prefab, ownership);
            
            container.Registration.Register(statement);

            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnityFactoryResolution RegisterFactoryOnNewGameObject<TInput, TOutput>(this IObjectContainer container, IInstallation installation, Ownership ownership)
            where TInput : notnull
            where TOutput : notnull
        {
            var statement = new CreateFactoryOnNewGameObjectStatement<TInput, TOutput>(container, ownership);
            
            container.Registration.Register(statement);

            statement.With(installation);

            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnityFactoryResolution RegisterFactoryOnNewGameObject<TInput, TOutput>(this IObjectContainer container, Action<IObjectContainer> installation, Ownership ownership)
            where TInput : notnull
            where TOutput : notnull
        {
            var statement = new CreateFactoryOnNewGameObjectStatement<TInput, TOutput>(container, ownership);
            
            container.Registration.Register(statement);

            statement.With(installation);

            return statement;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUnityFactoryResolution RegisterFactoryOnNewGameObject<TInput, TOutput, TInstallation>(this IObjectContainer container, Ownership ownership)
            where TInput : notnull
            where TOutput : notnull
            where TInstallation : IInstallation
        {
            var statement = new CreateFactoryOnNewGameObjectStatement<TInput, TOutput>(container, ownership);
            
            container.Registration.Register(statement);

            statement.With<TInstallation>();

            return statement;
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
