using CMP.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace CMP.Tests.EditMode
{
    public class GridMoverTests
    {
        private const float MoveDuration = 0.25f;
        private const float Overshoot = 0.05f;

        private GameObject _go;

        [SetUp]
        public void SetUp() => _go = new GameObject();

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(_go);

        [Test]
        public void Constructor_PlacesTransformAtStartCell()
        {
            var startCell = new Vector2Int(2, 3);
            var mover = new GridMover(_go.transform, startCell);

            Assert.AreEqual(new Vector3(2, 3, 0), _go.transform.position);
            Assert.AreEqual(startCell, mover.CurrentCell);
            Assert.IsFalse(mover.IsMoving);
        }

        [Test]
        public void Tick_PastDuration_SnapsToTargetAndStops()
        {
            var mover = new GridMover(_go.transform, Vector2Int.zero);

            mover.BeginMove(Vector2Int.right, MoveDuration);
            mover.Tick(MoveDuration + Overshoot);

            Assert.AreEqual(new Vector3(1, 0, 0), _go.transform.position);
            Assert.IsFalse(mover.IsMoving);
        }

        [Test]
        public void BeginMove_CarriesOvershootIntoNextMove()
        {
            var mover = new GridMover(_go.transform, Vector2Int.zero);

            mover.BeginMove(Vector2Int.right, MoveDuration);
            mover.Tick(MoveDuration + Overshoot);
            mover.BeginMove(new Vector2Int(2, 0), MoveDuration);

            // yeni hareket t = Overshoot/MoveDuration = 0.2'den başlar → x = 1.2
            Assert.AreEqual(1.2f, _go.transform.position.x, 1e-4f);
        }
    }
}