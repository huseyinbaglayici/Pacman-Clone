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

        public GameObject LeftEye;
        public GameObject RightEye;

        private GhostBlackboard _blackboard;
        private AiStates.GhostState _currentState;

        private Vector3 _moveStartWorld;
        private Vector3 _moveTargetWorld;
        private float _moveElapsed;

        private bool _isMoving = false;
        public bool IsMoving => _isMoving;

        #endregion


        public void Init(GridData gridData, Vector2Int spawnGridPos)
        {
            _blackboard = new GhostBlackboard(this, gridData, spawnGridPos, Direction.Up);
            transform.position = new Vector3(spawnGridPos.x, spawnGridPos.y, 0);
            ChangeState(new InHouseState(_blackboard));
        }

        public void MoveTo(Vector2Int target)
        {
            _blackboard.CurrentGridPos = target;
            _moveStartWorld = transform.position;
            _moveTargetWorld = new Vector3(target.x, target.y, 0);
            _moveElapsed = 0f;
            _isMoving = true;
        }

        private void Update()
        {
            if (_isMoving)
            {
                _moveElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(_moveElapsed / GameSettings.AiMovementDuration);
                transform.position = Vector3.Lerp(_moveStartWorld, _moveTargetWorld, t);
                if (t >= 1f)
                    _isMoving = false;
            }

            _currentState.Update();
        }


        private void ChangeState(AiStates.GhostState newState)
        {
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
}