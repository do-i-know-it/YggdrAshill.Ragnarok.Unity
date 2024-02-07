#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class SenderInstallation : MonoInstallation
    {
        [SerializeField] private GameObject? instance;
        private GameObject Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException($"{nameof(instance)} is null.");
                }

                return instance;
            }
        }
        
        [SerializeField] private SearchOrder order = SearchOrder.Children;

        public override void Install(IObjectContainer container)
        {
            container.RegisterComponentInGameObject<SenderComponent>(Instance, order);
        }
    }
}
