#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class HelloWorldWithGameObject
    {
        private readonly GameObject instance;

        [Inject]
        public HelloWorldWithGameObject(GameObject instance)
        {
            this.instance = instance;
        }
            
        public void Execute()
        {
            Debug.LogError($"Hello world in {instance}.");
        }
    }
}
