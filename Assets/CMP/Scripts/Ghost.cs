using System;
using CMP.Scripts.AiStates;
using UnityEngine;

namespace CMP.Scripts
{
    public enum GhostState
    {
        InHouse,
        JoiningGame,
        Scatter,
        Chase,
    }

    public class Ghost : MonoBehaviour
    {
        #region variables

        private GhostBlackboard _blackboard;
        private AiStates.GhostState _currentState;

        private Vector3 _moveStartWorld;
        private Vector3 _moveTargetWorld;
        private float _moveElapsed;

        public GameObject LeftEye;
        public GameObject RightEye;
        public bool IsMoving { get; private set; }

        #endregion


        public void Init(GridData gridData, Vector2Int spawnGridPos, float joinDelay)
        {
            _blackboard = new GhostBlackboard(this, gridData, spawnGridPos, Direction.Up, joinDelay);
            transform.position = new Vector3(spawnGridPos.x, spawnGridPos.y, 0);
            ChangeState(new InHouseState(_blackboard));
        }

        public void MoveTo(Vector2Int target)
        {
            _blackboard.CurrentGridPos = target;
            _moveStartWorld = transform.position;
            _moveTargetWorld = new Vector3(target.x, target.y, 0);
            _moveElapsed = 0f;
            IsMoving = true;
        }

        private void Update()
        {
            if (IsMoving)
            {
                _moveElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(_moveElapsed / GameSettings.AiMovementDuration);
                transform.position = Vector3.Lerp(_moveStartWorld, _moveTargetWorld, t);
                if (t >= 1f)
                    IsMoving = false;
            }

            _currentState.Update();
        }


        public void ChangeState(AiStates.GhostState newState)
        {
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
}