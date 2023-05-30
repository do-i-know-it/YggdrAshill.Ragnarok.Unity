using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Random = System.Random;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(UnityDependencyContext))]
    internal sealed class UnityDependencyContextSpecification
    {
        [Test]
        public void ShouldResolveComponentOnNewGameObjectPerRequest()
        {
            var parentContext = new UnityDependencyContext();
            
            parentContext.RegisterComponentOnNewGameObject<NoDependencyComponent>(Lifetime.Temporal);

            using var parentScope = parentContext.Build();

            var component1 = parentScope.Resolver.Resolve<NoDependencyComponent>();
            var component2 = parentScope.Resolver.Resolve<NoDependencyComponent>();

            Assert.That(component1, Is.Not.EqualTo(component2));

            using var childScope = parentScope.CreateChildScope();

            var component3 = childScope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component3, Is.Not.EqualTo(component1));
            Assert.That(component3, Is.Not.EqualTo(component2));
        }
        
        [Test]
        public void ShouldResolveComponentOnNewGameObjectPerLocalScope()
        {
            var parentContext = new UnityDependencyContext();
           
            parentContext.RegisterComponentOnNewGameObject<NoDependencyComponent>(Lifetime.Local);

            using var parentScope = parentContext.Build();

            var component1 = parentScope.Resolver.Resolve<NoDependencyComponent>();
            var component2 = parentScope.Resolver.Resolve<NoDependencyComponent>();

            Assert.That(component1, Is.EqualTo(component2));

            using var childScope = parentScope.CreateChildScope();

            var component3 = childScope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component3, Is.Not.EqualTo(component1));
            Assert.That(component3, Is.Not.EqualTo(component2));
        }
        
        [Test]
        public void ShouldResolveComponentOnNewGameObjectPerGlobalScope()
        {
            var parentContext = new UnityDependencyContext();
            
            parentContext.RegisterComponentOnNewGameObject<NoDependencyComponent>(Lifetime.Global);

            using var parentScope = parentContext.Build();

            var component1 = parentScope.Resolver.Resolve<NoDependencyComponent>();
            var component2 = parentScope.Resolver.Resolve<NoDependencyComponent>();

            Assert.That(component1, Is.EqualTo(component2));

            using var childScope = parentScope.CreateChildScope();

            var component3 = childScope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component3, Is.EqualTo(component1));
            Assert.That(component3, Is.EqualTo(component2));
        }
        
        [Test]
        public void ShouldResolveComponentOnNewGameObjectUnderTransform()
        {
            var parent = new GameObject("Parent").transform;

            var context = new UnityDependencyContext();
            
            context.RegisterComponentOnNewGameObject<NoDependencyComponent>(Lifetime.Temporal).Under(parent);

            using var scope = context.Build();

            var component = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component.transform.parent, Is.EqualTo(parent));
        }
        
        [Test]
        public void ShouldResolveComponentInNewPrefabPerRequest()
        {
            var prefab = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();
            
            var parentContext = new UnityDependencyContext();
            
            parentContext.RegisterComponentInNewPrefab(prefab, Lifetime.Temporal);

            using var parentScope = parentContext.Build();

            var component1 = parentScope.Resolver.Resolve<NoDependencyComponent>();
            var component2 = parentScope.Resolver.Resolve<NoDependencyComponent>();

            Assert.That(component1, Is.Not.EqualTo(component2));

            using var childScope = parentScope.CreateChildScope();

            var component3 = childScope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component3, Is.Not.EqualTo(component1));
            Assert.That(component3, Is.Not.EqualTo(component2));
        }
        
        [Test]
        public void ShouldResolveComponentInNewPrefabPerLocalScope()
        {
            var prefab = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();

            var parentContext = new UnityDependencyContext();

            parentContext.RegisterComponentInNewPrefab(prefab, Lifetime.Local);

            using var parentScope = parentContext.Build();

            var component1 = parentScope.Resolver.Resolve<NoDependencyComponent>();
            var component2 = parentScope.Resolver.Resolve<NoDependencyComponent>();

            Assert.That(component1, Is.EqualTo(component2));

            using var childScope = parentScope.CreateChildScope();

            var component3 = childScope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component3, Is.Not.EqualTo(component1));
            Assert.That(component3, Is.Not.EqualTo(component2));
        }
        
        [Test]
        public void ShouldResolveComponentInNewPrefabPerGlobalScope()
        {
            var prefab = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();

            var parentContext = new UnityDependencyContext();
            
            parentContext.RegisterComponentInNewPrefab(prefab, Lifetime.Global);

            using var parentScope = parentContext.Build();

            var component1 = parentScope.Resolver.Resolve<NoDependencyComponent>();
            var component2 = parentScope.Resolver.Resolve<NoDependencyComponent>();

            Assert.That(component1, Is.EqualTo(component2));

            using var childScope = parentScope.CreateChildScope();

            var component3 = childScope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component3, Is.EqualTo(component1));
            Assert.That(component3, Is.EqualTo(component2));
        }

        [Test]
        public void ShouldResolveComponentInNewPrefabUnderTransform()
        {
            var parent = new GameObject("Parent").transform;
            var prefab = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();

            var context = new UnityDependencyContext();
            
            context.RegisterComponentInNewPrefab(prefab, Lifetime.Temporal).Under(parent);

            using var scope = context.Build();

            var component = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component.transform.parent, Is.EqualTo(parent));
        }

        [Test]
        public void ShouldResolveComponentInstantiatedExternally()
        {
            var component = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();
 
            var context = new UnityDependencyContext();
            
            context.RegisterComponent(component);

            using var scope = context.Build();

            var resolved = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(resolved, Is.EqualTo(component));
        }
        
        [Test]
        public void ShouldResolveComponentInGameObject()
        {
            var component = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();
 
            var context = new UnityDependencyContext();
            
            context.RegisterComponent<NoDependencyComponent>(component.gameObject);

            using var scope = context.Build();

            var resolved = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(resolved, Is.EqualTo(component));
        }

        [UnityTest]
        public IEnumerator ShouldUseUnityEventLoop()
        {
            var context = new UnityDependencyContext();

            context.Register<UnityEventLoop>(Lifetime.Global).AsImplementedInterfaces();
            context.UseUnityEventLoop();

            var scope = context.Build();

            var initializable = scope.Resolver.Resolve<IInitializable>();
            var preUpdatable = scope.Resolver.Resolve<IPreUpdatable>();
            var postUpdatable = scope.Resolver.Resolve<IPostUpdatable>();
            var preLateUpdatable = scope.Resolver.Resolve<IPreLateUpdatable>();
            var postLateUpdatable = scope.Resolver.Resolve<IPostLateUpdatable>();
            var disposable = scope.Resolver.Resolve<IDisposable>();
            
            Assert.That(initializable, Is.InstanceOf<UnityEventLoop>());
            Assert.That(preUpdatable, Is.InstanceOf<UnityEventLoop>());
            Assert.That(postUpdatable, Is.InstanceOf<UnityEventLoop>());
            Assert.That(preLateUpdatable, Is.InstanceOf<UnityEventLoop>());
            Assert.That(postLateUpdatable, Is.InstanceOf<UnityEventLoop>());
            Assert.That(disposable, Is.InstanceOf<UnityEventLoop>());

            var unityEventLoop = (UnityEventLoop)initializable;
            
            Assert.That(unityEventLoop.Initialized, Is.True);

            var frameCount = new Random().Next(1, 60);

            foreach (var _ in Enumerable.Range(0, frameCount))
            {
                yield return null;
            }

            Assert.That(unityEventLoop.PreUpdateExecutedCount, Is.EqualTo(frameCount));
            Assert.That(unityEventLoop.PostUpdateExecutedCount, Is.EqualTo(frameCount));
            
            Assert.That(unityEventLoop.PreLateUpdateExecutedCount, Is.EqualTo(frameCount - 1));
            Assert.That(unityEventLoop.PostLateUpdateExecutedCount, Is.EqualTo(frameCount - 1));
            
            scope.Dispose();
            
            Assert.That(unityEventLoop.Disposed, Is.True);
        }
    }
}
