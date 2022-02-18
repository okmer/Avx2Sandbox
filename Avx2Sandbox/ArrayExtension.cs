using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Avx2Sandbox
{
    internal static class ArrayExtension
    {
        internal static bool IsAvx2 = Avx2.IsSupported;

        internal static void ApplyInPlace_(this float[] input, MultiplicationInfo[] multiplierInfos)
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
                for (var i = fromInclusive; i < toExclusive; i++)
                {
                    input[i] *= multiplier;
                }
            }
        }

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

        internal static void InvertInPlace(this int[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = -data[i];
            }
        }

        internal static void InvertInPlaceAvx2(this int[] data)
        {
            var zero = Vector256<int>.Zero;

            int i = 0;
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
