#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateComponentInNewPrefab : IInstantiation
    {
        private readonly Component prefab;
        private readonly IInjection? injection;
        private readonly IAnchor? anchor;
        private readonly bool dontDestroyOnLoad;
        
        public CreateComponentInNewPrefab(Component prefab, IInjection? injection, IAnchor? anchor, bool dontDestroyOnLoad)
        {
            this.prefab = prefab;
            this.injection = injection;
            this.anchor = anchor;
            this.dontDestroyOnLoad = dontDestroyOnLoad;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            var wasActive = prefab.gameObject.activeSelf;
            
            if (wasActive)
            {
                prefab.gameObject.SetActive(false);
            }

            var component = Object.Instantiate(prefab);

            var parent = anchor?.GetTransform();

            if (parent != null)
            {
                component.transform.SetParent(parent, false);
            }
            
            try
            {
                injection?.Inject(resolver, component);
            }
            finally
            {
                if (wasActive)
                {
                    prefab.gameObject.SetActive(true);
                    component.gameObject.SetActive(true);
                }
            }
            
            if (dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(component);
            }

            return component;
        }
    }
}
