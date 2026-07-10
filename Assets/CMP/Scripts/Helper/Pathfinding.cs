using System;
using System.Collections.Generic;
using UnityEngine;

namespace CMP.Scripts.Helper
{
    public static class Pathfinding
    {
        private static readonly Vector2Int[] Offsets =
            { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down, };

        public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal,
            Func<Vector2Int, bool> isWalkable)
        {
            var frontier = new Queue<Vector2Int>();
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

            frontier.Enqueue(start);
            cameFrom[start] = start;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                if (current == goal)
                    return ReconstructPath(cameFrom, start, goal);

                foreach (var offSet in Offsets)
                {
                    var next = current + offSet;
                    if (cameFrom.ContainsKey(next))
                        continue;

                    if (!isWalkable(next))
                        continue;

                    cameFrom[next] = current;
                    frontier.Enqueue(next);
                }
            }

            return null;
        }

        private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start,
            Vector2Int goal)
        {
            var path = new List<Vector2Int>();
            var current = goal;
            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Reverse();

            return path;
        }
    }
}