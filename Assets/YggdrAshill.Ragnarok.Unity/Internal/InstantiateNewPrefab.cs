#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Internal
{
    internal sealed class InstantiateNewPrefab :
        IInstantiation
    {
        private readonly Component prefab;
        private readonly IInjection? injection;
        private readonly IAnchor? anchor;
        private readonly bool dontDestroyOnLoad;
        
        public InstantiateNewPrefab(Component prefab, IInjection? injection, IAnchor? anchor, bool dontDestroyOnLoad)
        {
            this.prefab = prefab;
            this.injection = injection;
            this.anchor = anchor;
            this.dontDestroyOnLoad = dontDestroyOnLoad;
        }

        public object Instantiate(IResolver resolver)
        {
            var wasActive = prefab.gameObject.activeSelf;
            
            if (wasActive)
            {
                prefab.gameObject.SetActive(false);
            }

            var component = Object.Instantiate(prefab);

            var parentTransform = anchor?.GetParentTransform();

            if (parentTransform != null)
            {
                component.transform.SetParent(parentTransform, false);
            }
            
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
                if (wasActive)
                {
                    prefab.gameObject.SetActive(true);
                    component.gameObject.SetActive(true);
                }
            }

            return component;
        }
    }
}
