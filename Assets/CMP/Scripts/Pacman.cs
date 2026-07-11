using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts
{
    public class Pacman : MonoBehaviour
    {
        #region variables

        private const string FailAnimationName = "FailAnimation";
        private InputManager _inputManager;
        private GridData _gridData;
        private GridMover _mover;
        private Vector2Int _startCell;
        private Direction _headingDirection = Direction.None;

        public Vector2Int CurrentGridPos => _mover.CurrentCell;

        public Animator Animator;

        #endregion


        public void Init(InputManager inputManager, GridData gridData)
        {
            _inputManager = inputManager;
            _gridData = gridData;
            _startCell = gridData.GetCoordsOfCellType(CellType.Pacman)[0];
            Spawn();
        }

        public void Spawn()
        {
            _mover = new GridMover(transform, _startCell);
            _headingDirection = Direction.None;
            Animator.Rebind();
            enabled = true;
        }


        private void Update()
        {
            _mover.Tick(Time.deltaTime);
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

                float distance = Vector3.Distance(transform.position, _mover.PreviousCell.ToWorld());
                _mover.BeginMove(_mover.PreviousCell, distance * GameSettings.PacmanMovementDuration);
                return;
            }

            if (_mover.IsMoving)
                return;

            if (requested != Direction.None)
            {
                Vector2Int requestedTarget = _mover.CurrentCell + requested.ToVector2Int();
                if (_gridData.IsMovable(requestedTarget))
                {
                    _headingDirection = requested;
                    _inputManager.ConsumeInput();
                }
            }

            if (_headingDirection == Direction.None)
                return;

            Vector2Int target = _mover.CurrentCell + _headingDirection.ToVector2Int();
            if (!_gridData.IsMovable(target))
                return;

            _mover.BeginMove(target, GameSettings.PacmanMovementDuration);
        }

        private void HandleRotation()
        {
            Quaternion rotation = _headingDirection.ToQuaternion();
            if (transform.rotation == rotation) return;
            transform.rotation = rotation;
        }


        public void PlayFail() => Animator.Play(FailAnimationName);
    }
}