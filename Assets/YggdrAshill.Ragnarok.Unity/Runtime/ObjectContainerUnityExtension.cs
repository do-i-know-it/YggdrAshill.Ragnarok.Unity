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
        
        public static IInstanceInjection RegisterComponent<T>(this IObjectContainer container, GameObject instance)
            where T : notnull
        {
            container.Register(resolver => resolver.Resolve<T>());

            return container.RegisterInstance(() => ReturnComponentInGameObject<T>(instance));
        }

        private static T ReturnComponentInGameObject<T>(GameObject instance)
            where T : notnull
        {
            // TODO: instance.GetComponentInParent<T>(true)?
            // TODO: includeInactive = false?
            // TODO: var scene = instance.scene?
            var component = instance.GetComponentInChildren<T>(true);

            if (component == null)
            {
                var type = typeof(T);
                throw new RagnarokException(type, $"{type} is not in {instance}.");
            }
            
            return component;
        }
        
        public static IComponentInjection RegisterComponentOnNewGameObject<TComponent>(this IObjectContainer container, Lifetime lifetime, string? objectName = null)
            where TComponent : Component
        {
            var statement = new CreateComponentOnNewGameObjectStatement(container.Compilation, typeof(TComponent), lifetime, objectName);
            
            container.Registration.Register(statement);

            statement.As<TComponent>();

            return statement;
        }

        public static IComponentInjection RegisterComponentOnNewGameObject<TInterface, TComponent>(this IObjectContainer container, Lifetime lifetime, string? objectName = null)
            where TInterface : notnull
            where TComponent : Component, TInterface
        {
            var injection = container.RegisterComponentOnNewGameObject<TComponent>(lifetime, objectName);

            injection.As<TInterface>();

            return injection;
        }
        
        public static IComponentInjection RegisterComponentInNewPrefab<TComponent>(this IObjectContainer container, TComponent prefab, Lifetime lifetime)
            where TComponent : Component
        {
            var statement = new CreateComponentInNewPrefabStatement(container.Compilation, lifetime, prefab);
            
            container.Registration.Register(statement);
            
            statement.As<TComponent>();

            return statement;
        }

        public static IComponentInjection RegisterComponentInNewPrefab<TInterface, TComponent>(this IObjectContainer container, TComponent prefab, Lifetime lifetime)
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
        
        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, IAnchor? anchor = null)
            where T : notnull
        {
            var statement = new ResolveFromUnitySubContainerStatement(container.Registration, typeof(T), prefab, anchor);
            
            container.Registration.Register(statement);
            
            return statement.TypeAssignment;
        }
        
        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, Func<Transform> getTransform)
            where T : notnull
        {
            return container.RegisterFromSubContainer<T>(prefab, new Anchor(getTransform));
        }

        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, GameObjectLifecycle prefab, Transform parent)
            where T : notnull
        {
            return container.RegisterFromSubContainer<T>(prefab, () => parent);
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
