using CMP.Scripts.AiStates;
using UnityEngine;

namespace CMP.Scripts
{
    public class Ghost : MonoBehaviour
    {
        #region variables

        private GhostBlackboard _blackboard;
        private GhostState _currentState;
        private GridMover _mover;

        public Vector2Int CurrentGridPos => _mover.CurrentCell;
        public GhostStateType State => _currentState.Type;
        public Direction Heading => _blackboard.Heading;
        public bool IsMoving => _mover.IsMoving;

        #endregion


        public void Init(GridData gridData, Vector2Int spawnGridPos, float joinDelay, GameManager gameManager)
        {
            _mover = new GridMover(transform, spawnGridPos);
            _blackboard = new GhostBlackboard(this, gridData, Direction.Up, joinDelay, gameManager);
            ChangeState(new InHouseState(_blackboard));
        }

        public void MoveTo(Vector2Int target) => _mover.BeginMove(target, GameSettings.AiMovementDuration);

        private void Update()
        {
            _mover.Tick(Time.deltaTime);
            _currentState.Update();
        }


        public void ChangeState(GhostState newState)
        {
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
}