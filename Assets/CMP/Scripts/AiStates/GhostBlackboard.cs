using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class GhostBlackboard
    {
        public Ghost Ghost;
        public GridData GridData;
        public Vector2Int CurrentGridPos => Ghost.CurrentGridPos;
        public float JoinDelay;
        public Direction Heading;

        public GhostBlackboard(Ghost ghost, GridData gridData, Direction heading,
            float joinDelay)
        {
            Ghost = ghost;
            GridData = gridData;
            Heading = heading;
            JoinDelay = joinDelay;
        }
    }
}