using System.Collections.Generic;
using UnityEngine;

namespace SazenGames.Skeleton
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float runSpeed = 5.0f;
        public float jumpHeight = 1.5f;
        public float gravity = -9.81f;
        public float rotationSpeed = 10.0f;

        [Header("Input Settings")]
        public KeyCode runKey = KeyCode.LeftShift;
        public KeyCode attack1Key = KeyCode.Alpha1;
        public KeyCode attack2Key = KeyCode.Alpha2;
        public KeyCode attack3Key = KeyCode.Alpha3;
        public KeyCode attack4Key = KeyCode.Alpha4;

        // Joystick (Hỗ trợ cả DynamicJoystick, FixedJoystick, VariableJoystick...)
        public Joystick joystick;

        // References
        private CharacterController _controller;
        PlayerHealth _playerHealth;
        private Animator _animator;
        private Transform _cameraTransform;

        // State
        private Vector3 _velocity;
        private bool _isGrounded;
        public bool _isDead = false;
        private string _currentAnimState;
        private float _lockedTill; // Biến dùng để khóa logic chuyển đổi trong thời gian ngắn

        // Animation State Names
        public string ANIM_SPAWN ;
        public string ANIM_DEATH ;
        public string ANIM_REVIVE;
        public string ANIM_IDLE ;
        public string ANIM_RUN ;
        public string ANIM_JUMP ;
        public string ANIM_ATTACK1 ;
        public string ANIM_ATTACK2 ; 
        public string ANIM_ATTACK3 ;
        public string ANIM_ATTACK4 ;
       
        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _playerHealth = GetComponent<PlayerHealth>();
            _animator = GetComponent<Animator>();
            
            if (Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
            }

            // Trigger Spawn animation
            ChangeAnimationState(ANIM_SPAWN);
            LockState(1.0f); // Khóa 1 giây cho Spawn
        }

        void Update()
        {
            // Test Death/Revive - Đưa lên đầu để kiểm tra được ngay cả khi đang chết
          
            if (Input.GetKeyDown(KeyCode.R)) Revive();

            if (_isDead) return;

            // Xử lý tấn công trước (ưu tiên)
            if (HandleAttack()) return;

            // Xử lý di chuyển
            HandleMovement();
        }

        void HandleMovement()
        {
            _isGrounded = _controller.isGrounded;
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            // Lấy input từ Keyboard
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Cộng thêm input từ Joystick nếu có
            if (joystick != null)
            {
                if (Mathf.Abs(joystick.Horizontal) > 0.1f || Mathf.Abs(joystick.Vertical) > 0.1f)
                {
                    moveX += joystick.Horizontal;
                    moveZ += joystick.Vertical;
                }
            }

            Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;

            // Xác định trạng thái di chuyển mong muốn
            string targetAnim = ANIM_IDLE;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
                float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                
                _controller.Move(moveDir.normalized * runSpeed * Time.deltaTime);

                targetAnim = ANIM_RUN;
            }

            // Xử lý Nhảy
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                
              
                _animator.CrossFade(ANIM_JUMP, 0.01f);

                LockState(0.1f); // Khóa ngắn để đảm bảo nhảy bắt đầu
            }
            else if (_isGrounded)
            {
                // Chỉ chuyển về Idle/Run nếu KHÔNG đang bận (Attack/Spawn)
                if (!IsPlayingAction())
                {
                    ChangeAnimationState(targetAnim);
                }
            }

            // Trọng lực
            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
        

        bool HandleAttack()
        {
            // Nếu đang bị khóa (ví dụ đang spam nút hoặc vừa bấm xong), không nhận input mới ngay
            if (Time.time < _lockedTill) return false;

            if ( Input.GetKeyDown(attack1Key))
            {
                ChangeAnimationState(ANIM_ATTACK1);
                LockState(0.25f); // Khóa 0.25s để Animator kịp chuyển sang Attack
                return true;
            }
            if ( Input.GetKeyDown(attack2Key))
            {
                ChangeAnimationState(ANIM_ATTACK2);
                LockState(0.25f);
                return true;
            }
            if ( Input.GetKeyDown(attack3Key))
            {
                ChangeAnimationState(ANIM_ATTACK3);
                LockState(2f); // Khóa 0.25s để Animator kịp chuyển sang Attack
                return true;
            }
            if ( Input.GetKeyDown(attack4Key))
            {
                ChangeAnimationState(ANIM_ATTACK4);
                LockState(1f);
                return true;
            }

            return false;
        }

        public void Die()
        {
            if (_isDead) return;
            _isDead = true;
            ChangeAnimationState(ANIM_DEATH);
            _controller.enabled = false;
        }

        public void Revive()
        {
            if (!_isDead) return;
            _isDead = false;
            ChangeAnimationState(ANIM_REVIVE);
            LockState(1.0f);
            _controller.enabled = true;
            _playerHealth.playerHealth = _playerHealth.totalHealth;
        }

        void ChangeAnimationState(string newState, float transitionTime = 0.1f)
        {
            if (_currentAnimState == newState) return;

            _animator.CrossFade(newState, transitionTime);
            _currentAnimState = newState;
        }

        // Hàm khóa trạng thái trong một khoảng thời gian
        void LockState(float duration)
        {
            _lockedTill = Time.time + duration;
        }

        bool IsPlayingAction()
        {
            // 1. Ưu tiên: Nếu đang trong thời gian khóa (vừa bấm nút), coi như đang hành động
            if (Time.time < _lockedTill) return true;

            // 2. Kiểm tra Animator State thực tế
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            
            if (stateInfo.IsName(ANIM_ATTACK1) || stateInfo.IsName(ANIM_ATTACK2) || stateInfo.IsName(ANIM_SPAWN))
            {
                return stateInfo.normalizedTime < 1.0f; 
            }
            
            if (stateInfo.IsName(ANIM_JUMP))
            {
                return !_isGrounded;
            }

            return false;
        }
    }
}
