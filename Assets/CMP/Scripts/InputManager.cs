using UnityEngine;
using UnityEngine.UI;

namespace CMP.Scripts
{
    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public class InputManager : MonoBehaviour
    {
        public Button LeftButton;
        public Button RightButton;
        public Button UpButton;
        public Button DownButton;

        public Direction CurrentDirection { get; private set; }

        private void Awake()
        {
            LeftButton.onClick.AddListener(() => { CurrentDirection = Direction.Left; });
            RightButton.onClick.AddListener(() => { CurrentDirection = Direction.Right; });
            UpButton.onClick.AddListener(() => { CurrentDirection = Direction.Up; });
            DownButton.onClick.AddListener(() => { CurrentDirection = Direction.Down; });
        }

        /// <summary>
        /// Call only when the direction is validated and about to be applied.
        /// </summary>
        public Direction ConsumeInput()
        {
            var dir = CurrentDirection;
            CurrentDirection = Direction.None;
            return dir;
        }

        public void Clear() => CurrentDirection = Direction.None;
    }
}