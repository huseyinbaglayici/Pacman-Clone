using System.Collections.Generic;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class InHouseState : GhostState
    {
        private static readonly List<CellType> AllowedCells = new() { CellType.AiSpawnZone, CellType.Empty };

        private float _elapsed;

        public InHouseState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override GhostStateType Type => GhostStateType.InHouse;
        public override void OnEnter() => GhostBlackboard.Heading = Direction.Up;

        public override void Update()
        {
            _elapsed += Time.deltaTime;

            if (GhostBlackboard.Ghost.IsMoving)
                return;

            if (_elapsed >= GhostBlackboard.JoinDelay)
            {
                GhostBlackboard.Ghost.ChangeState(new JoiningGameState(GhostBlackboard));
                return;
            }

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