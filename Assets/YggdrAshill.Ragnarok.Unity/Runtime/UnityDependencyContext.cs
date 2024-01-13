#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add diagnostics.
    /// <summary>
    /// Implementation of <see cref="IObjectContext"/> using <see cref="DependencyContext"/>.
    /// </summary>
    public sealed class UnityDependencyContext : IObjectContext
    {
        private readonly DependencyContext context;

        /// <summary>
        /// Create default <see cref="UnityDependencyContext"/>.
        /// </summary>
        public UnityDependencyContext() :
#if UNITY_IOS
            this(ReflectionSolver.Instance)
#else
            this(ExpressionSolver.Instance)
#endif            
        {
            
        }

        private UnityDependencyContext(ISolver solver)
        {
            context = new DependencyContext(solver);
        }

        /// <inheritdoc/>
        public IObjectResolver Resolver => context.Resolver;

        /// <inheritdoc/>
        public ICompilation Compilation => context.Compilation;

        /// <inheritdoc/>
        public IObjectContext CreateContext()
        {
            return context.CreateContext();
        }
        
        /// <inheritdoc/>
        public IObjectScope CreateScope()
        {
            return context.CreateScope();
        }

        /// <inheritdoc/>
        public int Count(IStatementSelection selection)
        {
            return context.Count(selection);
        }

        /// <inheritdoc/>
        public void Register(IStatement statement)
        {
            context.Register(statement);
        }

        /// <inheritdoc/>
        public void Register(IOperation operation)
        {
            context.Register(operation);
        }

        /// <inheritdoc/>
        public void Register(IDisposable disposable)
        {
            context.Register(disposable);
        }
    }
}
