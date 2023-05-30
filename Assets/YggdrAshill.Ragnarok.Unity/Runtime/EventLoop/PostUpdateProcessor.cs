#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class PostUpdateProcessor : UnityEventLoopProcessor
    {
        private readonly IReadOnlyList<IPostUpdatable> postUpdatableList;

        public PostUpdateProcessor(IReadOnlyList<IPostUpdatable> postUpdatableList, UnityEventLoopExceptionHandler? exceptionHandler)
            : base(exceptionHandler)
        {
            this.postUpdatableList = postUpdatableList;
        }

        protected override void Execute()
        {
            for (var index = 0; index < postUpdatableList.Count; index++)
            {
                postUpdatableList[index].PostUpdate();
            }
        }
    }
}
