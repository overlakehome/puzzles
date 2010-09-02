using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace oishi.com
{
    public class Puzzles {
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
        public void TestMatchBrackets()
        {
            Assert.IsTrue(Puzzles.MatchBrackets(""));
            Assert.IsTrue(Puzzles.MatchBrackets("()"));
            Assert.IsTrue(Puzzles.MatchBrackets("[()]"));
            Assert.IsTrue(Puzzles.MatchBrackets("{[()]}"));
        }
    }
}

