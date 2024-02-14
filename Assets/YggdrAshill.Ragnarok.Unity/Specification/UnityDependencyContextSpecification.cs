#nullable enable
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

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

            using var parentScope = parentContext.CreateScope();

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

            using var parentScope = parentContext.CreateScope();

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

            using var parentScope = parentContext.CreateScope();

            var component1 = parentScope.Resolver.Resolve<NoDependencyComponent>();
            var component2 = parentScope.Resolver.Resolve<NoDependencyComponent>();

            Assert.That(component1, Is.EqualTo(component2));

            using var childScope = parentScope.CreateChildScope();

            var component3 = childScope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component3, Is.EqualTo(component1));
            Assert.That(component3, Is.EqualTo(component2));
        }
        
        [TestCase(Lifetime.Global)]
        [TestCase(Lifetime.Local)]
        [TestCase(Lifetime.Temporal)]
        public void ShouldResolveComponentOnNewGameObjectUnderTransform(Lifetime lifetime)
        {
            var parent = new GameObject("Parent").transform;

            var context = new UnityDependencyContext();
            
            context.RegisterComponentOnNewGameObject<NoDependencyComponent>(lifetime).Under(parent);

            using var scope = context.CreateScope();

            var component = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component.transform.parent, Is.EqualTo(parent));
        }
        
        [Test]
        public void ShouldResolveComponentInNewPrefabPerRequest()
        {
            var prefab = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();
            
            var parentContext = new UnityDependencyContext();
            
            parentContext.RegisterComponentInNewPrefab(prefab, Lifetime.Temporal);

            using var parentScope = parentContext.CreateScope();

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

            using var parentScope = parentContext.CreateScope();

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

            using var parentScope = parentContext.CreateScope();

            var component1 = parentScope.Resolver.Resolve<NoDependencyComponent>();
            var component2 = parentScope.Resolver.Resolve<NoDependencyComponent>();

            Assert.That(component1, Is.EqualTo(component2));

            using var childScope = parentScope.CreateChildScope();

            var component3 = childScope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component3, Is.EqualTo(component1));
            Assert.That(component3, Is.EqualTo(component2));
        }

        [TestCase(Lifetime.Global)]
        [TestCase(Lifetime.Local)]
        [TestCase(Lifetime.Temporal)]
        public void ShouldResolveComponentInNewPrefabUnderTransform(Lifetime lifetime)
        {
            var parent = new GameObject("Parent").transform;
            var prefab = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();

            var context = new UnityDependencyContext();
            
            context.RegisterComponentInNewPrefab(prefab, lifetime).Under(parent);

            using var scope = context.CreateScope();

            var component = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(component.transform.parent, Is.EqualTo(parent));
        }

        [Test]
        public void ShouldInjectComponent()
        {
            var component = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();
 
            var context = new UnityDependencyContext();
            
            context.RegisterComponent(component);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(resolved, Is.EqualTo(component));
        }
        
        [Test]
        public void ShouldResolveComponentInChildren()
        {
            var component = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();

            var context = new UnityDependencyContext();
            
            context.RegisterComponentInHierarchy<NoDependencyComponent>(component.gameObject, SearchOrder.Children);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(resolved, Is.EqualTo(component));
        }

        [Test]
        public void ShouldResolveComponentInParent()
        {
            var component = new GameObject(nameof(NoDependencyComponent)).AddComponent<NoDependencyComponent>();
 
            var context = new UnityDependencyContext();
            
            context.RegisterComponentInHierarchy<NoDependencyComponent>(component.gameObject, SearchOrder.Parent);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<NoDependencyComponent>();
            
            Assert.That(resolved, Is.EqualTo(component));
        }

        [UnityTest]
        public IEnumerator ShouldResolveEntryPoint()
        {
            var context = new UnityDependencyContext();

            context.RegisterEntryPoint<UnityEventLoop>();

            var scope = context.CreateScope();

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

            var frameCount = new System.Random().Next(1, 60);

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
