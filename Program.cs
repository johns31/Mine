using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UIDGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("started...");

            //Test2();
            int inputId = GetUnique(1, 2);
            string code = GetEncode(inputId);
            Console.WriteLine("Input : " + inputId);
            Console.WriteLine("Encoded value : " + code);
            var input = GetDecode(code);
            Console.WriteLine("Decoded value : " + input);
            Console.WriteLine("End !!!");
            Console.ReadLine();
        }

        private static void Test1()
        {
            String[] codes = new string[10000];

            UniqueIdGenerator uniqueIdGenerator = new UniqueIdGenerator();
            for (int i = 0; i < 10000; i++)
            {
                var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                string input = string.Concat(i, now).Replace(" ", "");

                codes[i] = uniqueIdGenerator.GetUniqueIdentifier(input);
                //codes[i] = uniqueIdGenerator.GetGuid(i);
                //codes[i] = uniqueIdGenerator.GetBase62UniqueIdentifier("Rajesh John" + i, "31/05/1980");
            }

            for (int i = 0; i < codes.Length; i++)
            {
                for (int j = i + 1; j < codes.Length; j++)
                {
                    if (codes[i] == codes[j])
                    {
                        Console.WriteLine("Duplicates !!!!!");
                        Console.WriteLine("item on " + i + " : " + codes[i]);
                        Console.WriteLine("item on " + j + " : " + codes[j]);
                    }
                }
                // Console.WriteLine("code :" + codes[i]);
            }
        }

        private static void Test2()
        {
            var codes = new List<Tuple<string, int, int>> ();

            foreach (var pair in PayoutData.GetData())
            {
                var code = Encoder.Conceal(GetUnique(pair.Key,pair.Value));
                codes.Add(new Tuple<string, int, int>(code,pair.Key,pair.Value));
            }

            for (int i = 0; i < codes.Count; i++)
            {
                for (int j = i + 1; j < codes.Count; j++)
                {
                    if (codes[i].Item1 == codes[j].Item1)
                    {
                        Console.WriteLine("Duplicates !!!!!");
                        Console.WriteLine("item on " + i + " : " + codes[i]);
                        Console.WriteLine("item on " + j + " : " + codes[j]);

                        Console.WriteLine($"Duplicate Code {codes[i].Item1}, payout id {codes[i].Item2}, player {codes[i].Item3}");
                        Console.WriteLine($"Duplicate Code {codes[j].Item1}, payout id {codes[j].Item2}, player {codes[j].Item3}");

                    }
                }

                //Console.WriteLine("code :" + codes[i]);
            }
        }

        private static int GetUnique(int a, int b)
        {
            int result = ((a - b)*(a + b + 1) + b)/2;
            if (result < 0)
                result *= -1;
            return result;
        }

        private static string GetEncode(int input)
        {
            var code = Encoder.Conceal(input);
            return code;
        }

        private static int GetDecode(string code)
        {
            var input = Encoder.Reveal(code);
            return input;
        }
    }
}
