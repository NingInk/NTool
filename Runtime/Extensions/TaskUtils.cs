using System;
using System.Threading;
using UnityEngine;

namespace NTool.Extensions
{
    // 定义一个静态类，用于处理Task相关的操作
    public static class TaskUtils
    {
        // 定义一个静态只读对象，用于线程同步
        private static readonly object Lock = new object();

        // 取消CancellationTokenSource
        static void Cancel(ref CancellationTokenSource cancellation)
        {
            // 如果cancellation不为空且没有被请求取消
            if (cancellation == null || cancellation.IsCancellationRequested) return;
            try
            {
                // 取消cancellation
                cancellation.Cancel();
                // 释放cancellation
                cancellation.Dispose();
            }
            // 捕获异常
            catch (Exception ex)
            {
                // 打印异常
                Debug.LogException(ex);
            }
        }

        // 安全创建CancellationTokenSource
        public static void SafeCreate(ref CancellationTokenSource cancellation)
        {
            // 锁定线程
            lock (Lock)
            {
                // 取消cancellation
                Cancel(ref cancellation);
                // 创建新的cancellation
                cancellation = new CancellationTokenSource();
            }
        }

        // 安全取消CancellationTokenSource
        public static void SafeCancel(ref CancellationTokenSource cancellation)
        {
            // 锁定线程
            lock (Lock)
            {
                // 取消cancellation
                Cancel(ref cancellation);
                // 将cancellation置为空
                cancellation = null;
            }
        }

        public static CancellationTokenSource Timer(float seconds)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(seconds));
            return cts;
        }
    }
}