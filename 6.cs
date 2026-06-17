using System;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("实验：查找和排序算法过程演示\n");

        // 1) 折半查找过程（有序表 {1,2,3,4,5,6,7,8,9,10} 查找关键字 9）
        int[] ordered = Enumerable.Range(1, 10).ToArray();
        Console.WriteLine("折半查找示例：有序表 {1,2,3,4,5,6,7,8,9,10} 查找关键字 9\n");
        PrintArray(ordered);
        BinarySearchTrace(ordered, 9);

        Console.WriteLine(new string('-', 60));

        // 2) 快速排序过程（对 {6,8,7,9,0,1,3,2,4,5} 输出排序过程）
        int[] unsorted = new int[] { 6, 8, 7, 9, 0, 1, 3, 2, 4, 5 };
        Console.WriteLine("快速排序示例：对序列 {6,8,7,9,0,1,3,2,4,5} 的排序过程\n");
        PrintArray(unsorted);
        QuickSortTrace(unsorted);
        Console.WriteLine("排序完成，结果：");
        PrintArray(unsorted);

        Console.WriteLine();
        Console.WriteLine("备注：折半查找时间复杂度 O(log n)；快速排序平均时间复杂度 O(n log n)，最坏情况 O(n^2)。");
    }

    static void BinarySearchTrace(int[] a, int key)
    {
        int low = 0, high = a.Length - 1;
        int step = 1;
        while (low <= high)
        {
            int mid = (low + high) / 2;
            Console.WriteLine($"步骤 {step++}: low={low}, mid={mid}, high={high}, a[mid]={a[mid]}");
            if (a[mid] == key)
            {
                Console.WriteLine($"找到关键字 {key}，下标为 {mid}\n");
                return;
            }
            else if (a[mid] < key)
            {
                Console.WriteLine($"a[mid] < key ({a[mid]} < {key}) -> 在右半区继续查找\n");
                low = mid + 1;
            }
            else
            {
                Console.WriteLine($"a[mid] > key ({a[mid]} > {key}) -> 在左半区继续查找\n");
                high = mid - 1;
            }
        }
        Console.WriteLine($"未找到关键字 {key}\n");
    }

    static void QuickSortTrace(int[] a)
    {
        QuickSort(a, 0, a.Length - 1);
    }

    static void QuickSort(int[] a, int left, int right)
    {
        if (left < right)
        {
            int p = Partition(a, left, right);
            Console.WriteLine($"分区完成：pivotIndex={p}，当前数组：");
            PrintArray(a);
            if (left < p - 1)
            {
                Console.WriteLine($"对左子区间 [{left}..{p - 1}] 递归快速排序");
                QuickSort(a, left, p - 1);
            }
            if (p + 1 < right)
            {
                Console.WriteLine($"对右子区间 [{p + 1}..{right}] 递归快速排序");
                QuickSort(a, p + 1, right);
            }
        }
    }

    static int Partition(int[] a, int left, int right)
    {
        int pivot = a[right];
        Console.WriteLine($"进行 Partition on [{left}..{right}]，选择 pivot = a[{right}] = {pivot}");
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (a[j] <= pivot)
            {
                i++;
                if (i != j)
                {
                    Swap(a, i, j);
                    Console.WriteLine($"交换 a[{i}] 和 a[{j}] -> ");
                    PrintArray(a);
                }
            }
        }
        if (i + 1 != right)
        {
            Swap(a, i + 1, right);
            Console.WriteLine($"将 pivot 放到正确位置，交换 a[{i + 1}] 和 a[{right}] -> ");
            PrintArray(a);
        }
        return i + 1;
    }

    static void Swap(int[] a, int i, int j)
    {
        int t = a[i];
        a[i] = a[j];
        a[j] = t;
    }

    static void PrintArray(int[] a)
    {
        Console.WriteLine("[" + string.Join(", ", a) + "]");
    }
}
