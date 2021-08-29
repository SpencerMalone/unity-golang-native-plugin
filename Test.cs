using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;

public class Test : MonoBehaviour
{
    public struct IntSlice
    {
        public IntPtr data;
        public long len, cap;
        public IntSlice(IntPtr data, long len, long cap)
        {
            this.data = data;
            this.len = len;
            this.cap = cap;
        }
    }

    public struct GoString
    {
        public string msg;
        public long len;
        public GoString(string msg, long len)
        {
            this.msg = msg;
            this.len = len;
        }
    }

    //The string dll should match the name of your plugin
    const string dll = "libgo";

    [DllImport(dll, CharSet = CharSet.Unicode)]
    private static extern GoString Squares(IntSlice vals);

    [DllImport(dll, CharSet = CharSet.Unicode)]
    private static extern GoString SlowSquares(IntSlice vals);

    [DllImport(dll)]
    private static extern int Add(int a, int b);

    void Start()
    {
        UnityEngine.Debug.Log(Add(1, 1).ToString());
        long[] squareInts = { 1, 3, 5, 7, 9 };
        IntPtr data_ptr = Marshal.AllocHGlobal(Buffer.ByteLength(squareInts));
        Marshal.Copy(squareInts, 0, data_ptr, squareInts.Length);
        var nums = new IntSlice(data_ptr, squareInts.Length, squareInts.Length);

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i=0; i <= 10000; i++)
        { 
            Squares(nums);
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log(Squares(nums).msg);
        UnityEngine.Debug.Log("With go func, took " + stopwatch.ElapsedMilliseconds);

        stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i=0; i <= 10000; i++)
        { 
            SlowSquares(nums);
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log(SlowSquares(nums).msg);
        UnityEngine.Debug.Log("Without go func, took " + stopwatch.ElapsedMilliseconds);

        stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i=0; i <= 10000; i++)
        { 
            foreach (var target in squareInts) {
                var mytest = target * target;
                mytest += 1;
            }
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log("native, took " + stopwatch.ElapsedMilliseconds);
    }
}