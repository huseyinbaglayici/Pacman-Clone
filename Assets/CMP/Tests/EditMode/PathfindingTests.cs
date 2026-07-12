using NUnit.Framework;
using UnityEngine;
using CMP.Scripts.Helper;

namespace CMP.Tests.EditMode
{
    public class PathfindingTests
    {
        // 5x5 acik alan; predicate arama alanini da sinirlar (sinirsiz predicate BFS'i sonsuza surukler)
        private static bool InBounds(Vector2Int c) =>
            c.x >= 0 && c.x < 5 && c.y >= 0 && c.y < 5;

        [Test]
        public void FindPath_StraightLine_ReturnShortestPath()
        {
            var path = Pathfinding.FindPath(new(0, 0), new(2, 0), InBounds);

            Assert.AreEqual(2, path.Count);
            Assert.AreEqual(new Vector2Int(1, 0), path[0]);
            Assert.AreEqual(new Vector2Int(2, 0), path[1]);
        }

        [Test]
        public void FindPath_AroundWall_DetoursCorrectly()
        {
            bool Walkable(Vector2Int c) => InBounds(c) && c != new Vector2Int(1, 0);
            var path = Pathfinding.FindPath(new Vector2Int(0, 0), new Vector2Int(2, 0), Walkable);

            Assert.AreEqual(4, path.Count);
            Assert.IsFalse(path.Contains(new Vector2Int(1, 0)));
            Assert.AreEqual(new Vector2Int(2, 0), path[^1]);
        }

        [Test]
        public void FindPath_UnreachableGoal_ReturnNull()
        {
            bool Walkable(Vector2Int c) => InBounds(c) && Mathf.Abs(c.x - 3) + Mathf.Abs(c.y - 3) != 1;
            var path = Pathfinding.FindPath(new(0, 0), new(3, 3), Walkable);

            Assert.IsNull(path);
        }

        [Test]
        public void FindPath_StartEqualsGoal_ReturnEmptyPath()
        {
            var path = Pathfinding.FindPath(new(2, 2), new(2, 2), InBounds);

            Assert.IsNotNull(path);
            Assert.AreEqual(0, path.Count);
        }
    }
}