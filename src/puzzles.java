import java.util.AbstractMap.SimpleEntry;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.BitSet;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.NoSuchElementException;
import java.util.Random;
import java.util.Stack;

import junit.framework.Assert;

import org.junit.Test;

import com.google.common.base.Function;
import com.google.common.base.Joiner;
import com.google.common.collect.ImmutableList;
import com.google.common.collect.Iterables;

public class puzzles {

    public static class LinkedList<T> {
        public static class Node<T> {
            public T getValue() { throw new UnsupportedOperationException(); }
            public Node<T> getNext() { throw new UnsupportedOperationException(); }
            public Node<T> getPrevious() { throw new UnsupportedOperationException(); }
        }

        public boolean isEmpty() { throw new UnsupportedOperationException(); }
        public T getFirst() { throw new UnsupportedOperationException(); }
        public Node<T> getLastNode() { throw new UnsupportedOperationException(); }
        public void remove(Node<T> t) { throw new UnsupportedOperationException(); }
        public void removeFirst() { throw new UnsupportedOperationException(); }
        public void addLast(Node<T> t) { throw new UnsupportedOperationException(); }
        public void addLast(T t) { throw new UnsupportedOperationException(); }
    }

    public static class LinkedDictionary<K, V> {
        private final LinkedList<SimpleEntry<K, V>> linkedList = new LinkedList<SimpleEntry<K, V>>();
        private final Map<K, LinkedList.Node<SimpleEntry<K, V>>> dictionary = new HashMap<K, LinkedList.Node<SimpleEntry<K, V>>>();
        private final int capacity = 1024;

        public void trimEldest() {
            SimpleEntry<K, V> eldest = this.linkedList.getFirst();
            this.dictionary.remove(eldest.getKey());
            this.linkedList.removeFirst();
        }

        private void access(LinkedList.Node<SimpleEntry<K, V>> node) {
            this.linkedList.remove(node);
            this.linkedList.addLast(node);
        }

        public void put(K key, V value) {
            this.remove(key);
            this.linkedList.addLast(new SimpleEntry<K, V>(key, value));
            this.dictionary.put(key, this.linkedList.getLastNode());

            while (this.dictionary.size() > this.capacity) {
                this.trimEldest();
            }
        }

        public V get(K key) {
            if (!dictionary.containsKey(key)) {
                return null;
            }

            access(dictionary.get(key));
            return dictionary.get(key).getValue().getValue();
        }

        public boolean remove(K key) {
            if (!dictionary.containsKey(key)) {
                return false;
            }

            linkedList.remove(dictionary.get(key));
            dictionary.remove(key);
            return true;
        }
    }

    @Test
    public void testProducts() {
        Iterables.elementsEqual(ImmutableList.of(120, 60, 40, 30, 24), ImmutableList.of(products(1, 2, 3, 4, 5)));
    }

    public static int[] products(int... numbers) {
        int[] exclusiveProducts = new int[numbers.length];
        int[] prefixProducts = new int[numbers.length];
        int[] postfixProducts = new int[numbers.length];

        prefixProducts[0] = 1;
        for (int i = 1; i < numbers.length; i++) {
            prefixProducts[i] = prefixProducts[i-1] * numbers[i-1];
        }

        postfixProducts[numbers.length - 1] = 1;
        for (int i = numbers.length - 2; i >= 0; i--) {
            postfixProducts[i] = postfixProducts[i+1] * numbers[i+1];
        }

        for (int i = 0; i < numbers.length; i++) {
            exclusiveProducts[i] = prefixProducts[i] * postfixProducts[i];
        }

        return exclusiveProducts;
    }

    @Test
    public void testPermutate() {
        Assert.assertTrue(
                Iterables.elementsEqual(
                    ImmutableList.of("abc", "acb", "bac", "bca", "cba", "cab"),
                    permutate("abc".toCharArray(), 0)));
        Assert.assertTrue(
                Iterables.elementsEqual(
                    ImmutableList.of("abab", "abba", "aabb", "baab", "baba", "bbaa"),
                    permutate("abab".toCharArray(), 0)));
    }

