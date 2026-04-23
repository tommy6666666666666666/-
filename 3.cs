using System;
using System.Collections.Generic;

class Program
{
    static int[,] maze;
    static int rows, cols;

    // 四个方向：上、下、左、右
    static int[] dr = { -1, 1, 0, 0 };
    static int[] dc = { 0, 0, -1, 1 };
    static string[] dirName = { "上", "下", "左", "右" };

    static void Main()
    {
        Console.WriteLine("请输入迷宫的行数和列数（用空格分隔）：");
        string[] size = Console.ReadLine().Split();
        rows = int.Parse(size[0]);
        cols = int.Parse(size[1]);

        maze = new int[rows, cols];
        Console.WriteLine("请逐行输入迷宫数据（0=通路，1=墙壁），每行数字用空格分隔：");
        for (int i = 0; i < rows; i++)
        {
            string[] line = Console.ReadLine().Split();
            for (int j = 0; j < cols; j++)
                maze[i, j] = int.Parse(line[j]);
        }

        Console.WriteLine("请输入起点坐标（行 列，从0开始）：");
        string[] start = Console.ReadLine().Split();
        int sr = int.Parse(start[0]), sc = int.Parse(start[1]);

        Console.WriteLine("请输入终点坐标（行 列，从0开始）：");
        string[] end = Console.ReadLine().Split();
        int er = int.Parse(end[0]), ec = int.Parse(end[1]);

        var path = SolveMaze(sr, sc, er, ec);

        if (path == null)
        {
            Console.WriteLine("无法找到从起点到终点的路径。");
        }
        else
        {
            Console.WriteLine($"\n找到路径，共 {path.Count} 步：");
            Console.WriteLine("路径坐标序列：");
            foreach (var (r, c) in path)
                Console.Write($"({r},{c}) ");
            Console.WriteLine();
            PrintMazeWithPath(path);
        }
    }

    static List<(int r, int c)> SolveMaze(int sr, int sc, int er, int ec)
    {
        if (maze[sr, sc] == 1 || maze[er, ec] == 1) return null;

        // 栈中存储 (行, 列, 方向索引)，方向索引表示下一个待尝试的方向
        var stack = new Stack<(int r, int c, int dir)>();
        var visited = new bool[rows, cols];
        var parent = new (int r, int c)[rows, cols];

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                parent[i, j] = (-1, -1);

        visited[sr, sc] = true;
        stack.Push((sr, sc, 0));

        while (stack.Count > 0)
        {
            var (r, c, dir) = stack.Pop();

            if (r == er && c == ec)
            {
                // 回溯路径
                var path = new List<(int, int)>();
                int cr = er, cc = ec;
                while (cr != -1)
                {
                    path.Add((cr, cc));
                    var (pr, pc) = parent[cr, cc];
                    cr = pr; cc = pc;
                }
                path.Reverse();
                return path;
            }

            // 尝试下一个方向
            bool pushed = false;
            for (int d = dir; d < 4; d++)
            {
                int nr = r + dr[d];
                int nc = c + dc[d];
                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols
                    && maze[nr, nc] == 0 && !visited[nr, nc])
                {
                    visited[nr, nc] = true;
                    parent[nr, nc] = (r, c);
                    // 将当前节点剩余方向压回栈
                    stack.Push((r, c, d + 1));
                    stack.Push((nr, nc, 0));
                    pushed = true;
                    break;
                }
            }
            // 若所有方向已尝试完，自动回退（不压回当前节点）
        }

        return null;
    }

    static void PrintMazeWithPath(List<(int r, int c)> path)
    {
        var pathSet = new HashSet<(int, int)>(path);
        var start = path[0];
        var end = path[^1];

        Console.WriteLine("\n迷宫路径图（# 墙壁，· 通路，* 路径，S 起点，E 终点）：");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (maze[i, j] == 1)
                    Console.Write("# ");
                else if ((i, j) == start)
                    Console.Write("S ");
                else if ((i, j) == end)
                    Console.Write("E ");
                else if (pathSet.Contains((i, j)))
                    Console.Write("* ");
                else
                    Console.Write("· ");
            }
            Console.WriteLine();
        }
    }
}
