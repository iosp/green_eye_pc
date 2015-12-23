using System;

namespace GreenEyeAPI.Core.IFS
{
    public class CQuickSort
    {
        public static void Execute(IComparable[] sortArray,
                                   int leftIndex,
                                   int rightIndex)
        {
            int leftPointer = leftIndex;
            int rightPointer = rightIndex;

            // 1. Pick a pivot value somewhere in the middle.
            IComparable pivot = sortArray[(leftIndex + rightIndex) / 2];

            // 2. Loop until pointers meet on the pivot.
            while (leftPointer <= rightPointer)
            {
                // 3. Find a larger value to the right of the pivot.
                //    If there is non we end up at the pivot.
                while (sortArray[leftPointer].CompareTo(pivot) < 0)
                {
                    leftPointer++;
                }

                // 4. Find a smaller value to the left of the pivot.
                //    If there is non we end up at the pivot.
                while (sortArray[rightPointer].CompareTo(pivot) > 0)
                {
                    rightPointer--;
                }

                // 5. Check if both pointers are not on the pivot.
                if (leftPointer <= rightPointer)
                {
                    // 6. Swap both values to the right side.
                    IComparable swap = sortArray[leftPointer];
                    sortArray[leftPointer] = sortArray[rightPointer];
                    sortArray[rightPointer] = swap;

                    leftPointer++;
                    rightPointer--;
                }
            }

            // Here's where the pivot value is in the right spot

            // 7. Recursively call the algorithm on the unsorted array 
            //    to the left of the pivot (if exists).
            if (leftIndex < rightPointer)
            {
                Execute(sortArray, leftIndex, rightPointer);
            }

            // 8. Recursively call the algorithm on the unsorted array 
            //    to the right of the pivot (if exists).
            if (leftPointer < rightIndex)
            {
                Execute(sortArray, leftPointer, rightIndex);
            }

            // 9. The algorithm returns when all sub arrays are sorted.
        }
    }
}
