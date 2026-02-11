using UnityEngine;
using UnityEngine.EventSystems;
namespace SazenGames.Skeleton
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;
        public Vector3 offset = new Vector3(0, 2, -5);

        [Header("Settings")]
        public float sensitivity = 5.0f;
        public float smoothSpeed = 10.0f;
        public float minPitch = -30f;
        public float maxPitch = 60f;

        private float _currentYaw = 0f;
        private float _currentPitch = 0f;

        void Start()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            HandleInput();
            UpdateCameraPosition();
        }

        void HandleInput()
        {
            if (IsClickingOnUI()) return;

            // Chuột phải để xoay hoặc luôn xoay (tùy chỉnh)
            if (Input.GetMouseButton(1) || Input.touchCount > 0) 
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                // Hỗ trợ cảm ứng cơ bản
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                    {
                        mouseX = touch.deltaPosition.x * 0.1f;
                        mouseY = touch.deltaPosition.y * 0.1f;
                    }
                }

                _currentYaw += mouseX * sensitivity;
                _currentPitch -= mouseY * sensitivity;
                _currentPitch = Mathf.Clamp(_currentPitch, minPitch, maxPitch);
            }
        }

        void UpdateCameraPosition()
        {
            Quaternion rotation = Quaternion.Euler(_currentPitch, _currentYaw, 0);
            Vector3 desiredPosition = target.position + rotation * offset;
            
            // Smooth follow
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            
            // Luôn nhìn vào nhân vật
            transform.LookAt(target.position + Vector3.up * 1.5f); 
        }
        bool IsClickingOnUI()
        {
            if (EventSystem.current == null) return false;

            // PC
            if (Input.GetMouseButton(0))
                return EventSystem.current.IsPointerOverGameObject();

            // Mobile
            if (Input.touchCount > 0)
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

            return false;
        }
    }
}
