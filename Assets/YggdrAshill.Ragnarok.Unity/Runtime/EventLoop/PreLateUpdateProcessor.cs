#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class PreLateUpdateProcessor : UnityEventLoopProcessor
    {
        private readonly IReadOnlyList<IPreLateUpdatable> preLateUpdatableList;

        public PreLateUpdateProcessor(IReadOnlyList<IPreLateUpdatable> preLateUpdatableList, UnityEventLoopExceptionHandler? exceptionHandler)
            : base(exceptionHandler)
        {
            this.preLateUpdatableList = preLateUpdatableList;
        }

        protected override void Execute()
        {
            for (var index = 0; index < preLateUpdatableList.Count; index++)
            {
                preLateUpdatableList[index].PreLateUpdate();
            }
        }
    }
}
