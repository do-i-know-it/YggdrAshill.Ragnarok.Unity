#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateComponentInNewPrefab : IInstantiation
    {
        private readonly Component prefab;
        private readonly IInjection? injection;
        private readonly IAnchorTransform? anchorTransform;
        private readonly bool dontDestroyOnLoad;
        
        public CreateComponentInNewPrefab(Component prefab, IInjection? injection, IAnchorTransform? anchorTransform, bool dontDestroyOnLoad)
        {
            this.prefab = prefab;
            this.injection = injection;
            this.anchorTransform = anchorTransform;
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

            try
            {
                var parent = anchorTransform?.GetAnchorTransform();
                
                if (parent != null)
                {
                    component.transform.SetParent(parent, false);
                }
                
                injection?.Inject(resolver, component);
            }
            catch
            {
                Object.Destroy(component.gameObject);
                throw;
            }
            
            if (dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(component.gameObject);
            }
            
            if (wasActive)
            {
                prefab.gameObject.SetActive(true);
                component.gameObject.SetActive(true);
            }

            return component;
        }
    }
}
