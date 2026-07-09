using System.Collections.Generic;
using UnityEngine;

namespace CMP.Scripts.Helper
{
    public static class Pathfinding
    {
        public static List<Vector2Int> FindPath(GridData gridData, Vector2Int start, Vector2Int goal,
            List<CellType> allowedCells)
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

                foreach (var next in current.GetNeighbours())
                {
                    if (cameFrom.ContainsKey(next))
                        continue;

                    if (!gridData.IsCellMovable(next, allowedCells))
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