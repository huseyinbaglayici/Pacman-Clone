using UnityEngine;

namespace CMP.Scripts.AiStates
{
    public class GhostBlackboard
    {
        public Ghost Ghost;
        public GridData GridData;
        public Vector2Int CurrentGridPos;
        public float JoinDelay;
        public Direction Heading;

        public GhostBlackboard(Ghost ghost, GridData gridData, Vector2Int currentGridPos, Direction heading,
            float joinDelay)
        {
            Ghost = ghost;
            GridData = gridData;
            CurrentGridPos = currentGridPos;
            Heading = heading;
            JoinDelay = joinDelay;
        }
    }
}