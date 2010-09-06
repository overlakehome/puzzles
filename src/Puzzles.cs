using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

// FIXME:
// - return HashSet instead of List in FindModes methods.
namespace oishi.com
{
    [TestFixture]
    public class Puzzles {
        public static List<int> FindModesUsingDictionary(params int[] numbers)
        {
            List<int> modes = new List<int>();
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

        public static List<int> FindModesThruSort (params int[] numbers)
        {
            List<int> modes = new List<int>();
            if (numbers.Length == 0) {
                return new List<int>(new int[]{});
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

        public static List<int> FindModesUsingArray(params int[] numbers)
        {
            if (null == numbers) {
                throw new NullReferenceException("'numbers' must be non-null.");
            }

            List<int> modes = new List<int> ();
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
            Assert.AreEqual(new List<int>(new int[]{}), Puzzles.FindModesThruSort(new int[]{}));
            Assert.AreEqual(new List<int>(new int[]{5}), Puzzles.FindModesThruSort(new int[]{1, 3, 3, 5, 5, 5, 8, 8}));
            Assert.AreEqual(new List<int>(new int[]{3, 8}), Puzzles.FindModesThruSort(new int[]{1, 3, 3, 8, 8}));
            Assert.AreEqual(new List<int>(new int[]{1}), Puzzles.FindModesThruSort(new int[]{1}));
            Assert.AreEqual(new List<int>(new int[]{1,2}), Puzzles.FindModesThruSort(new int[]{1, 2}));
            Assert.AreEqual(new List<int>(new int[]{1,2,3}), Puzzles.FindModesThruSort(new int[]{1, 2, 3}));

            Assert.AreEqual(new List<int>(new int[]{1,2}), Puzzles.FindModesThruSort(new int[]{1, 1, 2, 2}));
            Assert.AreEqual(new List<int>(new int[]{2}), Puzzles.FindModesThruSort(new int[]{1, 2, 2}));
            Assert.AreEqual(new List<int>(new int[]{1, 2}), Puzzles.FindModesThruSort(new int[]{3, 2, 1, 2, 1}));
            try {
                Assert.AreEqual(new List<int>(new int[]{5}), Puzzles.FindModesThruSort(null));
                Assert.Fail("");
            } catch (NullReferenceException) {
            }
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
        public void TestMatchBrackets()
        {
            Assert.IsTrue(Puzzles.MatchBrackets(""));
            Assert.IsTrue(Puzzles.MatchBrackets("()"));
        }

        private void testFindMissingNumbers(Func<int[], List<int>> findMissingNumbers) {
            Assert.AreEqual(new List<int>(new int[]{}), findMissingNumbers(new int[]{}));
            Assert.AreEqual(new List<int>(new int[]{2,4,6,7}), findMissingNumbers(new int[]{1, 3, 3, 5, 5, 5, 8, 8}));
            Assert.AreEqual(new List<int>(new int[]{2,4,6,7}), findMissingNumbers(new int[]{8, 9, 1, 3, 5}));
            Assert.AreEqual(new List<int>(new int[]{2}), findMissingNumbers(new int[]{3, 1}));
            Assert.AreEqual(new List<int>(new int[]{}), findMissingNumbers(new int[]{10, 10, 10}));
            Assert.AreEqual(new List<int>(new int[]{}), findMissingNumbers(new int[]{1, 2, 3, 4}));

            Assert.AreEqual(new List<int>(new int[]{2}), findMissingNumbers(new int[]{1, 1, 3, 3}));
            Assert.AreEqual(new List<int>(new int[]{2, 3, 4, 5, 6, 7, 8, 9}), findMissingNumbers(new int[]{10, 1}));
            try {
                Assert.AreEqual(new List<int>(new int[]{5}), findMissingNumbers(null));
                Assert.Fail("");
            } catch (NullReferenceException) {
            }
        }

        private void testFindModes(Func<int[], List<int>> findModes) {
            Assert.AreEqual(new List<int>(new int[]{}), findModes(new int[]{}));
            Assert.AreEqual(new List<int>(new int[]{5}), findModes(new int[]{1, 3, 3, 5, 5, 5, 8, 8}));
            Assert.AreEqual(new List<int>(new int[]{3, 8}), findModes(new int[]{1, 3, 3, 8, 8}));
            Assert.AreEqual(new List<int>(new int[]{1}), findModes(new int[]{1}));
            Assert.AreEqual(new List<int>(new int[]{1,2}), findModes(new int[]{1, 2}));
            Assert.AreEqual(new List<int>(new int[]{1,2}), findModes(new int[]{1, 1, 2, 2}));
            Assert.AreEqual(new List<int>(new int[]{2}), findModes(new int[]{1, 2, 2}));
            Assert.AreEqual(new List<int>(new int[]{2, 1}), findModes(new int[]{3, 2, 1, 2, 1}));
            try {
                Assert.AreEqual(new List<int>(new int[]{5}), findModes(null));
                Assert.Fail("");
            } catch (NullReferenceException) {
            }
        }
    }
}