    // http://n1b-algo.blogspot.com/2009/01/string-permutations.html
    public static Iterable<String> permutate(char[] chars, int k) {
        if (chars.length == k) {
            return ImmutableList.of(String.valueOf(chars));
        }

        List<String> permutations = new ArrayList<String>();
        BitSet bits = new BitSet(128);
        for (int i = k; i < chars.length; i++) {
            if (bits.get(chars[i])) {
                continue;
            } else {
                bits.set(chars[i]);
                swap(chars, k, i);
                Iterables.addAll(permutations, permutate(chars, k + 1));
                swap(chars, k, i);
            }
        }

        return permutations;
    }

    public static void swap(char[] chars, int i, int j) {
        if (chars[i] != chars[j]) {
            chars[i] ^= chars[j];
            chars[j] ^= chars[i];
            chars[i] ^= chars[j];
        }
    }

    @Test
    public void testDivide() {
        Assert.assertEquals(1024768/7, divide(1024768, 7));
    }

    public static int divide(int dividend, int divisor) {
        int bit = 1;
        int quotient = 0;
        while (divisor <= dividend) {
            divisor <<= 1;
            bit <<= 1;
        }

        while (divisor > 0) {
            divisor >>= 1;
            bit >>= 1;
            if (dividend >= divisor) {
                dividend -= divisor;
                quotient |= bit;
            }
        }

        return quotient;
    }

    @Test
    public void testKadane() {
        int[] a = { -2, 1, -3, 4, -1, 2, 1, -5, 4};
        Assert.assertTrue(Arrays.equals(new int[]{6, 3, 6}, kadane(a)));
        Assert.assertTrue(Arrays.equals(new int[]{3, 0, 0, 2, 2}, kadane(new int[][] {{1, 0, 1}, {0, -1, 0}, {1, 0, 1}})));
        Assert.assertTrue(Arrays.equals(new int[]{3, 0, 0, 0, 1}, kadane(new int[][] {{1, 2, -1}, {-3, -1, -4}, {1, -5, 2}})));
    }

    public static int[] kadane(int[][] m) {
        int[][] prefixSums = new int[m.length][m[0].length];
        for (int i = 0; i < m[0].length; i++) {
            prefixSums[0][i] = m[0][i];
            for (int j = 1; j < m.length; j++) {
                prefixSums[j][i] = prefixSums[j - 1][i] + m[j][i];
            }
        }

        int maxTop = 0, maxLeft = 0, maxBottom = 0, maxRight = 0, maxSum = m[0][0];
        for (int top = 0; top < m[0].length; top++) {
            for (int bottom = top; bottom < m[0].length; bottom++) {
                int minimum = 0, maximum = 0;
                for (int left = 0, right = 0; right < m.length; right++) {
                    if (top == 0) {
                        maximum += prefixSums[bottom][right];
                    } else {
                        maximum += prefixSums[bottom][right] - prefixSums[top - 1][right];
                    }

                    if (maximum <= minimum) {
                        minimum = maximum;
                        left = right;
                    }

                    if (maximum - minimum > maxSum) {
                        maxTop = top;
                        maxBottom = bottom;
                        maxLeft = left;
                        maxRight = right;
                        maxSum = maximum - minimum;
                    }
                }
            }
        }

        return new int[] { maxSum, maxTop, maxLeft, maxBottom, maxRight };
    }

    // Kadane's algorithm http://en.wikipedia.org/wiki/Maximum_subarray_problem
    public static int[] kadane(int... a) {
        int maxHead = 0, maxTail = 0, maxSum = a[0];
        for (int head = 0, tail = 1, sum = a[0]; tail < a.length; tail++) {
            sum = sum + a[tail];
            if (sum > maxSum) {
                maxHead = head; maxTail = tail; maxSum = sum;
            }

            if (0 > sum) {
                head = tail + 1; sum = 0;
            }
        }

        return new int[] { maxSum, maxHead, maxTail };
    }

