using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class ScatterState : GhostState
    {
        public ScatterState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override GhostStateType Type => GhostStateType.Scatter;

        public override void OnEnter()
        {
        }

        public override void Update()
        {
            if (GhostBlackboard.GameManager.Mode == GameMode.Frightened)
            {
                GhostBlackboard.Ghost.ChangeState(new FrightenedState(GhostBlackboard));
                return;
            }

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

            Direction chosen = DirectionPicker.PickRandom(GhostBlackboard.CurrentGridPos, GhostBlackboard.Heading,
                GhostBlackboard.GridData.IsMovable);
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
    }
}