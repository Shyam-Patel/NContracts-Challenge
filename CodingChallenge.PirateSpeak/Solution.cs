using System;
using System.Linq;

namespace CodingChallenge.PirateSpeak
{
    public class Solution
    {
        public string[] GetPossibleWords(string jumble, string[] dictionary)
        {
            // Create canonical form of jumble by sorting characters
            string sortedJumble = new string(jumble.OrderBy(c => c).ToArray());
            
            // Filter words that are anagrams (same length + same sorted characters)
            return dictionary
                .Where(word => word.Length == jumble.Length)
                .Where(word => new string(word.OrderBy(c => c).ToArray()) == sortedJumble)
                .ToArray();
        }
    }
}