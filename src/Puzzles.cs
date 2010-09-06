using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Text;

// FIXME:
// - return HashSet instead of List in FindModes methods.
namespace oishi.com
{
    [TestFixture]
    public class Puzzles {
        public static HashSet<int> FindModesUsingDictionary(params int[] numbers)
        {
            HashSet<int> modes = new HashSet<int>();
            int count = 1;
            Dictionary<int, int> hitbynumbers = new Dictionary<int, int>();
            foreach (int i in numbers) {
                if (hitbynumbers.ContainsKey(i)) hitbynumbers[i]++ ;
                else hitbynumbers[i] = 1;

                if (hitbynumbers[i] < count) continue;
                if (hitbynumbers[i] > count) modes.Clear();
                modes.Add(i);

                count = hitbynumbers[i];
            }

            return modes;
        }

        public static HashSet<int> FindModesThruSort (params int[] numbers)
        {
            HashSet<int> modes = new HashSet<int>();
            if (numbers.Length == 0) {
                return new HashSet<int>();
            }

            Array.Sort(numbers);

            int count = 1;
            int maxcount = 1;
            modes.Add(numbers[0]);
            for (int i = 1; i < numbers.Length; i++) {
                if (maxcount == 1) {
                    modes.Add(numbers[i]);
                }

                if (numbers[i] != numbers[i-1]) {
                    count = 1;
                    continue;
                }

                count++;

                if (count < maxcount) continue;
                if (count > maxcount) modes.Clear();
                modes.Add(numbers[i]);
                maxcount = count;
            }

            return modes;
        }

        public static HashSet<int> FindModesUsingArray(params int[] numbers)
        {
            if (null == numbers) {
                throw new NullReferenceException("'numbers' must be non-null.");
            }

            HashSet<int> modes = new HashSet<int> ();
            int max = 0;
            int min = 0;
            foreach (int i in numbers) {
                min = Math.Min(i, min);
                max = Math.Max(i, max);
            }

            int[] places = new int[max - min + 1];
            int count = 1;

            foreach (int i in numbers) {
                places[i-min]++;

                if (places[i-min] < count ) continue;
                if (places[i-min] > count ) modes.Clear();
                modes.Add(i);
                count = places[i-min];
            }

            return modes;
        }

        public static List<int> FindMissingNumbersUsingArray(params int[] numbers)
        {
            int max = 0;
            int min = 0;
            foreach (int i in numbers) {
                min = Math.Min(i, min);
                max = Math.Max(i, max);
            }

            List<int> missedlist = new List<int> ();
            Array.Sort(numbers);
            for ( int i = 1; i < numbers.Length; i++) {
                if ( numbers[i] <= numbers[i-1] + 1 ) continue;
                for (int missed = numbers[i-1] + 1; missed < numbers[i]; missed++) {
                    missedlist.Add(missed);
                }
            }

            return missedlist;
        }

        public static List<int> FindMissingNumbersUsingDictionary(params int[] numbers)
        {
            if (numbers.Length == 0) {
                return new List<int>(new int[]{});
            }

            int max = numbers[0];
            int min = numbers[0];
            foreach (int i in numbers) {
                min = Math.Min(i, min);
                max = Math.Max(i, max);
            }

            List<int> missedlist = new List<int> ();
            Dictionary<int, int> stored = new Dictionary<int, int>();

            foreach ( int i in numbers) {
                stored[i] = 1;
            }

            for ( int j = min + 1; j < max; j++ ) {
                if (stored.ContainsKey(j) == false) { missedlist.Add(j); continue;}
            }

            return missedlist;
        }

        public static List<int> FindMissingNumbersUsingBitmap(params int[] numbers)
        {
            if (numbers.Length == 0) {
                return new List<int>(new int[]{});
            }

            int max = numbers[0];
            int min = numbers[0];
            foreach (int i in numbers) {
                min = Math.Min(i, min);
                max = Math.Max(i, max);
            }

            List<int> missingNumber = new List<int>();
            BitArray stored = new BitArray(max-min+1);
            foreach (int i in numbers) {
                stored[i-min] = true;
            }

            for (int i = min + 1; i < max; i++) {
                if(stored[i-min] == false) missingNumber.Add(i);
            }

            return missingNumber;
        }

        public static double Median(int[] numbers)
        {
            Array.Sort(numbers);
            if (numbers.Length % 2 == 0) {
                double median = 0;
                median =  (numbers[numbers.Length/2 - 1] + numbers[numbers.Length/2])/2.0;
                return median;
            }

            return numbers[(numbers.Length + 1)/2 - 1];

        }

        public static string NumberToString(int number)
        {
            bool minus = false;
            if (number < 0 ) {
                minus = true;
                number = number * -1;
            }
            StringBuilder sb = new StringBuilder();
            if (number == 0) {
                sb.Insert(0, '0');
                return sb.ToString();
            }
            while (number > 0)
            {
                char num = (char)('0' + number % 10);
                sb.Insert(0, num);
                number = number / 10;
            }
            if(minus) sb.Insert(0, '-');
            return sb.ToString();
        }

        public static int StringToNumber(string text)
        {
            bool negative = false;
            int i = 0;
            while (i < text.Length && (text[i] > '9' || text[i] < '0')) {
                if ( text[i] == '+') {
                    i++; continue;
                }
                if (text[i] == '-'){
                    negative = true; i++; continue;
                }
                if (text[i] == ' ') {
                    i++; continue;
                }

                throw new ArgumentException("'text' must begin with a sign, or a digit.");
            }

            if (i >= text.Length) {
                throw new ArgumentException("'text' must contains a digit.");
            }

            int number = 0;
            for (int j = i; j < text.Length; j++)
            {
                if (text[j] > '9' || text[j] < '0') break;
                number = number * 10 + (text[j] - '0');
            }

            if (negative) {
                return -number;
            } else {
                return number;
            }
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
        public void TestFindModesUsingDictionary() {
            testFindModes(x => Puzzles.FindModesUsingDictionary(x));
        }

        [Test]
        public void TestFindModesUsingArray() {
            testFindModes(x => Puzzles.FindModesUsingArray(x));
        }

        [Test]
        public void TestFindModesThruSort() {
            testFindModes(x => Puzzles.FindModesThruSort(x));
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
        public void TestMedian() {
            Assert.AreEqual(3, Puzzles.Median(new int[]{1,2,3,4,5}));
            Assert.AreEqual(3.5 , Puzzles.Median(new int[]{1,2,3,4,5,6}));
            Assert.AreEqual(3, Puzzles.Median(new int[]{3}));
            Assert.AreEqual(2.5, Puzzles.Median(new int[]{2,3}));
            Assert.AreEqual(3.5, Puzzles.Median(new int[]{3,6,2,1,4,5}));
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
        public void TestMatchBrackets()
        {
            Assert.IsTrue(Puzzles.MatchBrackets(""));
            Assert.IsTrue(Puzzles.MatchBrackets("()"));
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
