using System;
using System.Runtime.CompilerServices;

namespace SuperUnsafeCast
{
    class Program
    {
        static void Main(string[] args)
        {
            uint[] intArray = new uint[5];
            intArray[0] = 1;
            intArray[1] = 2;
            intArray[2] = 4;
            intArray[4] = 0xfffffff0;
            byte[] byteArray = Unsafe.As<uint[], byte[]>(ref intArray);

            Console.WriteLine($"byteArray.Length is {byteArray.Length}");
            foreach (var b in byteArray)
            {
                Console.WriteLine(b);
            }

            byte[] byteArray2 = Cast(intArray);

            Console.WriteLine("---");
            Console.WriteLine($"byteArray2.Length is {byteArray2.Length}");
            foreach (var b in byteArray2)
            {
                Console.WriteLine(b);
            }

            var b3 = new byte[20];

            Buffer.BlockCopy(intArray, 0, b3, 0, 20);

            Console.WriteLine("---");

            foreach (var b in b3)
            {
                Console.WriteLine(b);
            }

        }

        private static unsafe byte[] Cast(uint[] array)
        {
            byte[] byteArray = Unsafe.As<uint[], byte[]>(ref array);

            fixed (byte* ptr = &byteArray[0])
            {
                *ptr = 15;
                long* lengthHeader = (long*)(ptr - 8);
                //*lengthHeader = -1;
                *lengthHeader = array.Length * sizeof(uint);
            }

            return byteArray;
        }
    }
}
