using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class FrightenedState : GhostState
    {
        public FrightenedState(GhostBlackboard blackboard) : base(blackboard)
        {
        }

        public override GhostStateType Type => GhostStateType.Frightened;

        public override void OnEnter()
        {
        }

        public override void Update()
        {
            if (GhostBlackboard.GameManager.Mode != GameMode.Frightened)
            {
                GhostBlackboard.Ghost.ChangeState(new ScatterState(GhostBlackboard));
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
    }
}