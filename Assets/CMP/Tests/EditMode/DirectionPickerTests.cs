using CMP.Scripts;
using CMP.Scripts.Helper;
using NUnit.Framework;
using UnityEngine;

namespace CMP.Tests.EditMode
{
    public class DirectionPickerTests
    {
        private static bool AllOpen(Vector2Int c) => true;

        [Test]
        public void PickRandom_NeverReturnsReverse_WhenAlternativesExist()
        {
            for (int i = 0; i < 100; i++)
            {
                var chosen = DirectionPicker.PickRandom(Vector2Int.zero, Direction.Right, AllOpen);

                Assert.AreNotEqual(Direction.Left, chosen);
            }
        }

        [Test]
        public void PickRandom_DeadEnd_ReturnsReverse()
        {
            bool Walkable(Vector2Int c) => c == Vector2Int.left;
            var chosen = DirectionPicker.PickRandom(Vector2Int.zero, Direction.Right, Walkable);

            Assert.AreEqual(Direction.Left, chosen);
        }

        [Test]
        public void PickRandom_FullyBlocked_ReturnsNone()
        {
            var chosen = DirectionPicker.PickRandom(Vector2Int.zero, Direction.Right, _ => false);

            Assert.AreEqual(Direction.None, chosen);
        }

        [Test]
        public void PickRandom_OnlyReturnsWalkableDirections()
        {
            // sadece yukari ve asagi acik
            bool Walkable(Vector2Int c) => c == Vector2Int.up || c == Vector2Int.down;
            for (int i = 0; i < 100; i++)
            {
                var chosen = DirectionPicker.PickRandom(Vector2Int.zero, Direction.Right, Walkable);

                Assert.IsTrue(chosen == Direction.Up || chosen == Direction.Down, $"Kapali yon secildi: {chosen}");
            }
        }
    }
}