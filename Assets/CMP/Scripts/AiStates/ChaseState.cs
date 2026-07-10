using System;
using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class ChaseState : GhostState
    {
        private Func<Vector2Int, bool> _isWalkable;

        public ChaseState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override GhostStateType Type => GhostStateType.Chase;

        public override void OnEnter() => _isWalkable = GhostBlackboard.GridData.IsMovable;

        public override void Update()
        {
            if (GhostBlackboard.Ghost.IsMoving)
                return;

            Vector2Int start = GhostBlackboard.CurrentGridPos;
            Vector2Int goal = GhostBlackboard.GameManager.Pacman.CurrentGridPos;
            var path = Pathfinding.FindPath(start, goal, _isWalkable);

            if (path == null || path.Count == 0)
                return;

            Vector2Int next = path[0];
            GhostBlackboard.Heading = (next - start).ToDirection();
            GhostBlackboard.Ghost.MoveTo(next);
        }
    }
}