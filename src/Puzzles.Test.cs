using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace oishi.com
{
    [TestFixture]
    public partial class Puzzles
    {
        [Test]
        public void TestFindModesUsingDictionary() {
            testFindModes(x => Puzzles.FindModesUsingDictionary(x));
        }

        [Test]
        public void TestFindModesUsingArray() {
            testFindModes(x => Puzzles.FindModesUsingArray(x));
        }

        [Test]
        public void TestFindModesUsingSort() {
            testFindModes(x => Puzzles.FindModesUsingSort(x));
        }

        [Test]
        public void TestFindMissingNumbersUsingArray() {
            testFindMissingNumbers(x => Puzzles.FindMissingNumbersUsingArray(x));
        }

        [Test]
        public void TestFindMissingNumbersUsingDictionary() {
            testFindMissingNumbers(x => Puzzles.FindMissingNumbersUsingDictionary(x));
        }

        [Test]
        public void TestFindMissingNumbersUsingBitmap() {
            testFindMissingNumbers(x => Puzzles.FindMissingNumbersUsingBitmap(x));
        }

        [Test]
        public void TestFindMedian() {
            Assert.AreEqual(3, Puzzles.FindMedian(new int[]{1,2,3,4,5}));
            Assert.AreEqual(3.5 , Puzzles.FindMedian(new int[]{1,2,3,4,5,6}));
            Assert.AreEqual(3, Puzzles.FindMedian(new int[]{3}));
            Assert.AreEqual(2.5, Puzzles.FindMedian(new int[]{2,3}));
            Assert.AreEqual(3.5, Puzzles.FindMedian(new int[]{3,6,2,1,4,5}));
        }

        [Test]
        public void TestNumberToString() {
            Assert.AreEqual("3", Puzzles.NumberToString(3));
            Assert.AreEqual("35", Puzzles.NumberToString(35));
            Assert.AreEqual("-35", Puzzles.NumberToString(-35));
            Assert.AreEqual("0", Puzzles.NumberToString(0));
        }

        [Test]
        public void TestStringToNumber(){
            Assert.AreEqual(1, Puzzles.StringToNumber("1"));
            Assert.AreEqual(12, Puzzles.StringToNumber("12"));
            Assert.AreEqual(321, Puzzles.StringToNumber("321"));
            Assert.AreEqual(1, Puzzles.StringToNumber("+1"));
            Assert.AreEqual(-1, Puzzles.StringToNumber("-1"));
            Assert.AreEqual(1, Puzzles.StringToNumber(" 1"));
            Assert.AreEqual(1, Puzzles.StringToNumber(" +1"));
            Assert.AreEqual(-1, Puzzles.StringToNumber(" - 1"));
            Assert.AreEqual(1, Puzzles.StringToNumber(" ++ 1"));
            Assert.AreEqual(-1, Puzzles.StringToNumber(" -- 1"));
            Assert.AreEqual(-123, Puzzles.StringToNumber(" -- 123-"));
            Assert.AreEqual(-123, Puzzles.StringToNumber(" -- 123a"));
            try{
                Puzzles.StringToNumber("+");
                Assert.Fail("It should have thrown an argument exception.");
            } catch(ArgumentException) {
            }
        }

        [Test]
        public void TestReverseStringUsingStringBuilder(){
            Assert.AreEqual("edcba", Puzzles.ReverseStringUsingStringBuilder("abcde"));
            Assert.AreEqual("aa12", Puzzles.ReverseStringUsingStringBuilder("21aa"));
            Assert.AreEqual("a", Puzzles.ReverseStringUsingStringBuilder("a"));
            Assert.AreEqual("edc ba", Puzzles.ReverseStringUsingStringBuilder("ab cde"));
            try{
                Puzzles.ReverseStringUsingStringBuilder("");
                Assert.Fail("It should have thrown an argument exception.");
            } catch(ArgumentException) {
            }
        }

        [Test]
        public void TestReverseStringUsingChars(){
            Assert.AreEqual("edcba", Puzzles.ReverseStringUsingChars("abcde"));
            Assert.AreEqual("aa12", Puzzles.ReverseStringUsingChars("21aa"));
            Assert.AreEqual("a", Puzzles.ReverseStringUsingChars("a"));
            Assert.AreEqual("edc ba", Puzzles.ReverseStringUsingChars("ab cde"));
            Assert.AreEqual("321", Puzzles.ReverseStringUsingChars("    123"));
            Assert.AreEqual("321", Puzzles.ReverseStringUsingChars("    123   "));
            try{
                Puzzles.ReverseStringUsingChars("");
                Assert.Fail("It should have thrown an argument exception.");
            } catch(ArgumentException) {
            }
        }

        [Test]
        public void TestLastIndexesOfChars() {
            testLastIndexesOfChars((x, y) => Puzzles.LastIndexesOfChars(x, y));
        }

        [Test]
        public void TestLastIndexesOfCharsUsingDictionary() {
            testLastIndexesOfChars((x, y) => Puzzles.LastIndexesOfCharsUsingDictionary(x, y));
        }

        private void testLastIndexesOfChars(Func<string, char[], int[]> lastIndexesOfChars) {
            Assert.AreEqual(new int[]{3}, lastIndexesOfChars("abcde", new char[]{'d'}));
            Assert.AreEqual(new int[]{1, 3}, lastIndexesOfChars("hello", new char[] {'e','l'}));
            Assert.AreEqual(new int[]{4, 1}, lastIndexesOfChars("hello", new char[] {'o','e'}));
        }

        [Test]
        public void TestFindNonRepeatedChars(){
            Assert.AreEqual(new char[]{'a', 'c'}, Puzzles.FindNonRepeatedChars("abcbdd"));
            Assert.AreEqual(new char[]{'a', 'b', 'c', 'd'}, Puzzles.FindNonRepeatedChars("abcd"));
            Assert.AreEqual(new char[]{}, Puzzles.FindNonRepeatedChars("aabbcc"));
            Assert.AreEqual(new char[]{'m', 'y'}, Puzzles.FindNonRepeatedChars("hellohellomyhello"));
            Assert.AreEqual(new char[]{'3'}, Puzzles.FindNonRepeatedChars("hihi3"));
        }

        [Test]
        public void TestReplaceSubstring() {
            // FIXME: boundary value analysis, equivalent class partitioning, and combinational technique.
            //
            // positive cases:
            // - input begins, or ends with an incomplete match.-ok
            // - input has not even an incomplete match.-ok
            // - replace string is longer than input and it matches in the beginning
            // -
            //
            // some interesting cases.
            // - pattern equals replace, e.g. "abc", "xyz", "xyz".
            // - replace yeilds another match, e.g. "abb", "ab", "a"

            // missing cases:
            // - pattern is longer than input. the result is input.
            // - pattern is same as replace. the result is input.

            Assert.AreEqual("hello", Puzzles.ReplaceSubstring("helloworld", "world", ""));
            Assert.AreEqual("hihello", Puzzles.ReplaceSubstring("worldhello", "world", "hi"));
            Assert.AreEqual("hellohi", Puzzles.ReplaceSubstring("helloworld", "world", "hi"));
            Assert.AreEqual("hihihello", Puzzles.ReplaceSubstring("worldworldhello", "world", "hi"));
            Assert.AreEqual("hellohihi", Puzzles.ReplaceSubstring("helloworldworld", "world", "hi"));
            Assert.AreEqual("hellohihihello", Puzzles.ReplaceSubstring("helloworldworldhello", "world", "hi"));
            Assert.AreEqual("worlhello", Puzzles.ReplaceSubstring("worlhello", "world", "hi"));
            Assert.AreEqual("helloworl", Puzzles.ReplaceSubstring("helloworl", "world", "hi"));
            Assert.AreEqual("hi", Puzzles.ReplaceSubstring("world", "world", "hi"));
            Assert.AreEqual("hihi", Puzzles.ReplaceSubstring("worldworld", "world", "hi"));
            Assert.AreEqual("hi", Puzzles.ReplaceSubstring("world", "world", "hi"));
            Assert.AreEqual("", Puzzles.ReplaceSubstring("", "world", "helloworld"));
            Assert.AreEqual("hello", Puzzles.ReplaceSubstring("hello", "world", "helloworld"));

            try {
                Puzzles.ReplaceSubstring("hello", "", "helloworld");
                Assert.Fail("'ReplaceSubstring' should have thrown an argument exception.");
            } catch (ArgumentException) {
            }

            try {
                Puzzles.ReplaceSubstring(null, "world", "hi");
                Assert.Fail("'ReplaceSubstring' should have thrown an argument null exception.");
            } catch (ArgumentNullException) {
            }

            try {
                Puzzles.ReplaceSubstring("hello", null, "hi");
                Assert.Fail("'ReplaceSubstring' should have thrown an argument null exception.");
            } catch (ArgumentNullException) {
            }

            try {
                Puzzles.ReplaceSubstring("hello", "world", null);
                Assert.Fail("'ReplaceSubstring' should have thrown an argument null exception.");
            } catch (ArgumentNullException) {
            }
        }

        [Test]
        public void TestMatchBrackets()
        {
            Assert.IsTrue(Puzzles.MatchBrackets(""));
            Assert.IsTrue(Puzzles.MatchBrackets("()"));
            Assert.IsTrue(Puzzles.MatchBrackets("({})"));
            Assert.IsTrue(Puzzles.MatchBrackets("({[]})"));
            Assert.IsFalse(Puzzles.MatchBrackets("("));
            Assert.IsFalse(Puzzles.MatchBrackets("({"));
            Assert.IsFalse(Puzzles.MatchBrackets("({["));
            Assert.IsFalse(Puzzles.MatchBrackets(")"));
            Assert.IsTrue(Puzzles.MatchBrackets("({})[]"));
            Assert.IsFalse(Puzzles.MatchBrackets("({}))"));
            Assert.IsTrue(Puzzles.MatchBrackets("(abc)d"));
        }

        [Test]
        public void TestPostfixOf() {
            Assert.AreEqual(new string[] {"1", "3", "4", "*" , "+", "5", "+"}, Puzzles.PostfixOf(new string[] {"1", "+", "3", "*", "4", "+", "5"}));
            Assert.AreEqual(new string[] {"1", "2", "3", "*" , "+", "4", "-"}, Puzzles.PostfixOf(new string[] {"1", "+", "2", "*", "3", "-", "4"}));
            Assert.AreEqual(new string[] {"1", "3", "+"}, Puzzles.PostfixOf(new string[] {"1", "+", "3"}));
        }

        [Test]
        public void TestEvaluatePostfix() {
            Assert.AreEqual(18, Puzzles.EvaluatePostfix(Puzzles.PostfixOf(new string[] {"1", "+", "3", "*", "4", "+", "5"})));
            Assert.AreEqual(8, Puzzles.EvaluatePostfix(Puzzles.PostfixOf(new string[] {"1", "+", "3", "*", "4", "-", "5"})));
            Assert.AreEqual(4, Puzzles.EvaluatePostfix(Puzzles.PostfixOf(new string[] {"1", "+", "3"})));
            Assert.AreEqual(-25, Puzzles.EvaluatePostfix(Puzzles.PostfixOf(new string[] {"1", "-", "3", "*", "4", "*", "2", "-", "2"})));
            Assert.AreEqual(11, Puzzles.EvaluatePostfix(Puzzles.PostfixOf(new string[] {"1", "+", "10", "/", "2", "+", "5"})));
            try {
                Puzzles.EvaluatePostfix(Puzzles.PostfixOf(new string[] {"1", "+"}));
                Assert.Fail("'postfixExp' should have tnhrown an argument exception.");
            } catch(InvalidOperationException){
            }
            try {
                Puzzles.EvaluatePostfix(Puzzles.PostfixOf(new string[] {"+", "+", "3", "*", "4", "+", "5"}));
                Assert.Fail("'postfixExp' should have tnhrown an argument exception.");
            } catch(InvalidOperationException){
            }
            try {
                Puzzles.EvaluatePostfix(Puzzles.PostfixOf(new string[] {"1", "+", "3", "3", "4", "+", "5"}));
                Assert.Fail("'postfixExp' should have tnhrown an argument exception.");
            } catch(InvalidOperationException){
            }
        }

        private void testFindMissingNumbers(Func<int[], List<int>> findMissingNumbers) {

            Assert.IsTrue(new HashSet<int>().SetEquals(findMissingNumbers(new int[]{})));
            Assert.IsTrue(new HashSet<int>(new int[]{2,4,6,7}).SetEquals(findMissingNumbers(new int[]{1, 3, 3, 5, 5, 5, 8, 8})));
            Assert.IsTrue(new HashSet<int>(new int[]{2,4,6,7}).SetEquals(findMissingNumbers(new int[]{8, 9, 1, 3, 5})));
            Assert.IsTrue(new HashSet<int>(new int[]{2}).SetEquals(findMissingNumbers(new int[]{3, 1})));
            Assert.IsTrue(new HashSet<int>(new int[]{}).SetEquals(findMissingNumbers(new int[]{10, 10, 10})));
            Assert.IsTrue(new HashSet<int>(new int[]{}).SetEquals(findMissingNumbers(new int[]{1, 2, 3, 4})));
            Assert.IsTrue(new HashSet<int>(new int[]{2}).SetEquals(findMissingNumbers(new int[]{1, 1, 3, 3})));
            Assert.IsTrue(new HashSet<int>(new int[]{2, 3, 4, 5, 6, 7, 8, 9}).SetEquals(findMissingNumbers(new int[]{10, 1})));
            try {
                findMissingNumbers(null);
                Assert.Fail("'findMissingNumbers' should have thrown a null reference exception.");
            } catch (NullReferenceException) {
            }
        }

        private void testFindModes(Func<int[], HashSet<int>> findModes) {
            Assert.IsTrue(findModes(new int[]{}).SetEquals(new int[0]));
            Assert.IsTrue(findModes(new int[]{1, 3, 3, 5, 5, 5, 8, 8}).SetEquals(new int[]{5}));
            Assert.IsTrue(findModes(new int[]{1, 3, 3, 8, 8}).SetEquals(new int[]{3, 8}));
            Assert.IsTrue(findModes(new int[]{1}).SetEquals(new int[]{1}));
            Assert.IsTrue(findModes(new int[]{1, 2}).SetEquals(new int[]{1,2}));
            Assert.IsTrue(findModes(new int[]{1, 1, 2, 2}).SetEquals(new int[]{1,2}));
            Assert.IsTrue(findModes(new int[]{1, 2, 2}).SetEquals(new int[]{2}));
            Assert.IsTrue(findModes(new int[]{3, 2, 1, 2, 1}).SetEquals(new int[]{2, 1}));
            try {
                findModes(null);
                Assert.Fail("'findModes' should have thrown a null reference exception.");
            } catch (NullReferenceException) {
            }
        }
    }
}