using CMP.Scripts;
using CMP.Scripts.Helper;
using NUnit.Framework;

namespace CMP.Tests.EditMode
{
    public class ExtensionsTests
    {
        [TestCase(Direction.Left, Direction.Right)]
        [TestCase(Direction.Right, Direction.Left)]
        [TestCase(Direction.Up, Direction.Down)]
        [TestCase(Direction.Down, Direction.Up)]
        [TestCase(Direction.None, Direction.None)]
        public void Reverse_ReturnOppositeDirection(Direction input, Direction expected)
        {
            Assert.AreEqual(expected, input.Reverse());
        }

        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Down)]
        public void ToVector2Int_ToDirection_Roundtrips(Direction direction)
        {
            Assert.AreEqual(direction, direction.ToVector2Int().ToDirection());
        }

        [TestCase(CellType.Pacman, true)]
        [TestCase(CellType.Empty, true)]
        [TestCase(CellType.JoinGameCell, true)]
        [TestCase(CellType.Wall, false)]
        [TestCase(CellType.AiGate, false)]
        [TestCase(CellType.AiSpawnZone, false)]
        [TestCase(CellType.Invalid, false)]
        [TestCase(CellType.PowerPellet, true)]
        [TestCase(CellType.Pellet, true)]
        public void GetIsMovable_MatchesWalkabilityRules(CellType cell, bool expected)
        {
            Assert.AreEqual(expected, cell.GetIsMovable());
        }
    }
}