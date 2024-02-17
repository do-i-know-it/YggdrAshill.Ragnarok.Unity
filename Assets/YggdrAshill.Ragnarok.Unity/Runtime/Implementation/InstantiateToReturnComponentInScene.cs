#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToReturnComponentInScene : IInstantiation
    {
        private readonly GameObject instance;
        private readonly Type type;
        private readonly bool includeInactive;

        public InstantiateToReturnComponentInScene(GameObject instance, Type type, bool includeInactive)
        {
            this.instance = instance;
            this.type = type;
            this.includeInactive = includeInactive;
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

                if (component != null)
                {
                    return component;
                }
            }

            throw new RagnarokException(type, $"{type} is not in {scene}.");
        }
    }
}
