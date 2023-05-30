#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class PreFixedUpdateProcessor : UnityEventLoopProcessor
    {
        private readonly IReadOnlyList<IPreFixedUpdatable> preFixedUpdatableList;

        public PreFixedUpdateProcessor(IReadOnlyList<IPreFixedUpdatable> preFixedUpdatableList, UnityEventLoopExceptionHandler? exceptionHandler)
            : base(exceptionHandler)
        {
            this.preFixedUpdatableList = preFixedUpdatableList;
        }

        protected override void Execute()
        {
            for (var index = 0; index < preFixedUpdatableList.Count; index++)
            {
                preFixedUpdatableList[index].PreFixedUpdate();
            }
        }
    }
}
