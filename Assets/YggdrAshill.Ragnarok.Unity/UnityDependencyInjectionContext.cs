#nullable enable
using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Unity
{
    // TODO: add diagnostics.
    // TODO: add document comments.
    /// <summary>
    /// Implementation of <see cref="IContext"/> using <see cref="DependencyInjectionContext"/>.
    /// </summary>
    public sealed class UnityDependencyInjectionContext :
        IContext
    {
        private readonly DependencyInjectionContext context;

        public UnityDependencyInjectionContext() :
#if UNITY_IOS
            this(ReflectionSolver.Instance)
#else
            this(ExpressionSolver.Instance)
#endif            
        {
            
        }

        private UnityDependencyInjectionContext(ISolver solver)
        {
            context = new DependencyInjectionContext(solver);
        }
        
        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetInstantiation(type, parameterList);
        }

        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetFieldInjection(type, parameterList);
        }

        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetPropertyInjection(type, parameterList);
        }

        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetMethodInjection(type, parameterList);
        }

        public void Register(IComposition composition)
        {
            context.Register(composition);
        }

        public void Register(Action<IResolver> callback)
        {
            context.Register(callback);
        }

        public IScope Build()
        {
            return context.Build();
        }
    }
}
