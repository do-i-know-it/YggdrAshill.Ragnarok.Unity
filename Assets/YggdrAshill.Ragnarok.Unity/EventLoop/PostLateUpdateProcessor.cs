#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Unity
{
    internal sealed class PostLateUpdateProcessor : UnityEventLoopProcessor
    {
        private readonly IReadOnlyList<IPostLateUpdatable> postLateUpdatableList;

        public PostLateUpdateProcessor(IReadOnlyList<IPostLateUpdatable> postLateUpdatableList, UnityEventLoopExceptionHandler? exceptionHandler)
            : base(exceptionHandler)
        {
            this.postLateUpdatableList = postLateUpdatableList;
        }

        protected override void Execute()
        {
            for (var index = 0; index < postLateUpdatableList.Count; index++)
            {
                postLateUpdatableList[index].PostLateUpdate();
            }
        }
    }
}