    @Test
    public void testFindSmallest() {
        try {
            findSmallest(null);
            Assert.fail("'numbers' must be non-null.");
        } catch (IllegalArgumentException e) {
        }

        try {
            findSmallest(new int[0]);
            Assert.fail("'numbers' must not be empty.");
        } catch (IllegalArgumentException e) {
        }

        Assert.assertEquals(6, findSmallest(new int[] {6}));
        Assert.assertEquals(6, findSmallest(new int[] {6, 7}));
        Assert.assertEquals(6, findSmallest(new int[] {7, 6}));
        Assert.assertEquals(6, findSmallest(new int[] {38, 40, 55, 89, 6, 13, 20, 23, 36}));
        Assert.assertEquals(6, findSmallest(new int[] {6, 13, 20, 23, 36, 38, 40, 55, 89}));
        Assert.assertEquals(6, findSmallest(new int[] {13, 20, 23, 36, 38, 40, 55, 89, 6}));
    }

    public static int findSmallest(int... numbers) {
        if (null == numbers) throw new IllegalArgumentException("'numbers' must be non-null.");
        if (0 == numbers.length) throw new IllegalArgumentException("'numbers' must not be empty.");

        return findSmallest(0, numbers.length - 1, numbers);
    }

    private static int findSmallest(int left, int right, int... numbers) {
        if (right == left) {
            return numbers[left];
        } else {
            int middle = (left + right) / 2;
            if (numbers[right] < numbers[middle]) {
                return findSmallest(middle + 1, right, numbers);
            } else {
                return findSmallest(left, middle, numbers);
            }
        }
    }

    @Test
    public void testToAndFromExcelColumn() {
        Assert.assertEquals("AB", toExcelColumn(28));
        Assert.assertEquals("ABC", toExcelColumn(731));
        Assert.assertEquals(28, fromExcelColumn(toExcelColumn(28)));
        Assert.assertEquals(731, fromExcelColumn(toExcelColumn(731)));
    }

    public static String toExcelColumn(int n) {
        //   26 cases :   A -   Z
        // 26^2 cases :  AA -  ZZ
        // 26^3 cases : AAA - ZZZ

        StringBuilder sb = new StringBuilder();
        int cases = 26;
        for (int k = 0; n > 0; k++) { // k is the level of recursion.
            if (n > cases) {
                n -= cases;
                cases *= 26;
            } else {
                n -= 1; // n falls between 0 and (26^k - 1).
                for (int i = 0; i <= k; i++) {
                    sb.insert(0, (char)(n % 26 + 'A'));
                    n /= 26;
                }
            }
        }

        return sb.toString();
    }

    public static int fromExcelColumn(String s) {
        int columns = 0;
        for (int k = 1, cases = 26; k < s.length(); k++, cases *= 26) {
            columns += cases;
        }

        for (int k = s.length() - 1, cases = 1; k >= 0; k--, cases *= 26) {
            columns += cases * (int)(s.charAt(k) - 'A');
        }

        return columns + 1;
    }

    // Design a card game http://math.hws.edu/javanotes/c5/s4.html
    interface Deck {
        void suffle();
        int cardsLeft();
        Card dealCard() throws IllegalStateException; // if the deck is empty.
    }

    interface Card {
        int getSuit();
        int getValue();
    }

    interface Hand {
        void addCard(Card c);
        boolean removeCard(Card card) throws NoSuchElementException;
        boolean removeCard(int position) throws IndexOutOfBoundsException;
        int getCardCount();
        void clear();
        Card getCard(int position);
        void sortBySuit();
        void sortByValue();
    }

    interface Game {
        int getGamesPlayed();
        int getScoresAchived();
        void setUp(); // new Deck() and shuffle();
        void tearDown(); // 
        Card getCurrentCard();
        Card getNextCard();
    }

    public static void subsets(int n) {
        Boolean a[] = new Boolean[n];
        backtrack(a, 0, n - 1);
    }

    public static boolean isSolution(Boolean a[], int k, int level) {
        return k == level;
    }

    public static void backtrack(Boolean a[], int k, int level) {
        if (isSolution(a, k, level)) {
            System.out.print(Joiner.on(" ,").join(a));
        } else {
            boolean candidates[] = {true, false}; // constructCandidates(a, k, n);
            for (int i = 0; i < candidates.length; i++) {
                a[k] = candidates[i];
                // move
                backtrack(a, k, level);
                // back
            }
        }
    }

