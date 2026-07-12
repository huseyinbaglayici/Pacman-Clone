using System.Collections.Generic;
using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class EatenState : GhostState
    {
        private static readonly List<CellType> AllowedCells = new()
        {
            CellType.AiSpawnZone, CellType.Empty, CellType.AiGate, CellType.JoinGameCell, CellType.Pellet,
            CellType.PowerPellet
        };

        private List<Vector2Int> _path;
        private int _pathIndex;

        public EatenState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override GhostStateType Type => GhostStateType.Eaten;

        public override void OnEnter()
        {
            _path = Pathfinding.FindPath(GhostBlackboard.CurrentGridPos, GhostBlackboard.SpawnGridPos,
                cell => GhostBlackboard.GridData.IsCellMovable(cell, AllowedCells));
            _pathIndex = 0;
        }

        public override void Update()
        {
            if (GhostBlackboard.Ghost.IsMoving)
                return;

            if (_path == null || _pathIndex >= _path.Count)
            {
                GhostBlackboard.Ghost.ChangeState(new InHouseState(GhostBlackboard));
                return;
            }

            Vector2Int next = _path[_pathIndex];
            _pathIndex++;

            GhostBlackboard.Heading = (next - GhostBlackboard.CurrentGridPos).ToDirection();
            GhostBlackboard.Ghost.MoveTo(next , GameSettings.EatenMovementDuration);
        }
    }
}