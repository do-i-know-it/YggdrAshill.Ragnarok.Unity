#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ProjectLifecycleSpecification : MonoInstallation
    {
        [SerializeField] private RequestSenderComponent? requestSender;
        private RequestSenderComponent RequestSender
        {
            get
            {
                if (requestSender == null)
                {
                    throw new InvalidOperationException($"{nameof(requestSender)} is null.");
                }

                return requestSender;
            }
        }
        [SerializeField] private ResponseReceiverComponent? responseReceiver;
        private ResponseReceiverComponent ResponseReceiver
        {
            get
            {
                if (responseReceiver == null)
                {
                    throw new InvalidOperationException($"{nameof(responseReceiver)} is null.");
                }

                return responseReceiver;
            }
        }
        [SerializeField] private ServiceHandlerComponent? serviceHandler;
        private ServiceHandlerComponent ServiceHandler
        {
            get
            {
                if (serviceHandler == null)
                {
                    throw new InvalidOperationException($"{nameof(serviceHandler)} is null.");
                }

                return serviceHandler;
            }
        }
        
        [SerializeField] private FieldInjectionServiceComponent? fieldInjectionService;
        private FieldInjectionServiceComponent FieldInjectionService
        {
            get
            {
                if (fieldInjectionService == null)
                {
                    throw new InvalidOperationException($"{nameof(fieldInjectionService)} is null.");
                }

                return fieldInjectionService;
            }
        }
        
        [SerializeField] private PropertyInjectionServiceComponent? propertyInjectionService;
        private PropertyInjectionServiceComponent PropertyInjectionService
        {
            get
            {
                if (propertyInjectionService == null)
                {
                    throw new InvalidOperationException($"{nameof(propertyInjectionService)} is null.");
                }

                return propertyInjectionService;
            }
        }
        
        [SerializeField] private MethodInjectionServiceComponent? methodInjectionService;
        private MethodInjectionServiceComponent MethodInjectionService
        {
            get
            {
                if (methodInjectionService == null)
                {
                    throw new InvalidOperationException($"{nameof(methodInjectionService)} is null.");
                }

                return methodInjectionService;
            }
        }
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterComponent(RequestSender).As<IRequestSender>();
            container.RegisterComponent(ResponseReceiver).As<IResponseReceiver>();
            container.RegisterComponent(ServiceHandler).As<IServiceHandler>();

            container.RegisterEntryPoint<Service>();
            container.RegisterComponent(FieldInjectionService).WithFieldInjection();
            container.RegisterComponent(PropertyInjectionService).WithPropertyInjection();
            container.RegisterComponent(MethodInjectionService).WithMethodInjection();
        }
    }
}
