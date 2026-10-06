using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

public static class NoApiCrash
{
    // 1. Null pointer write
    public static unsafe void NullPointerWrite()
    {
        *(int*)0 = 0;
    }


    // 2. Volatile null pointer write
    public static unsafe void VolatileNullPointerWrite()
    {
        volatile int* p = (int*)0;
        *p = 0;
    }


    // 3. Null pointer read
    public static unsafe void NullPointerRead()
    {
        int* p = (int*)0;
        int x = *p;
    }


    // 4. Null pointer read — short form
    public static unsafe void NullPointerReadShort()
    {
        _ = *(int*)0;
    }


    // 5. Null unmanaged function pointer
    public static unsafe void NullFunctionPointerCall()
    {
        delegate* unmanaged<void> fn =
            (delegate* unmanaged<void>)0;

        fn();
    }


    // 6. Stack overflow
    public static void StackOverflow()
    {
        StackOverflow();
    }


    // 7. Managed heap exhaustion
    public static void OutOfMemory()
    {
        var list = new List<byte[]>();

        while (true)
        {
            list.Add(new byte[1024 * 1024]);
        }
    }


    // 8. Thread exhaustion
    public static void ThreadExhaustion()
    {
        while (true)
        {
            new Thread(() =>
                Thread.Sleep(Timeout.Infinite))
            {
                IsBackground = true
            }.Start();
        }
    }


    // 9. Task exhaustion
    public static void TaskExhaustion()
    {
        while (true)
        {
            _ = Task.Run(static () => { });
        }
    }


    // 10. Large Object Heap exhaustion
    public static void LargeObjectHeapExhaustion()
    {
        var arrays = new List<byte[]>();

        while (true)
        {
            arrays.Add(new byte[100_000]);
        }
    }


    // 11. Pinned memory pressure
    public static void PinnedMemoryPressure()
    {
        var handles = new List<GCHandle>();

        while (true)
        {
            var buffer = new byte[1024 * 1024];

            handles.Add(
                GCHandle.Alloc(buffer, GCHandleType.Pinned));
        }
    }


    // 12. Generic recursion / stack exhaustion
    public static void GenericRecursion<T>()
    {
        GenericRecursion<T>();
    }


    // 13. Finalizer pressure
    public static void FinalizerPressure()
    {
        while (true)
        {
            _ = new Finalizable();
        }
    }


    // 14. GC allocation storm
    public static void GcAllocationStorm()
    {
        while (true)
        {
            for (int i = 0; i < 100_000; i++)
            {
                _ = new byte[1024];
            }
        }
    }

    // Helper class for finalizer pressure
    private sealed class Finalizable
    {
        ~Finalizable()
        {
        }
    }
}