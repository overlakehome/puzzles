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
    public void testFindModes() {

        try {
            findModes(null);
            Assert.fail("findsModes should have thrown NPE.");
        } catch (NullPointerException npe) {
        } catch (Exception e) {
            Assert.fail("findsModes should have thrown NPE.");
        }

        Assert.assertEquals(ImmutableList.of(), findModes(new int[0]));

        Assert.assertEquals(ImmutableList.of(1), findModes(1));
        Assert.assertEquals(ImmutableList.of(2), findModes(1, 2, 2));
        Assert.assertEquals(ImmutableList.of(3), findModes(1, 2, 2, 3, 3, 3));

        Assert.assertEquals(ImmutableList.of(2), findModes(2, 2));
        Assert.assertEquals(ImmutableList.of(1, 2), findModes(1, 1, 2, 2));
    }

    public static List<Integer> findModes(int... numbers) {
        if (null == numbers) {
            throw new NullPointerException("'numbers' must be non-null.");
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

        return modes;
    }
}