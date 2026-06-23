using System;
using CodingChallenge.FamilyTree;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var ted = new Person { Name = "Ted", Birthday = new DateTime(1970, 1, 1) };
            var jim = new Person { Name = "Jim", Birthday = new DateTime(1990, 2, 2) };
            var sally = new Person { Name = "Sally", Birthday = new DateTime(1992, 3, 3) };
            var bob = new Person { Name = "Bob", Birthday = new DateTime(2010, 4, 4) };
            var joe = new Person { Name = "Joe", Birthday = new DateTime(2012, 5, 5) };
            var george = new Person { Name = "George", Birthday = new DateTime(2014, 6, 6) };

            ted.Descendants.Add(jim);
            ted.Descendants.Add(sally);
            jim.Descendants.Add(bob);
            sally.Descendants.Add(joe);
            sally.Descendants.Add(george);

            var solution = new Solution();
            var birthMonth = solution.GetBirthMonth(ted, "Joe");

            Console.WriteLine($"Joe's birth month is: {birthMonth}");
        }
    }
}
