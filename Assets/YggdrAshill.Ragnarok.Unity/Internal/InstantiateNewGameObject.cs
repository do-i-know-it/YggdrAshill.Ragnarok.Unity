#nullable enable
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YggdrAshill.Ragnarok.Unity.Internal
{
    internal sealed class InstantiateNewGameObject :
        IInstantiation
    {
        private readonly Type componentType;
        private readonly IInjection? injection;
        private readonly string? objectName;
        private readonly IAnchor? anchor;
        private readonly bool dontDestroyOnLoad;
        
        public InstantiateNewGameObject(Type componentType, IInjection? injection, string? objectName, IAnchor? anchor, bool dontDestroyOnLoad)
        {
            this.componentType = componentType;
            this.injection = injection;
            this.objectName = objectName;
            this.anchor = anchor;
            this.dontDestroyOnLoad = dontDestroyOnLoad;
        }
        
        public object Instantiate(IResolver resolver)
        {
            var name = string.IsNullOrEmpty(objectName) ? componentType.Name : objectName;
            
            var gameObject = new GameObject(name);
            
            gameObject.SetActive(false);

            var parentTransform = anchor?.GetParentTransform();

            if (parentTransform != null)
            {
                gameObject.transform.SetParent(parentTransform, false);
            }
            
            var component = gameObject.AddComponent(componentType);

            try
            {
                injection?.Inject(resolver, component);
                
                if (dontDestroyOnLoad)
                {
                    Object.DontDestroyOnLoad(component);
                }
            }
            finally
            {
                component.gameObject.SetActive(true);
            }

            return component;
        }
    }
}