    public static void mergeSort(int[] elements) {
        int[] temp = new int[elements.length];
        mergeSort(elements, temp, 0, elements.length - 1);
    }

    private static void mergeSort(int[] input, int[] temp, int left, int right) {
        if (left < right) {
            int center = (left + right) / 2;
            mergeSort(input, temp, left, center);
            mergeSort(input, temp, center + 1, right);
            merge(input, temp, left, center + 1, right);
        }
    }

    private static void merge(int[] input, int[] temp, int left, int right, int rightEnd) {
        int leftEnd = right - 1;
        int position = 0;
        int leftBegin = left;
        while (left <= leftEnd && right <= rightEnd) {
            if (input[left] <= input[right]) {
                temp[position++] = input[left++];
            } else {
                temp[position++] = input[right++];
            }
        }

        while (left <= leftEnd) {
            temp[position++] = input[right++];
        }

        while (right <= rightEnd) {
            temp[position++] = input[right++];
        }

        System.arraycopy(temp, leftBegin, input, leftBegin, rightEnd - leftBegin + 1);
    }

    public static class SNode<T> implements Comparable<SNode<T>> {
        public T item;
        public SNode<T> next;

        public SNode<T> mergeSort(SNode<T> p) {
            if (null == p || null == p.next) return p;
            SNode<T> q = partition(p);
            p = mergeSort(p);
            q = mergeSort(q);
            p = merge(p, q);
            return p;
        }

        public SNode<T> partition(SNode<T> p) {
            SNode<T> p1 = p;
            SNode<T> p2 = p.next;
            while (null != p2 && null != p2.next) {
                p2 = p2.next.next;
                p2 = p1.next;
            }

            SNode<T> q = p1.next;
            p1.next = null;
            return q;
        }

        public SNode<T> merge(SNode<T> p, SNode<T> q) {
            SNode<T> head = null, r = null;
            while (null != p && null != q) {
                SNode<T> next;
                if (p.compareTo(q) < 0) {
                    next = p; p = p.next;
                } else {
                    next = q; q = q.next;
                }

                if (null == head) {
                    head = r = next;
                } else {
                    r.next = next; r = r.next;
                }
            }

            if (null == p) r.next = q;
            if (null == q) r.next = p;
            return head;
        }

        @Override
        public int compareTo(SNode<T> o) {
            return 0;
        }
    }

    public static class DNode<T> {
        public T item;
        public DNode<T> next;
        public DNode<T> prev;

        public static <T> DNode<T> reverse(DNode<T> current) {
            if (null == current) return current;

            while (null != current.next) {
                DNode<T> save = current.next;
                current.next = current.prev;
                current.prev = save;
                current = save;
            }

            return current;
        }
    }

    public static class BNode<T> {
        public T item;
        public BNode<T> left;
        public BNode<T> right;
        public BNode<T> parent;

        public static <T> void findKthLargest(BNode<T> current, int k) {
            if (null == current || k == 0) return;

            findKthLargest(current.right, k);
            if (0 == --k) {
                System.out.print(current.item);
            }

            findKthLargest(current.left, k);
        }

        public static <T> List<BNode<T>> yieldInorder(BNode<T> current) {
            if (null ==  current) return ImmutableList.of();

            List<BNode<T>> output = new ArrayList<BNode<T>>();
            if (null != current.left) output.addAll(yieldInorder(current.left));
            output.add(current);
            if (null != current.right) output.addAll(yieldInorder(current.right));
            return output;
        }

        public static <T> List<BNode<T>> yieldInorderFast(BNode<T> current) {
            List<BNode<T>> output = new ArrayList<BNode<T>>();
            Stack<BNode<T>> stack = new Stack<BNode<T>>();

            while (null != current || !stack.isEmpty()) {
                if (null == current) {
                    current = stack.pop();
                    output.add(current);
                    current = current.right;
                } else {
                    stack.push(current);
                    current = current.left;
                }
            }

            return output;
        }

