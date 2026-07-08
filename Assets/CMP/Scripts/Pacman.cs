using System.Collections.Generic;
using UnityEngine;

namespace CMP.Scripts
{
    public class Pacman : MonoBehaviour
    {
        #region variables

        #region Visual

        public Animator Animator;
        private const string FailAnimationName = "FailAnimation";

        #endregion


        #region outsource Datas

        private InputManager _inputManager;
        private GridData _gridData;

        #endregion

        #region movement Logic

        private const float MinMoveDuration = 0.0001f;

        private Direction _headingDirection = Direction.None;
        private Vector3 _moveStartWorld;
        private Vector3 _moveTargetWorld;
        private Vector2Int _previousGridPos;
        private Vector2Int _currentGridPos;
        private float _moveElapsed;
        private float _currentMoveDuration;
        private bool _isMoving;

        #endregion

        #endregion


        private static readonly List<CellType> AllowedCells = new()
        {
            CellType.Pacman,
            CellType.Empty,
            CellType.JoinGameCell
        };

        public void Init(InputManager inputManager, GridData gridData)
        {
            _inputManager = inputManager;
            _gridData = gridData;
            _currentGridPos = gridData.GetCoordsOfCellType(CellType.Pacman)[0];
            transform.position = new Vector3(_currentGridPos.x, _currentGridPos.y, 0);
        }


        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            Direction requested = _inputManager.CurrentDirection;
            if (requested != Direction.None && requested == _headingDirection.Reverse())
            {
                //reverse direction case implementation
                _headingDirection = requested;
                _inputManager.ConsumeInput();

                Vector3 previousWorld = new Vector3(_previousGridPos.x, _previousGridPos.y, 0);
                float distance = Vector3.Distance(transform.position, previousWorld);
                float reversalDuration = Mathf.Max(distance * GameSettings.PacmanMovementDuration, MinMoveDuration);
                StartMove(_previousGridPos, reversalDuration);
                return;
            }

            float overshoot = 0f;
            if (_isMoving)
            {
                _moveElapsed += Time.deltaTime;
                if (_moveElapsed < _currentMoveDuration)
                {
                    transform.position = Vector3.Lerp(_moveStartWorld, _moveTargetWorld,
                        _moveElapsed / _currentMoveDuration);
                    return;
                }

                transform.position = _moveTargetWorld;
                overshoot = _moveElapsed - _currentMoveDuration;
                _isMoving = false;
            }

            if (requested != Direction.None)
            {
                Vector2Int requestedTarget = _currentGridPos + requested.ToVector2Int();
                if (_gridData.IsCellMovable(requestedTarget, AllowedCells))
                {
                    _headingDirection = requested;
                    _inputManager.ConsumeInput();
                }
            }

            if (_headingDirection == Direction.None) return;

            Vector2Int target = _currentGridPos + _headingDirection.ToVector2Int();
            if (!_gridData.IsCellMovable(target, AllowedCells)) return;

            StartMove(target, GameSettings.PacmanMovementDuration);
            _moveElapsed = overshoot;
            transform.position = Vector3.Lerp(_moveStartWorld, _moveTargetWorld, _moveElapsed / _currentMoveDuration);
        }

        private void StartMove(Vector2Int target, float duration)
        {
            _previousGridPos = _currentGridPos;
            _currentGridPos = target;
            _moveStartWorld = transform.position;
            _moveTargetWorld = new Vector3(target.x, target.y, 0);
            _moveElapsed = 0f;
            _currentMoveDuration = duration;
            _isMoving = true;
        }

        private void HandleRotation()
        {
            Quaternion rotation = _headingDirection.ToQuaternion();
            if (transform.rotation == rotation) return;
            transform.rotation = rotation;
        }
    }
}