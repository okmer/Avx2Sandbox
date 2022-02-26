using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Avx2Sandbox
{
    internal static class ArrayExtension
    {
        internal static bool IsAvx2 = Avx2.IsSupported;

        internal static void MultiplyInPlace(this float[] input, int fromInclusive, int toExclusive, float multiplier)
        {
            for (var i = fromInclusive; i < toExclusive; i++)
            {
                input[i] *= multiplier;
            }
        }

        internal static void MultiplyInPlaceAvx(this float[] input, int fromInclusive, int toExclusive, float multiplier)
        {
            int i = fromInclusive;

            unsafe
            {
                Vector256<float> m = Vector256.Create(multiplier);
                fixed (float* ptr = input)
                {
                    var vectorCount = Vector256<float>.Count;
                    for (; i < toExclusive - vectorCount; i += vectorCount)
                    {
                        Vector256<float> v = Avx.LoadVector256(ptr + i);
                        Avx.Store(ptr + i, Avx.Multiply(m, v));
                    }
                }
            }

            for (; i < toExclusive; i++)
            {
                input[i] *= multiplier;
            }
        }

        internal static void MultiplyInPlaceVec(this float[] input, int fromInclusive, int toExclusive, float multiplier)
        {
            int i = fromInclusive;

            var m = new Vector<float>(multiplier);
            var vectorCount = Vector<float>.Count;
            for (; i < toExclusive - vectorCount; i += vectorCount)
            {
                var v = new Vector<float>(input, i);
                (m * v).CopyTo(input, i);
            }

            for (; i < toExclusive; i++)
            {
                input[i] *= multiplier;
            }
        }

        internal static void InvertInPlace(this int[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = -data[i];
            }
        }

        internal static void InvertInPlaceAvx2(this int[] data)
        {
            int i = 0;

            var zero = Vector256<int>.Zero;
            int dataCount = data.Length;
            unsafe
            {
                fixed (int* ptr = data)
                {
                    var vectorCount = Vector256<int>.Count;
                    for (; i < dataCount - vectorCount; i += vectorCount)
                    {
                        Vector256<int> v = Avx2.LoadVector256(ptr + i);
                        Avx2.Store(ptr + i, Avx2.Subtract(zero, v));
                    }
                }
            }

            for (; i < dataCount; i++)
            {
                data[i] = -data[i];
            }
        }

        internal static void InvertInPlaceVec(this int[] data)
        {
            int i = 0;

            var zero = Vector<int>.Zero;
            int dataCount = data.Length;
            var vectorCount = Vector<int>.Count;
            for (; i < dataCount - vectorCount; i += vectorCount)
            {
                var v = new Vector<int>(data, i);
                (zero - v).CopyTo(data, i);
            }

            for (; i < dataCount; i++)
            {
                data[i] = -data[i];
            }
        }

        internal static int[] RoundAndConvertToInt32(this float[] input)
        {
            var output = new int[input.Length];
            for (int i=0; i < input.Length; i++)
            {
                output[i] = (int)Math.Round(input[i]);
            }
            return output;
        }

        internal static int[] RoundAndConvertToInt32Avx(this float[] input)
        {
            int i = 0;

            int inputCount = input.Length;
            var output = new int[inputCount];
            unsafe
            {
                fixed (float* ptrIn = input)
                {
                    fixed (int* ptrOut = output)
                    {
                        var vectorCount = Vector256<float>.Count;
                        for (; i < inputCount - vectorCount; i += vectorCount)
                        {
                            Vector256<float> v = Avx.LoadVector256(ptrIn + i);
                            Avx.Store(ptrOut + i, Avx.ConvertToVector256Int32(v));
                        }
                    }
                }
            }

            for (; i < inputCount; i++)
            {
                output[i] = (int)Math.Round(input[i]);
            }

            return output;
        }
        
        internal static int[] RoundAndConvertToInt32Sse2(this float[] input)
        {
            int i = 0;

            int inputCount = input.Length;
            var output = new int[inputCount];
            unsafe
            {
                fixed (float* ptrIn = input)
                {
                    fixed (int* ptrOut = output)
                    {
                        var vectorCount = Vector128<float>.Count;
                        for (; i < inputCount - vectorCount; i += vectorCount)
                        {
                            Vector128<float> v = Sse2.LoadVector128(ptrIn + i);
                            Sse2.Store(ptrOut + i, Sse2.ConvertToVector128Int32(v));
                        }
                    }
                }
            }

            for (; i < inputCount; i++)
            {
                output[i] = (int)Math.Round(input[i]);
            }

            return output;
        }
    }

    internal struct MultiplicationInfo
    {
        internal int FromInclusive { get; }
        internal int ToExclusive { get; }
        internal float Multiplier { get; }

        internal MultiplicationInfo(int fromInclusive, int toExclusive, float multiplier)
        {
            FromInclusive = fromInclusive;
            ToExclusive = toExclusive;
            Multiplier = multiplier;
        }
    }
}
