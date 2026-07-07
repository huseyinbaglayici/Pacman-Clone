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

        private Vector2Int _previousGridPos;
        private Vector2Int _currentGridPos;
        private bool _isMoving;
        private float _moveElapsed;
        private float _currentMoveDuration;
        private Vector3 _moveStartWorld;
        private Vector3 _moveTargetWorld;
        private Direction _headingDirection = Direction.None;

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
            // buffered Input logics
            Direction requested = _inputManager.CurrentDirection;
            if (requested != Direction.None && requested == _headingDirection.Reverse())
            {
                float moveProgress = _isMoving ? Mathf.Clamp01(_moveElapsed / _currentMoveDuration) : 1f;
                _headingDirection = requested;
                _inputManager.ConsumeInput();

                float reversalDuration = Mathf.Max(moveProgress * GameSettings.PacmanMovementDuration, 0.0001f);
                StartMove(_previousGridPos, reversalDuration);
                return;
            }

            if (_isMoving)
            {
                _moveElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(_moveElapsed / _currentMoveDuration);
                transform.position = Vector3.Lerp(_moveStartWorld, _moveTargetWorld, t);

                if (t >= 1f)
                    _isMoving = false;

                return;
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