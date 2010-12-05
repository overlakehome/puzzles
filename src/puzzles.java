import java.util.ArrayList;
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

public class puzzles {
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