using System;

namespace CodingChallenge.PirateSpeak
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Solution().GetPossibleWords("ortsp", ["sport", "parrot", "ports", "matey"]);
            Array.ForEach(test, Console.WriteLine);
            Console.Read();
        }
    }
}
