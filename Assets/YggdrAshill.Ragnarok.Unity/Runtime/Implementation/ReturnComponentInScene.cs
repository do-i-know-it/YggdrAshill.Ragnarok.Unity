#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnComponentInScene : IInstantiation
    {
        private readonly GameObject instance;
        private readonly Type type;
        private readonly bool includeInactive;
        private readonly IInjection? injection;

        public ReturnComponentInScene(GameObject instance, Type type, bool includeInactive, IInjection? injection)
        {
            this.instance = instance;
            this.type = type;
            this.includeInactive = includeInactive;
            this.injection = injection;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            // TODO: object pooling.
            var buffer = new List<GameObject>();
            
            var scene = instance.scene;
            scene.GetRootGameObjects(buffer);

            foreach (var gameObject in buffer)
            {
                var component = gameObject.GetComponentInChildren(type, includeInactive);

                if (component == null)
                {
                    continue;
                }
                
                injection?.Inject(resolver, component);
                
                return component;
            }

            throw new RagnarokException(type, $"{type} is not in {scene}.");
        }
    }
}
