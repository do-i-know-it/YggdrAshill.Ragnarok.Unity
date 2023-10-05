#nullable enable
using System.Collections;
using UnityEngine.SceneManagement;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class LoadAdditiveSceneWithSceneLifecycle
    {
        private readonly SceneLifecycle lifecycle;
        private readonly string sceneName;

        [Inject]
        public LoadAdditiveSceneWithSceneLifecycle(SceneLifecycle lifecycle, string sceneName)
        {
            this.lifecycle = lifecycle;
            this.sceneName = sceneName;
        }

        public void Execute()
        {
            lifecycle.StartCoroutine(ExecuteAsync());
        }

        private IEnumerator ExecuteAsync()
        {
            using (lifecycle.OverrideParentLifecycle())
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }
    }
}
