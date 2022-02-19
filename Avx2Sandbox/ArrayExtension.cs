using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Avx2Sandbox
{
    internal static class ArrayExtension
    {
        internal static bool IsAvx2 = Avx2.IsSupported;

        //internal static void ApplyInPlace(this float[] input, MultiplicationInfo[] multiplierInfos)
        //{
        //    foreach (var multiplierInfo in multiplierInfos)
        //    {
        //        if (input.Length <= multiplierInfo.FromInclusive)
        //        {
        //            continue;
        //        }

        //        var fromInclusive = Math.Max(0, multiplierInfo.FromInclusive);
        //        var toExclusive = Math.Min(input.Length, multiplierInfo.ToExclusive);
        //        var multiplier = multiplierInfo.Multiplier;
        //        for (var i = fromInclusive; i < toExclusive; i++)
        //        {
        //            input[i] *= multiplier;
        //        }
        //    }
        //}

        internal static void ApplyInPlace(this float[] input, MultiplicationInfo[] multiplierInfos)
        {
            foreach (var multiplierInfo in multiplierInfos)
            {
                if (input.Length <= multiplierInfo.FromInclusive)
                {
                    continue;
                }

                var fromInclusive = Math.Max(0, multiplierInfo.FromInclusive);
                var toExclusive = Math.Min(input.Length, multiplierInfo.ToExclusive);
                var multiplier = multiplierInfo.Multiplier;

                if (IsAvx2)
                    input.ApplyInPlaceAvx2(fromInclusive, toExclusive, multiplier);
                else
                    input.ApplyInPlace(fromInclusive, toExclusive, multiplier);
            }
        }

        internal static void ApplyInPlace(this float[] input, int fromInclusive, int toExclusive, float multiplier)
        {
            for (var i = fromInclusive; i < toExclusive; i++)
            {
                input[i] *= multiplier;
            }
        }

        internal static void ApplyInPlaceAvx2(this float[] input, int fromInclusive, int toExclusive, float multiplier)
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
                        Vector256<float> v = Avx2.LoadVector256(ptr + i);
                        Avx2.Store(ptr + i, Avx2.Multiply(m, v));
                    }
                }
            }

            for (; i < toExclusive; i++)
            {
                input[i] *= multiplier;
            }
        }

        internal static void ApplyInPlaceVec(this float[] input, int fromInclusive, int toExclusive, float multiplier)
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
