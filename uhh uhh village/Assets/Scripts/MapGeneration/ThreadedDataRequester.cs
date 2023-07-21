using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    ///  This class is used to request data from a thread. 
    /// </summary>
    public class ThreadedDataRequester : MonoBehaviour
    {
        private static  ThreadedDataRequester instance;
        private readonly Queue<ThreadInfo> dataQueue = new Queue<ThreadInfo >();

        private void Awake()
        {
            instance = FindObjectOfType<ThreadedDataRequester>(); 
        }

        public static void RequestData(Func<object> generateData, Action<object> callback)
        {
            ThreadStart threadStart = delegate { instance.DataThread(generateData, callback); };

            new Thread(threadStart).Start();
        }

        private void DataThread(Func<object> generateData, Action<object> callback)
        {
            object data = generateData();
            lock (dataQueue)
            {
                dataQueue.Enqueue(new ThreadInfo(callback, data));
            }
        }

        private void Update()
        {
            lock (dataQueue)
            {
                if (dataQueue.Count <= 0) return;
                
                while (dataQueue.TryDequeue(out var item))
                {
                    item.callback(item.parameter);
                }
            }
        }

        /// <summary>
        ///  This struct is used to store the callback and the parameter of the callback.
        /// </summary>
        private struct ThreadInfo
        {
            public readonly Action<object> callback;
            public readonly object parameter;

            public ThreadInfo(Action<object> callback, object parameter)
            {
                this.callback = callback;
                this.parameter = parameter;
            }
        }

    }
}