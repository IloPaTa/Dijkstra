using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Пример графа
        var graph = new Dictionary<string, List<(string neighbor, int weight)>>()
        {
            { "A", new List<(string, int)>{ ("B", 1), ("C", 4) } },
            { "B", new List<(string, int)>{ ("C", 2), ("D", 5) } },
            { "C", new List<(string, int)>{ ("D", 1) } },
            { "D", new List<(string, int)>() }
        };

        string startNode = "A";
        var (distances, previousNodes) = Dijkstra(graph, startNode);

        // Восстановление пути
        string targetNode = "D";
        var path = GetPath(previousNodes, startNode, targetNode);

        Console.WriteLine($"Кратчайший путь от {startNode} до {targetNode}: {string.Join(" -> ", path)}");
    }

    static (Dictionary<string, int> distances, Dictionary<string, string> previousNodes) Dijkstra(
        Dictionary<string, List<(string neighbor, int weight)>> graph,
        string startNode)
    {
        var distances = new Dictionary<string, int>();
        var previousNodes = new Dictionary<string, string>();
        var visited = new HashSet<string>();

        foreach (var node in graph.Keys)
        {
            distances[node] = int.MaxValue;
            previousNodes[node] = null; // Изначально у всех узлов нет предшественников
        }

        distances[startNode] = 0;

        while (visited.Count < graph.Count)
        {
            string currentNode = GetMinDistanceNode(distances, visited);
            if (currentNode == null) break; // Все доступные узлы посещены

            visited.Add(currentNode);

            foreach (var (neighbor, weight) in graph[currentNode])
            {
                if (!visited.Contains(neighbor))
                {
                    int newDistance = distances[currentNode] + weight;
                    if (newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        previousNodes[neighbor] = currentNode; // Устанавливаем предшественника
                    }
                }
            }
        }

        return (distances, previousNodes);
    }

    static string GetMinDistanceNode(Dictionary<string, int> distances, HashSet<string> visited)
    {
        string minNode = null;
        int minDistance = int.MaxValue;

        foreach (var node in distances.Keys)
        {
            if (!visited.Contains(node) && distances[node] < minDistance)
            {
                minDistance = distances[node];
                minNode = node;
            }
        }

        return minNode;
    }

    static List<string> GetPath(Dictionary<string, string> previousNodes, string startNode, string targetNode)
    {
        var path = new List<string>();
        for (var at = targetNode; at != null; at = previousNodes[at])
        {
            path.Add(at);
        }

        path.Reverse(); // Путь нужно развернуть, чтобы получить правильный порядок
        return path;
    }
}
