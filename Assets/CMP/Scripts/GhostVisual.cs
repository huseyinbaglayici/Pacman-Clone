using CMP.Scripts.AiStates;
using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts
{
    [RequireComponent(typeof(Ghost))]
    public class GhostVisual : MonoBehaviour
    {
        private const float BlinkRate = 6f;

        [SerializeField] private GameObject leftEye;
        [SerializeField] private GameObject rightEye;
        [SerializeField] private SpriteRenderer body;
        [SerializeField] private float eyeLookOffset = 0.025f;

        private Ghost _ghost;
        private Vector3 _leftEyeHome, _rightEyeHome;
        private Color _bodyColor;

        private void Awake()
        {
            _ghost = GetComponent<Ghost>();
            _leftEyeHome = leftEye.transform.localPosition;
            _rightEyeHome = rightEye.transform.localPosition;
            _bodyColor = body.color;
        }

        private void LateUpdate()
        {
            Vector2Int dir = _ghost.Heading.ToVector2Int();
            Vector3 offset = new Vector3(dir.x, dir.y, 0) * eyeLookOffset;
            leftEye.transform.localPosition = _leftEyeHome + offset;
            rightEye.transform.localPosition = _rightEyeHome + offset;

            body.color = GetBodyColor();
            body.enabled = _ghost.State != GhostStateType.Eaten;
        }

        private Color GetBodyColor()
        {
            if (_ghost.State != GhostStateType.Frightened)
                return _bodyColor;

            bool ending = _ghost.FrightenedTimeLeft <= GameSettings.FrightenedBlinkWarning;
            bool flash = ending && Mathf.FloorToInt(Time.time * BlinkRate) % 2 == 0;

            return flash ? Color.white : Color.blue;
        }
    }
}