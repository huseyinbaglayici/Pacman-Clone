using System.Collections.Generic;
using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class JoiningGameState : GhostState
    {
        private static readonly List<CellType> AllowedCells = new()
        {
            CellType.AiSpawnZone, CellType.Empty, CellType.AiGate, CellType.JoinGameCell
        };

        private List<Vector2Int> _path;
        private int _pathIndex;

        public JoiningGameState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override void OnEnter()
        {
            Vector2Int goal = GhostBlackboard.GridData.GetCoordsOfCellType(CellType.JoinGameCell)[0];
            _path = Pathfinding.FindPath(GhostBlackboard.GridData, GhostBlackboard.CurrentGridPos, goal, AllowedCells);
            _pathIndex = 0;
        }

        public override void Update()
        {
            if (GhostBlackboard.Ghost.IsMoving)
                return;

            if (_path == null || _pathIndex >= _path.Count)
                return; // TODO: add scatter 

            Vector2Int next = _path[_pathIndex];
            _pathIndex++;

            GhostBlackboard.Heading = (next - GhostBlackboard.CurrentGridPos).ToDirection();
            GhostBlackboard.Ghost.MoveTo(next);
        }
    }
}