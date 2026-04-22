using System;

class SeqList
{
    private readonly char[] data;

    public int Length { get; private set; }

    public SeqList(int capacity)
    {
        data = new char[capacity];
    }

    public void Create(char[] source)
    {
        Array.Copy(source, data, source.Length);
        Length = source.Length;
    }

    public void RemoveRange(char minValue, char maxValue)
    {
        int k = 0;
        for (int i = 0; i < Length; i++)
        {
            if (data[i] < minValue || data[i] > maxValue)
            {
                data[k++] = data[i];
            }
        }

        Length = k;
    }

    public override string ToString()
    {
        return string.Join(" ", data[..Length]);
    }
}

internal class Program
{
    private static void Main()
    {
        char[] a = ['1', '2', '3', '1', '1', '0', '4', '2', '3', '1', '0', '4', '2'];
        SeqList L = new(a.Length);
        L.Create(a);

        Console.WriteLine("原顺序表 L：" + L);
        L.RemoveRange('2', '3');
        Console.WriteLine("删除值从 '2' 到 '3' 的元素后：" + L);
    }
}