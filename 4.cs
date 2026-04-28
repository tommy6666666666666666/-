using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== 迷宫求解（非递归 DFS-栈）===");
Console.WriteLine("请输入迷宫行数和列数，例如：5 6");

var (rows, cols) = ReadSize();

Console.WriteLine($"请输入 {rows} 行迷宫数据（可用 0/1 或 ./# 或 S/E）：");
Console.WriteLine("- 可通行：0 或 . 或 S 或 E");
Console.WriteLine("- 障碍物：1 或 #");

var grid = ReadMaze(rows, cols, out var start, out var end);

if (!IsPassable(grid[start.Row, start.Col]) || !IsPassable(grid[end.Row, end.Col]))
{
    Console.WriteLine("起点或终点不可通行，无法求解。\n");
    return;
}

var result = FindPathByStack(grid, start, end);
if (!result.Found)
{
    Console.WriteLine("未找到可行路径。\n");
    return;
}

Console.WriteLine("找到一条可行路径：");
Console.WriteLine(string.Join(" -> ", result.Path.Select(p => $"({p.Row},{p.Col})")));
Console.WriteLine($"路径长度（步数）：{result.Path.Count - 1}\n");

PrintMazeWithPath(grid, result.Path, start, end);

static (int Rows, int Cols) ReadSize()
{
    while (true)
    {
        var line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            continue;
        }

        var parts = line.Split(new[] { ' ', '\t', ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && int.TryParse(parts[0], out var rows) && int.TryParse(parts[1], out var cols) && rows > 0 && cols > 0)
        {
            return (rows, cols);
        }

        Console.WriteLine("输入格式错误，请重新输入行数和列数，例如：5 6");
    }
}

static char[,] ReadMaze(int rows, int cols, out Pos start, out Pos end)
{
    var maze = new char[rows, cols];
    start = new Pos(0, 0);
    end = new Pos(rows - 1, cols - 1);

    Pos? foundS = null;
    Pos? foundE = null;

    for (var r = 0; r < rows; r++)
    {
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == cols)
            {
                for (var c = 0; c < cols; c++)
                {
                    var ch = tokens[c][0];
                    maze[r, c] = ch;
                    if (ch == 'S') foundS = new Pos(r, c);
                    if (ch == 'E') foundE = new Pos(r, c);
                }
                break;
            }

            if (line.Length >= cols)
            {
                for (var c = 0; c < cols; c++)
                {
                    var ch = line[c];
                    maze[r, c] = ch;
                    if (ch == 'S') foundS = new Pos(r, c);
                    if (ch == 'E') foundE = new Pos(r, c);
                }
                break;
            }

            Console.WriteLine($"第 {r + 1} 行长度不足，请重新输入（应有 {cols} 列）。");
        }
    }

    if (foundS.HasValue)
    {
        start = foundS.Value;
    }

    if (foundE.HasValue)
    {
        end = foundE.Value;
    }

    return maze;
}

static (bool Found, List<Pos> Path) FindPathByStack(char[,] maze, Pos start, Pos end)
{
    var rows = maze.GetLength(0);
    var cols = maze.GetLength(1);

    var visited = new bool[rows, cols];
    var parent = new Pos?[rows, cols];
    var stack = new Stack<Pos>();

    stack.Push(start);
    visited[start.Row, start.Col] = true;

    var directions = new[]
    {
        new Pos(-1, 0),
        new Pos(0, 1),
        new Pos(1, 0),
        new Pos(0, -1)
    };

    var found = false;
    while (stack.Count > 0)
    {
        var current = stack.Pop();
        if (current == end)
        {
            found = true;
            break;
        }

        foreach (var d in directions)
        {
            var nr = current.Row + d.Row;
            var nc = current.Col + d.Col;

            if (nr < 0 || nr >= rows || nc < 0 || nc >= cols)
            {
                continue;
            }

            if (visited[nr, nc] || !IsPassable(maze[nr, nc]))
            {
                continue;
            }

            visited[nr, nc] = true;
            parent[nr, nc] = current;
            stack.Push(new Pos(nr, nc));
        }
    }

    if (!found)
    {
        return (false, new List<Pos>());
    }

    var path = new List<Pos>();
    var node = end;
    path.Add(node);

    while (node != start)
    {
        var p = parent[node.Row, node.Col];
        if (!p.HasValue)
        {
            return (false, new List<Pos>());
        }

        node = p.Value;
        path.Add(node);
    }

    path.Reverse();
    return (true, path);
}

static bool IsPassable(char ch) => ch is '0' or '.' or 'S' or 'E';

static void PrintMazeWithPath(char[,] maze, List<Pos> path, Pos start, Pos end)
{
    var rows = maze.GetLength(0);
    var cols = maze.GetLength(1);

    var pathSet = new HashSet<Pos>(path);

    Console.WriteLine("路径可视化（* 为路径，# 为墙，. 为空地）：");
    for (var r = 0; r < rows; r++)
    {
        for (var c = 0; c < cols; c++)
        {
            var p = new Pos(r, c);
            if (p == start)
            {
                Console.Write('S');
            }
            else if (p == end)
            {
                Console.Write('E');
            }
            else if (!IsPassable(maze[r, c]))
            {
                Console.Write('#');
            }
            else if (pathSet.Contains(p))
            {
                Console.Write('*');
            }
            else
            {
                Console.Write('.');
            }
        }

        Console.WriteLine();
    }
}

readonly record struct Pos(int Row, int Col);
