#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class PostFixedUpdateProcessor : UnityEventLoopProcessor
    {
        private readonly IReadOnlyList<IPostFixedUpdatable> postFixedUpdatableList;

        public PostFixedUpdateProcessor(IReadOnlyList<IPostFixedUpdatable> postFixedUpdatableList, UnityEventLoopExceptionHandler? exceptionHandler)
            : base(exceptionHandler)
        {
            this.postFixedUpdatableList = postFixedUpdatableList;
        }

        protected override void Execute()
        {
            for (var index = 0; index < postFixedUpdatableList.Count; index++)
            {
                postFixedUpdatableList[index].PostFixedUpdate();
            }
        }
    }
}
