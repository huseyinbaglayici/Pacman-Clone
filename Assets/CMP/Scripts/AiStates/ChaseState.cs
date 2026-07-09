using System.Collections.Generic;
using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class ChaseState : GhostState
    {
        private static readonly List<CellType> AllowedCells = new()
        {
            CellType.Empty, CellType.JoinGameCell, CellType.Pacman
        };

        public ChaseState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override GhostStateType Type => GhostStateType.Chase;

        public override void OnEnter()
        {
        }

        public override void Update()
        {
            if (GhostBlackboard.Ghost.IsMoving)
                return;

            Vector2Int start = GhostBlackboard.CurrentGridPos;
            Vector2Int goal = GhostBlackboard.GameManager.Pacman.CurrentGridPos;
            var path = Pathfinding.FindPath(GhostBlackboard.GridData, start, goal, AllowedCells);

            if (path == null || path.Count == 0)
                return;

            Vector2Int next = path[0];
            GhostBlackboard.Heading = (next - start).ToDirection();
            GhostBlackboard.Ghost.MoveTo(next);
        }
    }
}