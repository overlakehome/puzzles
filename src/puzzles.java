import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import junit.framework.Assert;

import org.junit.Test;

import com.google.common.collect.ImmutableList;

public class puzzles {

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

        try {
            findModesUsingMap(null);
            Assert.fail("findsModes should have thrown NPE.");
        } catch (AssertionError ae) {
        } catch (NullPointerException npe) {
        } catch (Exception e) {
            Assert.fail("findsModes should have thrown NPE.");
        }

        Assert.assertEquals(ImmutableList.of(), findModesUsingMap(new int[0]));

        Assert.assertEquals(ImmutableList.of(1), findModesUsingMap(1));
        Assert.assertEquals(ImmutableList.of(2), findModesUsingMap(1, 2, 2));
        Assert.assertEquals(ImmutableList.of(3), findModesUsingMap(1, 2, 2, 3, 3, 3));

        Assert.assertEquals(ImmutableList.of(2), findModesUsingMap(2, 2));
        Assert.assertEquals(ImmutableList.of(1, 2), findModesUsingMap(1, 1, 2, 2));
    }

    @Test
    public void testFindModesUsingArray() {

        try {
            findModesUsingArray(null);
            Assert.fail("findsModes should have thrown NPE.");
        } catch (AssertionError ae) {
        } catch (NullPointerException npe) {
        } catch (Exception e) {
            Assert.fail("findsModes should have thrown NPE.");
        }

        Assert.assertEquals(ImmutableList.of(), findModesUsingArray(new int[0]));

        Assert.assertEquals(ImmutableList.of(1), findModesUsingArray(1));
        Assert.assertEquals(ImmutableList.of(2), findModesUsingArray(1, 2, 2));
        Assert.assertEquals(ImmutableList.of(3), findModesUsingArray(1, 2, 2, 3, 3, 3));

        Assert.assertEquals(ImmutableList.of(2), findModesUsingArray(2, 2));
        Assert.assertEquals(ImmutableList.of(1, 2), findModesUsingArray(1, 1, 2, 2));
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