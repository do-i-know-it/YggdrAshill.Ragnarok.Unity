#nullable enable
using System.Collections;
using YggdrAshill.Ragnarok.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YggdrAshill.Ragnarok.Unity.Specification
{
    internal sealed class SceneLifecycleSpecification : MonoInstallation
    {
        private sealed class LoadAdditiveScene
        {
            private readonly SceneLifecycle sceneLifecycle;

            [Inject]
            public LoadAdditiveScene(SceneLifecycle sceneLifecycle)
            {
                this.sceneLifecycle = sceneLifecycle;
            }

            public void Execute()
            {
                sceneLifecycle.StartCoroutine(ExecuteAsync());
                
            }

            private IEnumerator ExecuteAsync()
            {
                using (sceneLifecycle.OverrideParentLifecycle())
                {
                    yield return SceneManager.LoadSceneAsync("SceneLifecycleTarget", LoadSceneMode.Additive);
                }
            }
        }
        
        [SerializeField] private GameObject? target;
        private GameObject Target
        {
            get
            {
                if (target == null)
                {
                    target = transform.gameObject;
                }

                return target;
            }
        }
        
        public override void Install(IContainer container)
        {
            container.RegisterComponent(Target);
            container.Register<LoadAdditiveScene>(Lifetime.Global);
            container.Register(resolver => resolver.Resolve<LoadAdditiveScene>().Execute());
        }
    }
}
