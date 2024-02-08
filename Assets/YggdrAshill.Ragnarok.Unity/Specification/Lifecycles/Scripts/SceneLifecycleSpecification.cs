#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class SceneLifecycleSpecification : MonoInstallation
    {
        [SerializeField] private RequestSenderComponent? requestSenderPrefab;
        private RequestSenderComponent RequestSenderPrefab
        {
            get
            {
                if (requestSenderPrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(requestSenderPrefab)} is null.");
                }

                return requestSenderPrefab;
            }
        }
        
        [SerializeField] private ResponseReceiverComponent? responseReceiverPrefab;
        private ResponseReceiverComponent ResponseReceiverPrefab
        {
            get
            {
                if (responseReceiverPrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(responseReceiverPrefab)} is null.");
                }

                return responseReceiverPrefab;
            }
        }
        
        [SerializeField] private ServiceHandlerComponent? serviceHandlerPrefab;
        private ServiceHandlerComponent ServiceHandlerPrefab
        {
            get
            {
                if (serviceHandlerPrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(serviceHandlerPrefab)} is null.");
                }

                return serviceHandlerPrefab;
            }
        }
        
        [SerializeField] private string sceneName = string.Empty;
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterComponentInNewPrefab(RequestSenderPrefab, Lifetime.Global).As<IRequestSender>();
            container.RegisterComponentInNewPrefab(ResponseReceiverPrefab, Lifetime.Global).As<IResponseReceiver>();
            container.RegisterComponentInNewPrefab(ServiceHandlerPrefab, Lifetime.Global).As<IServiceHandler>();
            container.RegisterEntryPoint<LoadAdditiveSceneWithSceneLifecycle>().WithArgument(sceneName);
        }
    }
}
