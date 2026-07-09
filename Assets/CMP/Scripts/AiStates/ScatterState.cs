using System.Collections.Generic;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class ScatterState : GhostState
    {
        private static readonly List<CellType> AllowedCells = new()
        {
            CellType.Empty, CellType.JoinGameCell, CellType.Pacman
        };

        public ScatterState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override GhostStateType Type => GhostStateType.Scatter;

        public override void OnEnter()
        {
        }

        public override void Update()
        {
            if (GhostBlackboard.GameManager.Mode == GameMode.Chase)
            {
                GhostBlackboard.Ghost.ChangeState(new ChaseState(GhostBlackboard));
                return;
            }

            if (HasLineOfSight())
            {
                GhostBlackboard.GameManager.Mode = GameMode.Chase;
                GhostBlackboard.Ghost.ChangeState(new ChaseState(GhostBlackboard));
                return;
            }

            if (GhostBlackboard.Ghost.IsMoving)
                return;

            Direction chosen = PickDirection();
            if (chosen == Direction.None)
                return;

            GhostBlackboard.Heading = chosen;
            Vector2Int target = GhostBlackboard.CurrentGridPos + chosen.ToVector2Int();
            GhostBlackboard.Ghost.MoveTo(target);
        }

        private bool HasLineOfSight()
        {
            Vector2Int dir = GhostBlackboard.Heading.ToVector2Int();
            if (dir == Vector2Int.zero)
                return false;

            Vector2Int cell = GhostBlackboard.CurrentGridPos;
            Vector2Int pacmanCell = GhostBlackboard.GameManager.Pacman.CurrentGridPos;

            for (int i = 0; i < GameSettings.AiSightRange; i++)
            {
                cell += dir;
                if (GhostBlackboard.GridData.GetCellAtOrDefault(cell, CellType.Wall) == CellType.Wall)
                    return false;
                if (cell == pacmanCell)
                    return true;
            }

            return false;
        }

        private Direction PickDirection()
        {
            Direction reverse = GhostBlackboard.Heading.Reverse();
            var candidates = new List<Direction>();

            foreach (var dir in GameSettings.DirectionsToCheck)
            {
                if (dir == reverse)
                    continue;
                Vector2Int cell = GhostBlackboard.CurrentGridPos + dir.ToVector2Int();
                if (GhostBlackboard.GridData.IsCellMovable(cell, AllowedCells))
                    candidates.Add(dir);
            }

            if (candidates.Count > 0)
                return candidates[Random.Range(0, candidates.Count)];

            Vector2Int reverseCell = GhostBlackboard.CurrentGridPos + reverse.ToVector2Int();
            if (GhostBlackboard.GridData.IsCellMovable(reverseCell, AllowedCells))
                return reverse;

            return Direction.None;
        }
    }
}