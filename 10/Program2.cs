using System;
using System.Linq;

public class Program
{
	public static void Main()
	{
		int[] adapters = new int[]{71, 183, 111, 89, 92, 142, 25, 101, 52, 86, 18, 22, 70, 2, 135, 163, 34, 143, 153, 35, 144, 24, 23, 94, 100, 102, 17, 57, 76, 182, 134, 38, 7, 103, 66, 31, 11, 121, 77, 113, 128, 82, 99, 148, 137, 41, 32, 48, 131, 60, 127, 138, 73, 28, 10, 84, 180, 63, 125, 53, 176, 165, 114, 145, 152, 72, 107, 167, 59, 164, 78, 126, 118, 136, 83, 79, 58, 14, 106, 69, 51, 39, 157, 42, 177, 173, 93, 141, 3, 33, 13, 19, 45, 154, 95, 170, 54, 181, 6, 151, 1, 112, 96, 115, 85, 108, 166, 160, 40, 122, 12};
		int outputPort = adapters.Max() + 3;
		adapters = adapters.OrderByDescending(a => a).Concat(Enumerable.Range(0, 1)).ToArray();
		long[] ways = new long[outputPort + 1];
		ways[outputPort] = 1;
		foreach (var adapter in adapters)
		{
			ways[adapter] = ways[adapter + 1] + ways[adapter + 2] + ways[adapter + 3];
		}

		Console.WriteLine(ways[0]);
	}
}