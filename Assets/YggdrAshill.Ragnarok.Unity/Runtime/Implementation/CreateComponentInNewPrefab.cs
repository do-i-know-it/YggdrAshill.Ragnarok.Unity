#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateComponentInNewPrefab : IInstantiation
    {
        private readonly Component prefab;
        private readonly IInjection? injection;
        private readonly IParentTransform parentTransform;
        private readonly bool dontDestroyOnLoad;
        
        public CreateComponentInNewPrefab(Component prefab, IInjection? injection, IParentTransform parentTransform, bool dontDestroyOnLoad)
        {
            this.prefab = prefab;
            this.injection = injection;
            this.parentTransform = parentTransform;
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
                var parent = parentTransform.GetParentTransform();
                
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
