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
        private readonly IObjectName? objectName;
        private readonly bool dontDestroyOnLoad;
        
        public CreateComponentOnNewGameObject(Type type, IInjection? injection, IAnchor? anchor, IObjectName? objectName, bool dontDestroyOnLoad)
        {
            this.type = type;
            this.injection = injection;
            this.anchor = anchor;
            this.objectName = objectName;
            this.dontDestroyOnLoad = dontDestroyOnLoad;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            var name = objectName?.GetObjectName() ?? type.Name;
            
            var gameObject = new GameObject(name);
            
            gameObject.SetActive(false);

            var component = gameObject.AddComponent(type);

            try
            {
                if (anchor != null)
                {
                    var parent = anchor.GetTransform();

                    gameObject.transform.SetParent(parent, false);
                }
                
                injection?.Inject(resolver, component);
            }
            catch
            {
                UnityEngine.Object.Destroy(gameObject);
                throw;
            }
            
            if (dontDestroyOnLoad)
            {
                UnityEngine.Object.DontDestroyOnLoad(component);
            }

            gameObject.SetActive(true);

            return component;
        }
    }
}
