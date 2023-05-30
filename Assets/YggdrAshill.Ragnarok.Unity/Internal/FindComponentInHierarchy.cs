#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Internal
{
    internal sealed class FindComponentInHierarchy :
        IInstantiation
    {
        private readonly Type componentType;
        private readonly IInjection? injection;
        
        public FindComponentInHierarchy(Type componentType, IInjection? injection)
        {
            this.componentType = componentType;
            this.injection = injection;
        }
        
        public object Instantiate(IResolver resolver)
        {
            var lifecycle = resolver.Resolve<Lifecycle>();

            var scene = lifecycle.gameObject.scene;

            // TODO: object pooling.
            var buffer = new List<GameObject>();
            
            scene.GetRootGameObjects(buffer);
            
            foreach (var gameObject in buffer)
            {
                var component = gameObject.GetComponentInChildren(componentType, true);
            
                if (component != null)
                {
                    injection?.Inject(resolver, component);
                    
                    return component;
                }
            }
            
            throw new RagnarokException(componentType, $"{componentType} is not in {scene}.");
        }
    }
}
