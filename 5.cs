using System;
using System.Collections.Generic;

class Node
{
    public string Val;
    public Node Left;
    public Node Right;
    public Node(string v) { Val = v; }
}

class Program
{
    static void Main()
    {
        // 示例输入（括号表示法）：
        // A(B(D,E),C(,F)) 表示：
        //        A
        //      /   
        //     B     C
        //    / \     
        //   D   E     F
        string sample = "A(B(D,E),C(,F))";
        Console.WriteLine("样例括号表示字符串: " + sample);

        int idx = 0;
        Node root = Parse(sample, ref idx);

        Console.WriteLine("先序遍历: " + string.Join(' ', Preorder(root)));
        Console.WriteLine("中序遍历: " + string.Join(' ', Inorder(root)));
        Console.WriteLine("后序遍历: " + string.Join(' ', Postorder(root)));
        Console.WriteLine("层次遍历: " + string.Join(' ', LevelOrder(root)));

        Console.WriteLine("叶子节点个数: " + CountLeaves(root));
        Console.WriteLine("二叉树深度: " + Depth(root));

        Console.WriteLine();
        Console.WriteLine("算法时间复杂度说明：");
        Console.WriteLine("- 所有遍历（先/中/后/层次）的时间复杂度均为 O(n)，其中 n 为节点数。");
        Console.WriteLine("- 计数叶子节点和求深度均为 O(n)。");

        // 可让用户输入自定义字符串并查看结果（可留空回车结束）
        Console.WriteLine();
        Console.Write("请输入括号表示的二叉树字符串（回车跳过）：");
        string input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            idx = 0;
            try
            {
                var r = Parse(input.Trim(), ref idx);
                Console.WriteLine("先序遍历: " + string.Join(' ', Preorder(r)));
                Console.WriteLine("中序遍历: " + string.Join(' ', Inorder(r)));
                Console.WriteLine("后序遍历: " + string.Join(' ', Postorder(r)));
                Console.WriteLine("层次遍历: " + string.Join(' ', LevelOrder(r)));
                Console.WriteLine("叶子节点个数: " + CountLeaves(r));
                Console.WriteLine("二叉树深度: " + Depth(r));
            }
            catch (Exception ex)
            {
                Console.WriteLine("解析失败：" + ex.Message);
            }
        }
    }

    // 解析形如 A(B(D,E),C(,F)) 的字符串
    static Node Parse(string s, ref int i)
    {
        SkipSpaces(s, ref i);
        if (i >= s.Length) return null;

        // 读取节点值（允许多个字符，直到遇到 '(', ')', ',' ）
        string val = ReadLabel(s, ref i);
        if (string.IsNullOrEmpty(val)) throw new Exception("缺少节点值或格式错误。");
        Node node = new Node(val);

        SkipSpaces(s, ref i);
        if (i < s.Length && s[i] == '(')
        {
            i++; // consume '('
            SkipSpaces(s, ref i);

            // 解析左子树（若直接遇到 ',' 或 ')' 则为 null）
            if (i < s.Length && s[i] != ',' && s[i] != ')')
            {
                node.Left = Parse(s, ref i);
            }

            SkipSpaces(s, ref i);
            // 现在应当遇到 ',' 或 ')'
            if (i < s.Length && s[i] == ',')
            {
                i++; // consume ','
                SkipSpaces(s, ref i);
                if (i < s.Length && s[i] != ')')
                {
                    node.Right = Parse(s, ref i);
                }
            }

            SkipSpaces(s, ref i);
            if (i < s.Length && s[i] == ')')
            {
                i++; // consume ')'
            }
            else
            {
                throw new Exception("缺少右括号。位置=" + i);
            }
        }

        return node;
    }

    static string ReadLabel(string s, ref int i)
    {
        SkipSpaces(s, ref i);
        int start = i;
        while (i < s.Length && s[i] != '(' && s[i] != ')' && s[i] != ',') i++;
        if (i == start) return null;
        return s.Substring(start, i - start).Trim();
    }

    static void SkipSpaces(string s, ref int i)
    {
        while (i < s.Length && char.IsWhiteSpace(s[i])) i++;
    }

    static IEnumerable<string> Preorder(Node root)
    {
        if (root == null) yield break;
        yield return root.Val;
        foreach (var v in Preorder(root.Left)) yield return v;
        foreach (var v in Preorder(root.Right)) yield return v;
    }

    static IEnumerable<string> Inorder(Node root)
    {
        if (root == null) yield break;
        foreach (var v in Inorder(root.Left)) yield return v;
        yield return root.Val;
        foreach (var v in Inorder(root.Right)) yield return v;
    }

    static IEnumerable<string> Postorder(Node root)
    {
        if (root == null) yield break;
        foreach (var v in Postorder(root.Left)) yield return v;
        foreach (var v in Postorder(root.Right)) yield return v;
        yield return root.Val;
    }

    static IEnumerable<string> LevelOrder(Node root)
    {
        var list = new List<string>();
        if (root == null) return list;
        var q = new Queue<Node>();
        q.Enqueue(root);
        while (q.Count > 0)
        {
            var n = q.Dequeue();
            list.Add(n.Val);
            if (n.Left != null) q.Enqueue(n.Left);
            if (n.Right != null) q.Enqueue(n.Right);
        }
        return list;
    }

    static int CountLeaves(Node root)
    {
        if (root == null) return 0;
        if (root.Left == null && root.Right == null) return 1;
        return CountLeaves(root.Left) + CountLeaves(root.Right);
    }

    static int Depth(Node root)
    {
        if (root == null) return 0;
        return Math.Max(Depth(root.Left), Depth(root.Right)) + 1;
    }
}
