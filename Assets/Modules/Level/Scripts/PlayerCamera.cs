using UnityEngine;

namespace FGWorms.Gameplay
{
    public class PlayerCamera : MonoBehaviour
    {
        public Transform FollowTarget { get; private set; }

        public void SetFollowTarget(Transform target, bool allowLook)
        {
            FollowTarget = target;
            _allowLook = allowLook;
        }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _currentLookPosition = transform.position;
            _currentDistance = _distance;
        }

        private void LateUpdate()
        {
            if (_allowLook)
            {
                GetAngles();
                ConstrainAngles();
            }
            UpdateTracking();
        }

        private void GetAngles()
        {
            Vector2 input = new Vector2(
                Input.GetAxis("Mouse Y"),
                Input.GetAxis("Mouse X")
            );

            _lookAngles += _sensitivity * Time.deltaTime * input;
        }

        private void ConstrainAngles()
        {
            _lookAngles.x = Mathf.Clamp(_lookAngles.x, _minYAngle, maxYAngle);

            if (_lookAngles.y < 0f) {
                _lookAngles.y += 360f;
            } else if (_lookAngles.y >= 360f) {
                _lookAngles.y -= 360f;
            }
        }

        private void UpdateTracking()
        {
            Quaternion lookRotation = Quaternion.Euler(_lookAngles);
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = FollowTarget ? FollowTarget.position : _currentLookPosition;

            _currentLookPosition = Vector3.Lerp(_currentLookPosition, lookPosition, _positionSmooth * Time.deltaTime);
            // _currentLookPosition = Vector3.MoveTowards(
            //     _currentLookPosition, lookPosition, _positionSmooth * Time.deltaTime);
            // _currentLookRotation =
            //     Quaternion.Lerp(_currentLookRotation, lookRotation, _rotationSmooth * Time.deltaTime);
            
            transform.SetPositionAndRotation(_currentLookPosition - lookDirection * _currentDistance, lookRotation);
        }

        [Header("Follow Options")]
        [SerializeField]
        private float _distance = 10f;
        [SerializeField]
        private float _positionSmooth = 16f;
        [Header("Look Options")]
        [SerializeField]
        private Vector2 _sensitivity = new(500, 800);
        [SerializeField, Range(-89f, 89f)]
        private float _minYAngle = -30f, maxYAngle = 89f;

        private Vector2 _lookAngles = new(45, 0);
        private Vector3 _currentLookPosition;
        private Quaternion _currentLookRotation;
        private float _currentDistance;
        private bool _allowLook;
    }
}