#nullable enable
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
            this(ExpressionToOperate.Instance)
#endif            
        {
            
        }

        private UnityDependencyContext(IOperation operation)
        {
            context = new DependencyContext(operation);
        }

        /// <inheritdoc/>
        public IObjectResolver Resolver => context.Resolver;

        /// <inheritdoc/>
        public ICompilation Compilation => context.Compilation;

        /// <inheritdoc/>
        public IRegistration Registration => context.Registration;

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
    }
}
