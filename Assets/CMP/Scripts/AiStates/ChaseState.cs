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

            Vector2Int pos = GhostBlackboard.CurrentGridPos;
            Direction heading = GhostBlackboard.Heading;

            if (!IsAtCorner(pos, heading))
            {
                Vector2Int forward = pos + heading.ToVector2Int();
                if (GhostBlackboard.GridData.IsMovable(forward))
                {
                    GhostBlackboard.Ghost.MoveTo(forward);
                    return;
                }
            }

            Vector2Int goal = GhostBlackboard.GameManager.Pacman.CurrentGridPos;
            var path = Pathfinding.FindPath(pos, goal, _isWalkable);
            if (path == null || path.Count == 0)
                return;

            Vector2Int next = path[0];
            GhostBlackboard.Heading = (next - pos).ToDirection();
            GhostBlackboard.Ghost.MoveTo(next);
        }

        private bool IsAtCorner(Vector2Int pos, Direction heading)
        {
            Direction reverse = heading.Reverse();
            foreach (var dir in GameSettings.DirectionsToCheck)
            {
                if (dir == heading || dir == reverse)
                    continue;

                if (GhostBlackboard.GridData.IsMovable(pos + dir.ToVector2Int()))
                    return true;
            }

            return false;
        }
    }
}