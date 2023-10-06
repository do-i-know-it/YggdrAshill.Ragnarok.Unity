#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateComponentOnNewGameObject : IInstantiation
    {
        private readonly Type type;
        private readonly IInjection? injection;
        private readonly IAnchorTransform? anchorTransform;
        private readonly IObjectName? objectName;
        private readonly bool dontDestroyOnLoad;
        
        public CreateComponentOnNewGameObject(Type type, IInjection? injection, IAnchorTransform? anchorTransform, IObjectName? objectName, bool dontDestroyOnLoad)
        {
            this.type = type;
            this.injection = injection;
            this.anchorTransform = anchorTransform;
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
                var parent = anchorTransform?.GetAnchorTransform();
                
                if (parent != null)
                {
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
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }

            gameObject.SetActive(true);

            return component;
        }
    }
}
