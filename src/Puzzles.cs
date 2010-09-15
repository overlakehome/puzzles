using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace oishi.com
{
    public partial class Puzzles {
        public static HashSet<int> FindModesUsingDictionary(params int[] numbers)
        {
            // space to yield modes: O(n)
            HashSet<int> modes = new HashSet<int>();

            // time to scan the input: O(n)
            // space to keep a dictionary: O(1.6 n) = O(n)
            int maxHits = 1;
            Dictionary<int, int> hitsByNumber = new Dictionary<int, int>();
            foreach (int i in numbers) {
                if (hitsByNumber.ContainsKey(i)) hitsByNumber[i]++ ;
                else hitsByNumber[i] = 1;

                if (hitsByNumber[i] < maxHits) continue;
                if (hitsByNumber[i] > maxHits) modes.Clear();
                modes.Add(i);

                maxHits = hitsByNumber[i];
            }

            return modes;
        }

        public static HashSet<int> FindModesUsingSort (params int[] numbers)
        {
            if (numbers.Length == 0) {
                return new HashSet<int>();
            }

            // time to sort: O(nlogn)
            // space to sort: O(logn) because of recursive calls.
            Array.Sort(numbers);

            // time to scan: O(n)
            // space to yield modes: O(n)
            HashSet<int> modes = new HashSet<int>();
            modes.Add(numbers[0]);
            for (int i = 1, count = 1, maxcount = 1; i < numbers.Length; i++) {
                count = (numbers[i] == numbers[i-1]) ? count + 1 : 1;
                if (count >= maxcount) {
                    if (count > maxcount) {
                        modes.Clear();
                    }

                    modes.Add(numbers[i]);
                    maxcount = count;
                }
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
            for (int i = 1; i < numbers.Length; i++) {
                if (numbers[i] <= numbers[i-1] + 1) continue;
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

        public static double FindMedian(int[] numbers)
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
            if (number == 0){
                return "0";
            }

            bool minus = false;
            if (number < 0 ) {
                minus = true;
                number = number * -1;
            }
            StringBuilder sb = new StringBuilder();
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
            if (null == text) throw new ArgumentNullException("text");
            if (text.Length == 0) throw new ArgumentException("'text' must be non-empty.");
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

        public static string ReverseStringUsingStringBuilder(string text){

            if (text == null) throw new ArgumentException("'text' shoun't be null");
            if (text.Length  == 0 ) throw new ArgumentException("'text' shouldn't be empty");
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < text.Length; i++){
                sb.Insert(0, text[i]);
            }
            return sb.ToString();
        }

        public static string ReverseStringUsingChars(String text){
            if (text == null) throw new ArgumentException("'text' shoun't be null");
            if (text.Length  == 0 ) throw new ArgumentException("'text' shouldn't be empty");
            int i = 0;
            int j = text.Length - 1;
            while (i < text.Length && text[i] == ' '){
               i++;
            }
            while (j >= 0 && text[j] == ' '){
                j--;
            }

            char[] reversed = new char[j-i+1];
            for (int head = 0; head < reversed.Length; head++){
                reversed[head] = text[j - head];
            }

            return new string(reversed);
        }

        public static int[] LastIndexesOfChars(string s, params char[] chars){
            int[] lastIndexes = new int[chars.Length];
            for (int i = 0; i < chars.Length; i++){
                lastIndexes[i] = -1;
                for (int j = 0; j < s.Length; j++) {
                    if (s[j] == chars[i]) {
                        lastIndexes[i] = j;
                    }
                }
            }

            return lastIndexes;
        }

        public static int[] LastIndexesOfCharsUsingDictionary (string s, char[] chars){
            Dictionary<char, int> lastIndexs = new Dictionary<char, int>();
            int[] lastIndexes = new int[chars.Length];
            for(int i = 0 ; i < s.Length; i++){
                lastIndexs[s[i]] = i;
            }

            for(int i = 0; i < chars.Length; i++) {
                if(lastIndexs.ContainsKey(chars[i])){
                    lastIndexes[i] = lastIndexs[chars[i]];
                }
            }

            return lastIndexes;
        }

        public static List<char> FindNonRepeatedChars(string s){
            Dictionary<char, bool> map = new Dictionary<char, bool>();
            List<char> nonRepeated = new List<char>();
            for (int i = 0; i < s.Length; i++){
                if(map.ContainsKey(s[i])) map[s[i]] = false;
                else {
                    map[s[i]] = true;
                }
            }

            for (int i = 0; i < s.Length; i++){
                if(map[s[i]]) nonRepeated.Add(s[i]);
            }

            return nonRepeated;
        }

        public static string ReplaceSubstring(string input, string pattern, string replace) {
            if (input == null) throw new ArgumentNullException("input");
            if (pattern  == null) throw new ArgumentNullException("pattern");
            if (replace  == null) throw new ArgumentNullException("replace");
            if (input.Length == 0) return "";
            if (pattern.Length == 0) throw new ArgumentException("'pattern' must be non-empty.");

            StringBuilder sb = new StringBuilder();
            for (int tail = 0; tail < input.Length; ) {
                while (tail < input.Length && input[tail] != pattern[0]) {
                    sb.Append(input[tail]);
                    tail++;
                }

                int head = tail;
                for (int i = 0; tail < input.Length && i < pattern.Length && input[tail] == pattern[i]; tail++, i++) {
                }

                int span = tail - head;
                if (span != pattern.Length) {
                    sb.Append(input, head, span);
                }
                else {
                    if (replace.Length == 0) {
                        break;
                    } else {
                        sb.Append(replace);
                    }
                }
            }

            return sb.ToString();
        }

        public static bool MatchBrackets(string input){
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

        public static string[] PostfixOf(params string[] infix){
            Stack<string> stack = new Stack<string>();
            List<string> postfix = new List<string>(infix.Length); // the same capacity as the infix.

            Regex digits = new Regex(@"^\d+$"); // \d+ matches any decimal digits.
            Regex @operator = new Regex(@"^[+\-*/]$");

            foreach (string s in infix) {
                if (digits.IsMatch(s)) {
                    postfix.Add(s);
                } else if (@operator.IsMatch(s)) {
                    // output all the operators that are higher than, or equal to the current input.
                    if ("+" == s || "-" == s) {
                        while (stack.Count > 0) { // pop
                            postfix.Add(stack.Pop());
                        }
                    } else {
                        Debug.Assert("*" == s || "/" == s);
                        while (stack.Count > 0 && "*" == stack.Peek() || "/" == stack.Peek()) {
                            postfix.Add(stack.Pop());
                        }
                    }

                    stack.Push(s);
                } else {
                    throw new ArgumentException(string.Format("'infix' contains an invalid token '{0}'.", s));
                }
            }

            while (stack.Count > 0) {
                postfix.Add(stack.Pop());
            }

            return postfix.ToArray();
        }

        public static int EvaluatePostfix(params string[] postfix) {
            Regex digits = new Regex(@"^\d+$"); // \d+ matches any decimal digits.
            Regex @operator = new Regex(@"^[+\-*/]$");

            Stack<string> stack = new Stack<string>();
            foreach (string s in postfix) {
                if (@operator.IsMatch(s)) {
                    if (stack.Count < 2) {
                        throw new InvalidOperationException("'postfix' has invalid expression.");
                    }

                    int rhs = int.Parse(stack.Pop());
                    int lhs = int.Parse(stack.Pop());
                    int eval;
                    switch (s) {
                        case "+": eval = lhs + rhs; break;
                        case "-": eval = lhs - rhs; break;
                        case "*": eval = lhs * rhs; break;
                        case "/": eval = lhs / rhs; break;
                        default: throw new InvalidOperationException("'postfix' has invalid expression.");
                    }

                    stack.Push(eval.ToString());
                } else if (digits.IsMatch(s)) {
                    stack.Push(s);
                } else {
                    throw new InvalidOperationException("'postfix' has invalid expression.");
				}
            }

            if (stack.Count > 1) {
                throw new InvalidOperationException("'postfix' has invalid expression.");
            }

            return int.Parse(stack.Pop());
        }
    }
}