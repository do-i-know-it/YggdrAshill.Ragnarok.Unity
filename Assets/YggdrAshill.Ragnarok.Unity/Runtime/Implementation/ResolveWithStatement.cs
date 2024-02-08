#nullable enable
namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveWithStatement : IInstruction
    {
        private readonly IStatement statement;

        public ResolveWithStatement(IStatement statement)
        {
            this.statement = statement;
        }
        
        public void Execute(IObjectResolver resolver)
        {
            
            var assignedTypeList = statement.AssignedTypeList;

            var targetType = assignedTypeList.Count != 0 ? assignedTypeList[0] : statement.ImplementedType;
            
            resolver.Resolve(targetType);
        }
    }
}
