using Avx2Sandbox;
using System.Diagnostics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, AVX2!");

int cycleSize = 100;
int[] dataSizes = new int[]{ 10, 100, 1000, 10000, 100000, 1000000, 2000000, 5000000, 10000000 };


//ApplyInPlace/ApplyInPlaceAvx2
Console.WriteLine("ApplyInPlace/ApplyInPlaceAvx2");
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

    Console.WriteLine($"Native size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    var b = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        b = input.ToArray();

        sw.Start();

        b.ApplyInPlaceAvx2(0, dataSize, 1.111f);

        sw.Stop();
    }

    Console.WriteLine($"Avx2 size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    Console.WriteLine($"Native and AvX2 are equal: {a.SequenceEqual(b)}");
    
}

Console.ReadLine();

//InvertInPlace/InvertInPlaceAvx2
Console.WriteLine("InvertInPlace/InvertInPlaceAvx2");
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

    Console.WriteLine($"Native size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    sw.Reset();

    var b = input.ToArray();
    for (int i = 0; i < cycleSize; i++)
    {
        b = input.ToArray();

        sw.Start();

        b.InvertInPlaceAvx2();

        sw.Stop();
    }

    Console.WriteLine($"Avx2 size[{dataSize}] {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} mS");

    Console.WriteLine($"Native and AvX2 are equal: {a.SequenceEqual(b)}");

}

Console.ReadLine();




