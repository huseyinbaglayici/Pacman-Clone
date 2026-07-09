using UnityEngine;

namespace CMP.Scripts
{
    public class GridMover
    {
        private const float MinDuration = 0.0001f;

        private readonly Transform _transform;
        private Vector3 _startWorld, _targetWorld;
        private float _elapsed, _duration, _overshoot;

        public bool IsMoving { get; private set; }
        public Vector2Int CurrentCell { get; private set; }
        public Vector2Int PreviousCell { get; private set; }

        public GridMover(Transform transform, Vector2Int startCell)
        {
            _transform = transform;
            CurrentCell = startCell;
            PreviousCell = startCell;
            _transform.position = startCell.ToWorld();
        }

        public void BeginMove(Vector2Int target, float duration)
        {
            PreviousCell = CurrentCell;
            CurrentCell = target;
            _startWorld = _transform.position;
            _targetWorld = target.ToWorld();
            _duration = Mathf.Max(duration, MinDuration);
            _elapsed = _overshoot;
            _overshoot = 0f;
            IsMoving = true;
            Apply();
        }

        public void Tick(float deltaTime)
        {
            if (!IsMoving)
            {
                _overshoot = 0f;
                return;
            }

            _elapsed += deltaTime;
            if (_elapsed >= _duration)
            {
                _overshoot = _elapsed - _duration;
                _transform.position = _targetWorld;
                IsMoving = false;
            }
            else
            {
                Apply();
            }
        }

        private void Apply() => _transform.position =
            Vector3.Lerp(_startWorld, _targetWorld, Mathf.Clamp01(_elapsed / _duration));
    }
}