#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity
{
    internal sealed class UnityEventLoopRunner
    {
        private readonly Queue<IUnityEventLoopProcessor> runningQueue = new Queue<IUnityEventLoopProcessor>();
        private readonly Queue<IUnityEventLoopProcessor> waitingQueue = new Queue<IUnityEventLoopProcessor>();

        private readonly object runningGate = new object();
        private readonly object waitingGate = new object();

        private int running;

        public void Dispatch(IUnityEventLoopProcessor item)
        {
            if (Interlocked.CompareExchange(ref running, 1, 1) == 1)
            {
                lock (waitingGate)
                {
                    waitingQueue.Enqueue(item);
                }
            }
            else
            {
                lock (runningGate)
                {
                    runningQueue.Enqueue(item);
                }   
            }
        }

        public void Run()
        {
            Interlocked.Exchange(ref running, 1);

            lock (runningGate)
            lock (waitingGate)
            {
                while (waitingQueue.Count > 0)
                {
                    var waiting = waitingQueue.Dequeue();
                    runningQueue.Enqueue(waiting);
                }
            }

            while (true)
            {
                IUnityEventLoopProcessor? item;
                lock (runningGate)
                {
                    item = runningQueue.Count > 0 ? runningQueue.Dequeue() : null;
                }

                if (item == null)
                {
                    break;
                }

                var continuous = false;
                try
                {
                    continuous = item.Process();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }

                if (!continuous)
                {
                    continue;
                }

                lock (waitingGate)
                {
                    waitingQueue.Enqueue(item);
                }
            }

            Interlocked.Exchange(ref running, 0);
        }
    }
}
