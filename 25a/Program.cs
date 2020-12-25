using System;

namespace _25a
{
    class Program
    {
        static void Main(string[] args)
        {
            var cardPublicKey = 12232269;
            var doorPublicKey = 19452773;

            var subjectNumber = 7;

            var loopSizes = FindLoopSizes(subjectNumber, 10_000_000, cardPublicKey, doorPublicKey);

            var encryptionKey = Transform(doorPublicKey, loopSizes.Card);
            var encryptionKey2 = Transform(cardPublicKey, loopSizes.Door);
            Console.WriteLine(encryptionKey);
            Console.WriteLine(encryptionKey2);
        }

        static (long Card, long Door) FindLoopSizes(int subjectNumber, long maxLoopSize, int cardPublicKey, int doorPublicKey)
        {
            long value = 1;

            var publicKeyFound = false;
            long doorLoopSize = 0;
            long cardLoopSize = 0;

            for (long i = 0; i < maxLoopSize; i++)
            {
                value *= subjectNumber;
                value %= 20201227;

                if (value == cardPublicKey)
                {
                    Console.WriteLine($"Card loop size: {i + 1}");
                    cardLoopSize = i + 1;
                    if (publicKeyFound)
                        return (cardLoopSize, doorLoopSize);
                    publicKeyFound = true;
                }
                if (value == doorPublicKey)
                {
                    Console.WriteLine($"Door loop size: {i + 1}");
                    doorLoopSize = i + 1;
                    if (publicKeyFound)
                        return (cardLoopSize, doorLoopSize);
                    publicKeyFound = true;
                }
            }

            throw new Exception("Too low max loop size");
        }

        static long Transform(long subjectNumber, long loopSize)
        {
            long value = 1;

            for (long i = 0; i < loopSize; i++)
            {
                value *= subjectNumber;
                value %= 20201227;
            }

            return value;
        }
    }
}
