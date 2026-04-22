using System;

// ===== 单链表结点定义 =====
class Node
{
    public char Data;
    public Node? Next;
    public Node(char data) { Data = data; Next = null; }
}

// ===== 问题1：拆分单链表 =====
class SplitLinkedList
{
    // 创建带头结点的单链表
    public static Node Create(string values)
    {
        Node head = new Node('\0');
        Node tail = head;
        foreach (char c in values)
        {
            tail.Next = new Node(c);
            tail = tail.Next;
        }
        return head;
    }

    // 打印链表（跳过头结点）
    public static void Print(Node head, string name)
    {
        Console.Write($"{name}: ");
        Node? cur = head.Next;
        while (cur != null)
        {
            Console.Write(cur.Data + " ");
            cur = cur.Next;
        }
        Console.WriteLine();
    }

    // 将L拆分为L1(数字)、L2(小写)、L3(大写)
    public static (Node L1, Node L2, Node L3) Split(Node L)
    {
        Node L1 = new Node('\0'), L2 = new Node('\0'), L3 = new Node('\0');
        Node t1 = L1, t2 = L2, t3 = L3;

        Node? cur = L.Next;
        while (cur != null)
        {
            Node? next = cur.Next;
            cur.Next = null;
            if (char.IsDigit(cur.Data))
            {
                t1.Next = cur; t1 = cur;
            }
            else if (char.IsLower(cur.Data))
            {
                t2.Next = cur; t2 = cur;
            }
            else if (char.IsUpper(cur.Data))
            {
                t3.Next = cur; t3 = cur;
            }
            cur = next;
        }
        return (L1, L2, L3);
    }
}

// ===== 问题2：约瑟夫问题（循环链表） =====
class Josephus
{
    class JNode
    {
        public int No;
        public JNode? Next;
        public JNode(int no) { No = no; Next = null; }
    }

    public static void Solve(int n, int m, int start = 1)
    {
        // 建立循环链表，编号1~n
        JNode head = new JNode(1);
        JNode tail = head;
        for (int i = 2; i <= n; i++)
        {
            tail.Next = new JNode(i);
            tail = tail.Next;
        }
        tail.Next = head; // 形成循环

        // 找到起始位置的前一个结点
        JNode prev = tail;
        JNode cur = head;
        // 移动到start位置的前驱
        for (int i = 1; i < start; i++)
        {
            prev = cur;
            cur = cur.Next!;
        }

        Console.Write($"约瑟夫出列顺序 (n={n}, m={m}, start={start}): ");
        int count = n;
        while (count > 0)
        {
            // 从当前位置数m个
            for (int i = 1; i < m; i++)
            {
                prev = cur;
                cur = cur.Next!;
            }
            Console.Write(cur.No + " ");
            prev.Next = cur.Next;
            cur = prev.Next!;
            count--;
        }
        Console.WriteLine();
    }
}

// ===== 主程序 =====
class Program
{
    static void Main()
    {
        // 问题1演示
        Console.WriteLine("===== 问题1：拆分单链表 =====");
        Node L = SplitLinkedList.Create("a1B2cD3eF4");
        SplitLinkedList.Print(L, "原链表L");
        var (L1, L2, L3) = SplitLinkedList.Split(L);
        SplitLinkedList.Print(L1, "L1(数字)");
        SplitLinkedList.Print(L2, "L2(小写)");
        SplitLinkedList.Print(L3, "L3(大写)");

        Console.WriteLine();

        // 问题2演示
        Console.WriteLine("===== 问题2：约瑟夫问题 =====");
        Josephus.Solve(8, 4, 1); // 期望输出: 4 8 5 2 1 3 7 6
    }
}
