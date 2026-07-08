using System.Collections.Generic;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class InHouseState : GhostState
    {
        private static readonly List<CellType> AllowedCells = new() { CellType.AiSpawnZone, CellType.Empty };

        public InHouseState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override void OnEnter() => GhostBlackboard.Heading = Direction.Up;

        public override void Update()
        {
            if (GhostBlackboard.Ghost.IsMoving)
                return;

            Vector2Int target = GhostBlackboard.CurrentGridPos + GhostBlackboard.Heading.ToVector2Int();

            if (!GhostBlackboard.GridData.IsCellMovable(target, AllowedCells))
            {
                GhostBlackboard.Heading = GhostBlackboard.Heading.Reverse();
                target = GhostBlackboard.CurrentGridPos + GhostBlackboard.Heading.ToVector2Int();

                if (!GhostBlackboard.GridData.IsCellMovable(target, AllowedCells))
                    return;
            }

            GhostBlackboard.Ghost.MoveTo(target);
        }
    }
}