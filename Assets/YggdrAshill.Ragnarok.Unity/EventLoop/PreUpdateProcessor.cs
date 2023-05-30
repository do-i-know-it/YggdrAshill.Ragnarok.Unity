#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class PreUpdateProcessor : UnityEventLoopProcessor
    {
        private readonly IReadOnlyList<IPreUpdatable> preUpdatableList;

        public PreUpdateProcessor(IReadOnlyList<IPreUpdatable> preUpdatableList, UnityEventLoopExceptionHandler? exceptionHandler)
            : base(exceptionHandler)
        {
            this.preUpdatableList = preUpdatableList;
        }

        protected override void Execute()
        {
            for (var index = 0; index < preUpdatableList.Count; index++)
            {
                preUpdatableList[index].PreUpdate();
            }
        }
    }
}