        public static <T> List<BNode<T>> yieldPreorder(BNode<T> current) {
            List<BNode<T>> output = new ArrayList<BNode<T>>();
            Stack<BNode<T>> stack = new Stack<BNode<T>>();

            while (null != current || !stack.isEmpty()) {
                if (null == current) {
                    current = stack.pop();
                } else {
                    output.add(current);
                    stack.push(current.right);
                    current = current.left;
                }
            }

            return output;
        }

        public static <T> List<BNode<T>> yieldPostorder(BNode<T> current) {
            List<BNode<T>> output = new ArrayList<BNode<T>>();
            Stack<BNode<T>> stack = new Stack<BNode<T>>();

            while (null != current || !stack.isEmpty()) {
                if (null == current) {
                    while (!stack.isEmpty() && null == stack.peek().right) {
                        current = stack.pop();
                        output.add(current);
                    }

                    current = stack.isEmpty() ? null : stack.peek().right;
                } else {
                    stack.push(current);
                    current = current.left;
                }
            }

            return output;
        }

        public static <T> int findHeight(BNode<T> current) {
            return null == current ? -1 : 1 + Math.max(findHeight(current.left), findHeight(current.right));
        }

        public static <T> int findDistance(BNode<T> lhs, BNode<T> rhs) {
            Map<BNode<T>, Integer> map = new HashMap<BNode<T>, Integer>();
            for (int i = 0; null != lhs; lhs = lhs.parent) {
                map.put(lhs, i++);
            }

            for (int i = 0; null != rhs; rhs = rhs.parent) {
                if (map.containsKey(rhs)) return i + map.get(rhs);
                i++;
            }

            return -1; // disjoint
        }
    }

    public static void knuthShuffle(List<Integer> list) {
        Random random = new Random();
        for (int i = 0; i < list.size(); i++) {
            int j = i + random.nextInt(list.size() - i);
            Integer save = list.get(j);
            list.set(j, list.get(i));
            list.set(i, save);
        }
    }

    public static int rand7() {
        int rand21;
        do { 
            rand21 = 5 * (rand5() - 1); // 0, 5, ..., 20.
            rand21 += rand5(); // 1, 2, ..., 5.
        } while (rand21 > 21);

        assert rand21 >= 1 && rand21 <= 21;
        return rand21 % 7 + 1; // 1, 2, ..., 7
    }

    public boolean matchesWildcard(String pattern, String input) {
        if (pattern.equals(input)) return true;

        int i = 0, j = 0;
        for (; i < pattern.length() && j < input.length(); i++, j++) {
            // recursive cases
            if ('*' == pattern.charAt(i)) {
                return matchesWildcard(pattern.substring(i), input.substring(j + 1))
                       || matchesWildcard(pattern.substring(i + 1), input.substring(j + 1))
                       || matchesWildcard(pattern.substring(i + 1), input.substring(j));
            }

            if ('?' != pattern.charAt(i) && pattern.charAt(i) != input.charAt(j)) return false;
        }

        if (i == pattern.length() && j == input.length()) return true;
        if ('*' == pattern.charAt(i)) return true;
        
        return false;
    }

    public boolean bracketsMatching(String s) {
        Stack<Character> brackets = new Stack<Character>();
        for (int i = 0; i < s.length(); i++) {
            Character c = s.charAt(i);
            if ('(' == c) brackets.push(')');
            else if ('[' == c) brackets.push(']');
            else if ('{' == c) brackets.push('}');
            else {
                if (')' != c && ']' != c && '}' != c) continue;
                if (brackets.isEmpty() || brackets.peek() != c) return false;
                brackets.pop();
            }
        }

        return brackets.empty();
    }

    public static boolean isPowerOf2(int x) {
        return (0 == (x & x - 1)) && (x > 0);
    }

    public static int rand5() {
        return new Random().nextInt() % 5 + 1;
    }

