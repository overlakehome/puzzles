using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace oishi.com
{
    [TestFixture]
    public class Puzzles {
        public static List<int> findModes(params int[] numbers)
        {
            //List<int> set = new List<int>();
            //set.AddRange();

            List<int> modes = new List<int>();
            int count = 1;
            Dictionary<int, int> hitbynumbers = new Dictionary<int, int>();
            foreach (int i in numbers)
            {
                if (hitbynumbers.ContainsKey(i)) hitbynumbers[i]++ ;
                else hitbynumbers[i] = 1;

                if (hitbynumbers[i] < count) continue;
                if (hitbynumbers[i] > count) modes.Clear();
                modes.Add(i);

                count = hitbynumbers[i];

            }

            return modes;
        }

        public static bool MatchBrackets(string input)
        {
            Stack<char> brackets = new Stack<char> ();
            foreach (char c in input) {
                if ('(' == c) brackets.Push(')');
                else if ('[' == c) brackets.Push(']');
                else if ('{' == c) brackets.Push('}');
                else if (')' == c || ']' == c || '}' == c) {
                    if (0 == brackets.Count || c != brackets.Pop()) return false;
                }
            }

            return 0 == brackets.Count;
        }
    }

    [TestFixture]
    public class PuzzlesTest
    {
        [Test]
        public void TestFindModes() {
            Assert.AreEqual(new List<int>(new int[]{}), Puzzles.findModes(new int[]{}));
            Assert.AreEqual(new List<int>(new int[]{5}), Puzzles.findModes(new int[]{1, 3, 3, 5, 5, 5, 8, 8}));
            Assert.AreEqual(new List<int>(new int[]{3, 8}), Puzzles.findModes(new int[]{1, 3, 3, 8, 8}));
            Assert.AreEqual(new List<int>(new int[]{1}), Puzzles.findModes(new int[]{1}));
            Assert.AreEqual(new List<int>(new int[]{1,2}), Puzzles.findModes(new int[]{1, 2}));
            Assert.AreEqual(new List<int>(new int[]{1,2}), Puzzles.findModes(new int[]{1, 1, 2, 2}));
            Assert.AreEqual(new List<int>(new int[]{2}), Puzzles.findModes(new int[]{1, 2, 2}));
            Assert.AreEqual(new List<int>(new int[]{2, 1}), Puzzles.findModes(new int[]{3, 2, 1, 2, 1}));
            try {
                Assert.AreEqual(new List<int>(new int[]{5}), Puzzles.findModes(null));
                Assert.Fail("");
            } catch (NullReferenceException nre) {
                // OK
            }
        }

        [Test]
        public void TestMatchBrackets()
        {
            Assert.IsTrue(Puzzles.MatchBrackets(""));
            Assert.IsTrue(Puzzles.MatchBrackets("()"));

        }
    }
}