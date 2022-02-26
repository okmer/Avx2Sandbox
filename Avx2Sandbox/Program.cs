using Avx2Sandbox;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, AVX2!");

//Overview
Console.WriteLine($"Supported SSE {Sse.IsSupported}, SSE2 {Sse2.IsSupported}, SSE3, {Sse3.IsSupported}, SSE41 {Sse41.IsSupported}, SSE42 {Sse42.IsSupported}, SSSE3 {Ssse3.IsSupported}");
Console.WriteLine($"Supported AVX {Avx.IsSupported}, AVX2 {Avx2.IsSupported}, BMI1 {Bmi1.IsSupported}, BMI2 {Bmi2.IsSupported}, FMA {Fma.IsSupported}");

int cycleSize = 100;
int[] dataSizes = new int[]{ 10, 100, 1000, 10000, 100000, 1000000, 2000000, 5000000, 10000000 };

//MultiplyInPlace
Console.WriteLine("MultiplyInPlace: Native/AVX/Vector");
foreach (int dataSize in dataSizes)
{
    var input = new float[dataSize];

    for (int i = 0; i < dataSize; i++)
    {
        input[i] = Random.Shared.NextSingle();
    }

    var sw = new Stopwatch();

    var a = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        a = input.ToArray();

        sw.Start();

        a.MultiplyInPlace(0, dataSize, 1.111f);

        sw.Stop();
    }

    Console.WriteLine($"Native {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    var b = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        b = input.ToArray();

        sw.Start();

        b.MultiplyInPlaceAvx(0, dataSize, 1.111f);

        sw.Stop();
    }

    Console.WriteLine($"AVX {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    var c = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        c = input.ToArray();

        sw.Start();

        c.MultiplyInPlaceVec(0, dataSize, 1.111f);

        sw.Stop();
    }

    Console.WriteLine($"Vector {cycleSize}x ize[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    Console.WriteLine($"Native and AVX sequences are equal: {a.SequenceEqual(b)}, Native and Vector sequences are equal: {a.SequenceEqual(c)}");
    
}

Console.Write("Press Enter...");
Console.ReadLine();

//InvertInPlace
Console.WriteLine("InvertInPlace Native/AVX2/Vector");
foreach (int dataSize in dataSizes)
{
    var input = new int[dataSize];

    for (int i = 0; i < dataSize; i++)
    {
        input[i] = Random.Shared.Next();
    }

    var sw = new Stopwatch();

    var a = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        a = input.ToArray();

        sw.Start();

        a.InvertInPlace();

        sw.Stop();
    }

    Console.WriteLine($"Native {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    var b = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        b = input.ToArray();

        sw.Start();

        b.InvertInPlaceAvx2();

        sw.Stop();
    }

    Console.WriteLine($"AVX2 {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    var c = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        c = input.ToArray();

        sw.Start();

        c.InvertInPlaceVec();

        sw.Stop();
    }

    Console.WriteLine($"Vector {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    Console.WriteLine($"Native and AVX2 sequences are equal: {a.SequenceEqual(b)}, Native and Vector sequences are equal: {a.SequenceEqual(c)}");
}

Console.Write("Press Enter...");
Console.ReadLine();

//RoundAndConvertToInt32
Console.WriteLine("RoundAndConvertToInt32: Native/AVX/SSE2");
foreach (int dataSize in dataSizes)
{
    var input = new float[dataSize];

    for (int i = 0; i < dataSize; i++)
    {
        input[i] = Random.Shared.NextSingle();
    }

    var sw = new Stopwatch();

    var a = Array.Empty<int>();
    for (int i = 0; i < cycleSize; i++)
    {
        var d = input.ToArray();

        sw.Start();

        a = d.RoundAndConvertToInt32();

        sw.Stop();
    }

    Console.WriteLine($"Native {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    int[] b = Array.Empty<int>();
    for (int i = 0; i < cycleSize; i++)
    {
        var d = input.ToArray();

        sw.Start();

        b = d.RoundAndConvertToInt32Sse2();

        sw.Stop();
    }

    Console.WriteLine($"SSE2 {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    int[] c = Array.Empty<int>();
    for (int i = 0; i < cycleSize; i++)
    {
        var d = input.ToArray();

        sw.Start();

        c = d.RoundAndConvertToInt32Avx();

        sw.Stop();
    }

    Console.WriteLine($"AVX {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    Console.WriteLine($"Native and SSE2 sequences are equal: {a.SequenceEqual(b)}, Native and AVX sequences are equal: {a.SequenceEqual(c)}");
}

Console.Write("Press Enter...");
Console.ReadLine();