    /*
     * Positive test cases:
     * - {1} yields {1}.
     * - {1, 2, 2} yields {2}.
     * - {1, 2, 2, 3, 3, 3} yields {3}.
     * - {2, 2} yields {2}.
     * - {1, 1, 2, 2} yields {1, 2}.

     * Negative test cases:
     * - null throws NPE. 
     * - empty yields empty.
     */
    @Test
    public void testFindModesUsingMap() {
        testFindModes(new Function<int[], List<Integer>>() {
            public List<Integer> apply(int[] input) { return findModesUsingMap(input); };
        });
    }

    @Test
    public void testFindModesUsingArray() {
        testFindModes(new Function<int[], List<Integer>>() {
            public List<Integer> apply(int[] input) { return findModesUsingArray(input); };
        });
    }

    public void testFindModes(Function<int[], List<Integer>> findModes) {
        try {
            findModes.apply(null);
            Assert.fail("findsModes should have thrown NPE.");
        } catch (AssertionError ae) {
        } catch (NullPointerException npe) {
        } catch (Exception e) {
            Assert.fail("findsModes should have thrown NPE.");
        }

        Assert.assertEquals(ImmutableList.of(), findModes.apply(new int[0]));

        Assert.assertEquals(ImmutableList.of(1), findModes.apply(new int[] {1}));
        Assert.assertEquals(ImmutableList.of(2), findModes.apply(new int[] {1, 2, 2}));
        Assert.assertEquals(ImmutableList.of(3), findModes.apply(new int[] {1, 2, 2, 3, 3, 3}));

        Assert.assertEquals(ImmutableList.of(2), findModes.apply(new int[] {2, 2}));
        Assert.assertEquals(ImmutableList.of(1, 2), findModes.apply(new int[] {1, 1, 2, 2}));
    }

    public static List<Integer> findModesUsingMap(int... numbers) {
        // We assert preconditions; arguments according to assumptions (for debug builds).
        assert null != numbers;

        // We validate arguments if this is a public interface (for debug and retail builds).
        if (null == numbers) {
            throw new NullPointerException("'numbers' must be non-null.");
        }

        // We short-circuit known cases such as an empty set of numbers.
        if (0 == numbers.length) {
            return ImmutableList.of();
        }

        int maximumHits = 1;
        List<Integer> modes = new ArrayList<Integer>();
        Map<Integer, Integer> hitsByNumber = new HashMap<Integer, Integer>();
        for (int number : numbers) {
            if (hitsByNumber.containsKey(number)) {
                hitsByNumber.put(number, 1 + hitsByNumber.get(number));
            } else {
                hitsByNumber.put(number, 1);
            }

            if (hitsByNumber.get(number) >= maximumHits) {
                if (hitsByNumber.get(number) > maximumHits) {
                    maximumHits = hitsByNumber.get(number);
                    modes.clear();
                }

                modes.add(number);
            } 
        }

        // We assert postconditions.
        assert !modes.isEmpty();
        return modes;
    }

    public static List<Integer> findModesUsingArray(int... numbers) {
        // We assert preconditions; arguments according to assumptions (for debug builds).
        assert null != numbers;

        // We validate arguments if this is a public interface (for debug and retail builds).
        if (null == numbers) {
            throw new NullPointerException("'numbers' must be non-null.");
        }

        // We short-circuit known cases such as an empty set of numbers.
        if (0 == numbers.length) {
            return ImmutableList.of();
        }

        int min = numbers[0];
        int max = numbers[0];
        for (int i = 1; i < numbers.length; i++) {
            min = Math.min(min, numbers[i]); // 1, 2, 2, 3, 3, 3
            max = Math.max(max, numbers[i]);
        }

        int maximumHits = 1;
        List<Integer> modes = new ArrayList<Integer>();

        int[] hitsByNumber = new int[max - min + 1]; // 11, 10..20 -> 10 rooms
        for (int number : numbers) {
            hitsByNumber[number - min]++; // {0, 0, ... } => {0, 1, ... }
            if (hitsByNumber[number - min] >= maximumHits) {
                if (hitsByNumber[number - min] > maximumHits) {
                    modes.clear();
                    maximumHits = hitsByNumber[number - min];
                }

                modes.add(number);
            }
        }

        return modes;
    }
}