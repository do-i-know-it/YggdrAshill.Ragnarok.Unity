#nullable enable
using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace YggdrAshill.Ragnarok.Unity
{
    internal sealed class UnityEventLoopDispatcher :
        IDisposable
    {
        private static UnityEventLoopRunner PreFixedUpdateRunner { get; } = new UnityEventLoopRunner();
        private static UnityEventLoopRunner PostFixedUpdateRunner { get; } = new UnityEventLoopRunner();
        private static UnityEventLoopRunner PreUpdateRunner { get; } = new UnityEventLoopRunner();
        private static UnityEventLoopRunner PostUpdateRunner { get; } = new UnityEventLoopRunner();
        private static UnityEventLoopRunner PreLateUpdateRunner { get; } = new UnityEventLoopRunner();
        private static UnityEventLoopRunner PostLateUpdateRunner { get; } = new UnityEventLoopRunner();
        
        static UnityEventLoopDispatcher()
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            
            var buffer = playerLoop.subSystemList;

            ref var fixedUpdateSystem = ref FindSubSystem(typeof(FixedUpdate), buffer);
            InsertSubsystem(
                ref fixedUpdateSystem,
                typeof(FixedUpdate.ScriptRunBehaviourFixedUpdate),
                new PlayerLoopSystem
                {
                    type = typeof(RagnarokPreFixedUpdate),
                    updateDelegate = PreFixedUpdateRunner.Run
                },
                new PlayerLoopSystem
                {
                    type = typeof(RagnarokPostFixedUpdate),
                    updateDelegate = PostFixedUpdateRunner.Run
                });
            
            ref var updateSystem = ref FindSubSystem(typeof(Update), buffer);
            InsertSubsystem(
                ref updateSystem,
                typeof(Update.ScriptRunBehaviourUpdate),
                new PlayerLoopSystem
                {
                    type = typeof(RagnarokPreUpdate),
                    updateDelegate = PreUpdateRunner.Run
                },
                new PlayerLoopSystem
                {
                    type = typeof(RagnarokPostUpdate),
                    updateDelegate = PostUpdateRunner.Run
                });

            ref var lateUpdateSystem = ref FindSubSystem(typeof(PreLateUpdate), buffer);
            InsertSubsystem(
                ref lateUpdateSystem,
                typeof(PreLateUpdate.ScriptRunBehaviourLateUpdate),
                new PlayerLoopSystem
                {
                    type = typeof(RagnarokPreLateUpdate),
                    updateDelegate = PreLateUpdateRunner.Run
                },
                new PlayerLoopSystem
                {
                    type = typeof(RagnarokPostLateUpdate),
                    updateDelegate = PostLateUpdateRunner.Run
                });
            
            playerLoop.subSystemList = buffer;

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        private static ref PlayerLoopSystem FindSubSystem(Type targetType, PlayerLoopSystem[] systemList)
        {
            for (var index = 0; index < systemList.Length; index++)
            {
                if (systemList[index].type == targetType)
                {
                    return ref systemList[index];
                }
            }
            
            throw new InvalidOperationException($"{targetType.FullName} not found in unity event loop.");
        }
        
        private static void InsertSubsystem(ref PlayerLoopSystem parentSystem, Type targetType, PlayerLoopSystem preSystem, PlayerLoopSystem postSystem)
        {
            var source = parentSystem.subSystemList;
            
            var insertIndex = FindIndex(source, targetType);

            if (insertIndex < 0)
            {
                throw new ArgumentException($"{targetType.FullName} not found in system {parentSystem} {parentSystem.type.FullName}");
            }
            
            var dest = new PlayerLoopSystem[source.Length + 2];
            
            for (var index = 0; index < source.Length; index++)
            {
                if (index < insertIndex)
                {
                    dest[index] = source[index];
                }
                else if (index == insertIndex)
                {
                    dest[index] = preSystem;
                    dest[index + 1] = source[index];
                    dest[index + 2] = postSystem;
                }
                else
                {
                    dest[index + 2] = source[index];
                }
            }

            parentSystem.subSystemList = dest;
        }
        
        private static int FindIndex(PlayerLoopSystem[] systemList, Type targetType)
        {
            for (var index = 0; index < systemList.Length; index++)
            {
                if (systemList[index].type == targetType)
                {
                    return index;
                }
            }

            return -1;
        }
        
        private readonly IResolver resolver;

        [Inject]
        public UnityEventLoopDispatcher(IResolver resolver)
        {
            this.resolver = resolver;
        }

        private readonly List<IDisposable> disposableList = new List<IDisposable>();

        public void Dispatch()
        {
            var exceptionHandler = ResolveUnityEventLoopExceptionHandler();

            ExecuteInitializableList(exceptionHandler);

            DispatchPreFixedUpdatableList(exceptionHandler);
            DispatchPostFixedUpdatableList(exceptionHandler);

            DispatchPreUpdatableList(exceptionHandler);
            DispatchPostUpdatableList(exceptionHandler);

            DispatchPreLateUpdatableList(exceptionHandler);
            DispatchPostLateUpdatableList(exceptionHandler);
        }
        
        private UnityEventLoopExceptionHandler? ResolveUnityEventLoopExceptionHandler()
        {
            var exceptionHandlerList = resolver.Resolve<IReadOnlyList<IExceptionHandler>>();

            if (exceptionHandlerList.Count == 0)
            {
                return null;
            }
            
            return new UnityEventLoopExceptionHandler(exceptionHandlerList);
        }

        private void ExecuteInitializableList(UnityEventLoopExceptionHandler? exceptionHandler)
        {
            var initializableList = resolver.Resolve<IServiceBundle<IInitializable>>().Package;
            for (var index = 0; index < initializableList.Count; index++)
            {
                try
                {
                    initializableList[index].Initialize();
                }
                catch (Exception exception)
                {
                    if (exceptionHandler == null)
                    {
                        throw;
                    }
                    
                    exceptionHandler.Invoke(exception);
                }
            }
        }

        private void DispatchPreFixedUpdatableList(UnityEventLoopExceptionHandler? exceptionHandler)
        {
            var preFixedUpdatableList = resolver.Resolve<IServiceBundle<IPreFixedUpdatable>>().Package;

            if (preFixedUpdatableList.Count == 0)
            {
                return;
            }
            
            var processor = new PreFixedUpdateProcessor(preFixedUpdatableList, exceptionHandler);
            disposableList.Add(processor);
            PreFixedUpdateRunner.Dispatch(processor);
        }
        private void DispatchPostFixedUpdatableList(UnityEventLoopExceptionHandler? exceptionHandler)
        {
            var preFixedUpdatableList = resolver.Resolve<IServiceBundle<IPostFixedUpdatable>>().Package;

            if (preFixedUpdatableList.Count == 0)
            {
                return;
            }
            
            var processor = new PostFixedUpdateProcessor(preFixedUpdatableList, exceptionHandler);
            disposableList.Add(processor);
            PostFixedUpdateRunner.Dispatch(processor);
        }
        
        private void DispatchPreUpdatableList(UnityEventLoopExceptionHandler? exceptionHandler)
        {
            var preFixedUpdatableList = resolver.Resolve<IServiceBundle<IPreUpdatable>>().Package;

            if (preFixedUpdatableList.Count == 0)
            {
                return;
            }
            
            var processor = new PreUpdateProcessor(preFixedUpdatableList, exceptionHandler);
            disposableList.Add(processor);
            PreUpdateRunner.Dispatch(processor);
        }
        private void DispatchPostUpdatableList(UnityEventLoopExceptionHandler? exceptionHandler)
        {
            var preFixedUpdatableList = resolver.Resolve<IServiceBundle<IPostUpdatable>>().Package;

            if (preFixedUpdatableList.Count == 0)
            {
                return;
            }
            
            var processor = new PostUpdateProcessor(preFixedUpdatableList, exceptionHandler);
            disposableList.Add(processor);
            PostUpdateRunner.Dispatch(processor);
        }

        private void DispatchPreLateUpdatableList(UnityEventLoopExceptionHandler? exceptionHandler)
        {
            var preLateUpdatableList = resolver.Resolve<IServiceBundle<IPreLateUpdatable>>().Package;

            if (preLateUpdatableList.Count == 0)
            {
                return;
            }
            
            var processor = new PreLateUpdateProcessor(preLateUpdatableList, exceptionHandler);
            disposableList.Add(processor);
            PreLateUpdateRunner.Dispatch(processor);
        }
        private void DispatchPostLateUpdatableList(UnityEventLoopExceptionHandler? exceptionHandler)
        {
            var preLateUpdatableList = resolver.Resolve<IServiceBundle<IPostLateUpdatable>>().Package;

            if (preLateUpdatableList.Count == 0)
            {
                return;
            }
            
            var processor = new PostLateUpdateProcessor(preLateUpdatableList, exceptionHandler);
            disposableList.Add(processor);
            PostLateUpdateRunner.Dispatch(processor);
        }

        public void Dispose()
        {
            foreach (var disposable in disposableList)
            {
                disposable.Dispose();
            }
            
            disposableList.Clear();
        }
    }
}
