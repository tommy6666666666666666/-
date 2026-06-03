"""图的遍历算法及其应用

实验内容：请自行构建一个有8个节点、大于等于15条边的无向图，
使用邻接表存储，并实现图的深度优先遍历和广度优先遍历。

说明：
- 存储结构：邻接表（字典 + 列表）
- 深度优先遍历（DFS）时间复杂度：O(V + E)
- 广度优先遍历（BFS）时间复杂度：O(V + E)
- 改进建议：如果需要处理稠密图，可改用邻接矩阵；
  需要求最短路径时可扩展为 BFS + 距离数组或 Dijkstra 算法。
"""

from collections import deque


class Graph:
    def __init__(self):
        self.adj = {}

    def add_vertex(self, vertex):
        if vertex not in self.adj:
            self.adj[vertex] = []

    def add_edge(self, u, v):
        self.add_vertex(u)
        self.add_vertex(v)
        if v not in self.adj[u]:
            self.adj[u].append(v)
        if u not in self.adj[v]:
            self.adj[v].append(u)

    def display(self):
        print('图的邻接表表示：')
        for vertex in sorted(self.adj):
            neighbors = ' '.join(sorted(self.adj[vertex]))
            print(f'  {vertex}: {neighbors}')
        print()

    def dfs(self, start):
        visited = set()
        order = []

        def _dfs(v):
            visited.add(v)
            order.append(v)
            for neighbor in sorted(self.adj[v]):
                if neighbor not in visited:
                    _dfs(neighbor)

        if start in self.adj:
            _dfs(start)
        return order

    def bfs(self, start):
        visited = {start}
        queue = deque([start])
        order = []
        while queue:
            v = queue.popleft()
            order.append(v)
            for neighbor in sorted(self.adj[v]):
                if neighbor not in visited:
                    visited.add(neighbor)
                    queue.append(neighbor)
        return order


def build_sample_graph():
    graph = Graph()
    edges = [
        ('A', 'B'),
        ('A', 'C'),
        ('A', 'D'),
        ('A', 'H'),
        ('B', 'C'),
        ('B', 'E'),
        ('B', 'F'),
        ('C', 'G'),
        ('C', 'D'),
        ('D', 'F'),
        ('D', 'H'),
        ('E', 'F'),
        ('E', 'H'),
        ('F', 'G'),
        ('G', 'H'),
        ('E', 'G'),
    ]
    for u, v in edges:
        graph.add_edge(u, v)
    return graph


def main():
    graph = build_sample_graph()
    print('实验名称：图的遍历算法及其应用')
    print('顶点数：8，边数：16（无向图）\n')

    graph.display()

    start_vertex = 'A'
    dfs_order = graph.dfs(start_vertex)
    bfs_order = graph.bfs(start_vertex)

    print('测试起点：', start_vertex)
    print('深度优先遍历结果（DFS）：', ' -> '.join(dfs_order))
    print('广度优先遍历结果（BFS）：', ' -> '.join(bfs_order))
    print()

    print('测试数据说明：')
    print('  节点集合：A B C D E F G H')
    print('  边集合：', ', '.join([f'{u}-{v}' for u, v in [
        ('A', 'B'), ('A', 'C'), ('A', 'D'), ('A', 'H'),
        ('B', 'C'), ('B', 'E'), ('B', 'F'),
        ('C', 'G'), ('C', 'D'), ('D', 'F'),
        ('D', 'H'), ('E', 'F'), ('E', 'H'),
        ('F', 'G'), ('G', 'H'), ('E', 'G'),
    ]]))
    print()

    print('算法时间复杂度：')
    print('  邻接表存储：空间复杂度 O(V + E)')
    print('  DFS：时间复杂度 O(V + E)')
    print('  BFS：时间复杂度 O(V + E)')
    print()

    print('改进建议：')
    print('  1. 若图为稠密图，可改用邻接矩阵实现，快速判断边是否存在。')
    print('  2. 若需要最短路径，可以在 BFS 基础上增加距离数组，或使用 Dijkstra/Prim/Kruskal。')


if __name__ == '__main__':
    main()
