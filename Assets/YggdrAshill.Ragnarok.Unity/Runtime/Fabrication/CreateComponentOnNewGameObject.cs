#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateComponentOnNewGameObject : IInstantiation
    {
        private readonly Type type;
        private readonly IInjection? injection;
        private readonly IAnchor? anchor;
        private readonly string? objectName;
        private readonly bool dontDestroyOnLoad;
        
        public CreateComponentOnNewGameObject(Type type, IInjection? injection, IAnchor? anchor, string? objectName, bool dontDestroyOnLoad)
        {
            this.type = type;
            this.injection = injection;
            this.anchor = anchor;
            this.objectName = objectName;
            this.dontDestroyOnLoad = dontDestroyOnLoad;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            var name = string.IsNullOrEmpty(objectName) ? type.Name : objectName;
            
            var gameObject = new GameObject(name);
            
            gameObject.SetActive(false);

            var parent = anchor?.GetTransform();

            if (parent != null)
            {
                gameObject.transform.SetParent(parent, false);
            }
            
            var component = gameObject.AddComponent(type);

            try
            {
                injection?.Inject(resolver, component);
            }
            finally
            {
                if (dontDestroyOnLoad)
                {
                    UnityEngine.Object.DontDestroyOnLoad(component);
                }
                
                component.gameObject.SetActive(true);
            }

            return component;
        }
    }
}
