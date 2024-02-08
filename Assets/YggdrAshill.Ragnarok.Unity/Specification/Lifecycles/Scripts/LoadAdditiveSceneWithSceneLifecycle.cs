#nullable enable
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class LoadAdditiveSceneWithSceneLifecycle : IInitializable, IDisposable
    {
        private readonly SceneLifecycle lifecycle;
        private readonly string sceneName;

        [Inject]
        public LoadAdditiveSceneWithSceneLifecycle(SceneLifecycle lifecycle, string sceneName)
        {
            this.lifecycle = lifecycle;
            this.sceneName = sceneName;
        }

        private Coroutine? coroutine;

        public void Initialize()
        {
            coroutine = lifecycle.StartCoroutine(ExecuteAsync());
        }

        private IEnumerator ExecuteAsync()
        {
            using (lifecycle.OverrideParentLifecycle())
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }

            coroutine = null;
        }

        public void Dispose()
        {
            if (coroutine != null)
            {
                lifecycle.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
