using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using DiasGames.Components;

namespace DiasGames.Controller
{
    public class CatController : MonoBehaviour
    {
        // Components
        private Animator _animator;
        private IMover _mover;

        // Input parameters
        public float walkSpeed = 2f;
        public float runSpeed = 5f;
        private float _currentSpeed;

        // Input Actions
        private Vector2 _moveInput;
        private bool _isRunning;
        private bool _isWalking;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _mover = GetComponent<IMover>();
        }

        private void Update()
        {
            HandleMovement();
            UpdateAnimator();
            
        }

        private void HandleMovement()
        {
            // Check if any movement input is detected
            bool isMoving = _moveInput.sqrMagnitude > 0.01f;

            // Determine if running or walking
            if (isMoving)
            {
                _isRunning = !Keyboard.current.shiftKey.isPressed;
                _isWalking = Keyboard.current.shiftKey.isPressed;
                _currentSpeed = _isRunning ? runSpeed : ( _isWalking ? walkSpeed : 0f);
                _mover.Move(_moveInput, _currentSpeed);
            }
            else
            {
                _currentSpeed = 0f;
                _mover.StopMovement();
                _isRunning = false;
                _isWalking = false;
            }
        }

        private void UpdateAnimator()
        {    
            Debug.Log($"Speed: {_currentSpeed}, IsRunning: {_isRunning}, IsWalking: {_isWalking}");
            _animator.SetFloat("Speed", _currentSpeed);
            _animator.SetBool("IsRunning", _isRunning);
            _animator.SetBool("IsWalking", _isWalking);
        }

        #region Input Receiver

        public void OnMove(Vector2 value)
        {
            _moveInput = value;
        }

        #if ENABLE_INPUT_SYSTEM
        private void OnMove(InputValue value)
        {
            OnMove(value.Get<Vector2>());
        }
        #endif

        #endregion
    }
}
