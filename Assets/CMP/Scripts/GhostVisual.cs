using UnityEngine;

namespace CMP.Scripts
{
    [RequireComponent(typeof(Ghost))]
    public class GhostVisual : MonoBehaviour
    {
        [SerializeField] private GameObject leftEye;
        [SerializeField] private GameObject rightEye;
        [SerializeField] private float eyeLookOffset = 0.025f;

        private Ghost _ghost;
        private Vector3 _leftEyeHome, _rightEyeHome;

        private void Awake()
        {
            _ghost = GetComponent<Ghost>();
            _leftEyeHome = leftEye.transform.localPosition;
            _rightEyeHome = rightEye.transform.localPosition;
        }

        private void LateUpdate()
        {
            Vector2Int dir = _ghost.Heading.ToVector2Int();
            Vector3 offset = new Vector3(dir.x, dir.y, 0) * eyeLookOffset;
            leftEye.transform.localPosition = _leftEyeHome + offset;
            rightEye.transform.localPosition = _rightEyeHome + offset;
        }
    }
}