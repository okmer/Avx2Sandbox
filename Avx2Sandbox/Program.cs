using Avx2Sandbox;
using System.Diagnostics;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, AVX2!");

int cycleSize = 100;
int[] dataSizes = new int[]{ 10, 100, 1000, 10000, 100000, 1000000, 2000000, 5000000, 10000000 };


//ApplyInPlace/ApplyInPlaceAvx2/ApplyInPlaceVec
Console.WriteLine("ApplyInPlace/ApplyInPlaceAvx2/ApplyInPlaceVec");
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

        a.ApplyInPlace(0, dataSize, 1.111f);

        sw.Stop();
    }

    Console.WriteLine($"Native {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    var b = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        b = input.ToArray();

        sw.Start();

        b.ApplyInPlaceAvx2(0, dataSize, 1.111f);

        sw.Stop();
    }

    Console.WriteLine($"Avx2 {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    var c = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        c = input.ToArray();

        sw.Start();

        c.ApplyInPlaceVec(0, dataSize, 1.111f);

        sw.Stop();
    }

    Console.WriteLine($"Vector {cycleSize}x ize[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    Console.WriteLine($"Native and AvX2 are equal: {a.SequenceEqual(b)}, Native and Vector are equal: {a.SequenceEqual(c)}");
    
}

Console.Write("Press Enter...");
Console.ReadLine();

//InvertInPlace/InvertInPlaceAvx2/InvertInPlaceVec
Console.WriteLine("InvertInPlace/InvertInPlaceAvx2/InvertInPlaceVec");
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

    Console.WriteLine($"Avx2 {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

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

    Console.WriteLine($"Native and AvX2 are equal: {a.SequenceEqual(b)}, Native and Vector are equal: {a.SequenceEqual(c)}");
}

//RoundToInt32/RoundToInt32Avx2
Console.WriteLine("RoundToInt32/RoundToInt32Avx2");
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

        a = d.RoundToInt32();

        sw.Stop();
    }

    Console.WriteLine($"Native {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    int[] b = Array.Empty<int>();
    for (int i = 0; i < cycleSize; i++)
    {
        var d = input.ToArray();

        sw.Start();

        b = d.RoundToInt32Avx2();

        sw.Stop();
    }

    Console.WriteLine($"Avx2 {cycleSize}x size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    Console.WriteLine($"Native and AvX2 are equal: {a.SequenceEqual(b)}");
}

Console.Write("Press Enter...");
Console.ReadLine();




