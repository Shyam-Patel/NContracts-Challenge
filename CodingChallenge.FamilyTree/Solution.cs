using System;
using System.Collections.Generic;

namespace CodingChallenge.FamilyTree
{
    public class Solution
    {
        public string GetBirthMonth(Person person, string descendantName)
        {
            return GetBirthMonthBFS(person, descendantName);
        }

        public string GetBirthMonthBFS(Person person, string descendantName)
        {
            if (person == null || string.IsNullOrWhiteSpace(descendantName))
                return string.Empty;
        
            var queue = new Queue<Person>();
            queue.Enqueue(person);
        
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (string.Equals(current.Name, descendantName, StringComparison.Ordinal))
                    return current.Birthday.ToString("MMMM");
        
                if (current.Descendants != null)
                {
                    foreach (var child in current.Descendants)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        
            return string.Empty;
        }

        public string GetBirthMonthDFS(Person person, string descendantName)
        {
            if (person == null || string.IsNullOrWhiteSpace(descendantName))
                return string.Empty;
        
            var stack = new Stack<Person>();
            stack.Push(person);
        
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (string.Equals(current.Name, descendantName, StringComparison.Ordinal))
                    return current.Birthday.ToString("MMMM");
        
                if (current.Descendants != null)
                {
                    foreach (var child in current.Descendants)
                    {
                        stack.Push(child);
                    }
                }
            }
        
            return string.Empty;
        }
    }
}