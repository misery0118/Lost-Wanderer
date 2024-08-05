using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using DiasGames.Components;

namespace DiasGames.Controller
{
    public class CSPlayerController : MonoBehaviour
    {
        // Components
        private AbilityScheduler _scheduler = null;
        private Health _health = null;
        private IMover _mover;
        private ICapsule _capsule;

        private const float _threshold = 0.01f;

        [SerializeField] private bool hideCursor = true;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;
        [Tooltip("Additional degrees to override the camera. Useful for fine-tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;
        [Tooltip("Speed of camera turn")]
        public Vector2 CameraTurnSpeed = new Vector2(300.0f, 200.0f);
        [Tooltip("For locking the camera position on all axes")]
        public bool LockCameraPosition = false;
        [Tooltip("Camera sensitivity multiplier")]
        [Range(0.1f, 60.0f)] public float CameraSensitivity = 1.0f;

        [Header("Invert Controls")]
        [Tooltip("Invert the vertical camera movement")]
        public bool InvertY = false;
        [Tooltip("Invert the horizontal camera movement")]
        public bool InvertX = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // for shooter UI
        public float CurrentRecoil { get; private set; } = 0f;
        private float recoilReturnVel = 0;

        private bool isAltToggled = false; // Variable to keep track of the toggle state

        private void Awake()
        {
            _scheduler = GetComponent<AbilityScheduler>();
            _health = GetComponent<Health>();
            _mover = GetComponent<IMover>();
            _capsule = GetComponent<ICapsule>();

            if (hideCursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            // set right angle on start for camera
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.eulerAngles.y;
        }

        private void OnEnable()
        {
#if ENABLE_INPUT_SYSTEM
            // subscribe reset action to scheduler to know when to reset actions
            _scheduler.OnUpdatedAbilities += ResetActions;
#endif

            // subscribe for death event
            if (_health != null)
                _health.OnDead += Die;
        }

        private void OnDisable()
        {
#if ENABLE_INPUT_SYSTEM
            // unsubscribe reset action
            _scheduler.OnUpdatedAbilities -= ResetActions;
#endif
            // unsubscribe for death event
            if (_health != null)
                _health.OnDead -= Die;
        }

        private void Update()
        {
            UpdateCharacterActions();

            if (CurrentRecoil > 0)
                CurrentRecoil = Mathf.SmoothDamp(CurrentRecoil, 0, ref recoilReturnVel, 0.2f);

            if (Keyboard.current.altKey.wasPressedThisFrame)
            {
                isAltToggled = !isAltToggled; // Toggle the state
                if (isAltToggled)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    // Stop movement immediately when Alt is pressed
                    Move = Vector2.zero;
                    Look = Vector2.zero;
                    Jump = false;
                    Walk = false;
                    Roll = false;
                    Crouch = false;
                    Crawl = false;
                    Interact = false;
                    Drop = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }

            // Check for pause state and update cursor visibility
            if (PauseMenu.GameIsPaused)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                if (!isAltToggled)
                {
                    Cursor.visible = hideCursor;
                    Cursor.lockState = hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
                }
            }

#if ENABLE_LEGACY_INPUT_MANAGER
            LegacyInput();
#endif
        }

        private void LateUpdate()
        {
            if (!PauseMenu.GameIsPaused && !isAltToggled) // Check if the game is not paused and Alt is not toggled
            {
                Cursor.lockState = CursorLockMode.Locked;
            	Cursor.visible = false;
                CameraRotation();
            }
        }

        private void Die()
        {
            _scheduler.StopScheduler();

            // disable any movement
            _mover.DisableGravity();
            _mover.StopMovement();

            // disable main character collision
            _capsule.DisableCollision();

            // activate root motion
            _mover.ApplyRootMotion(Vector3.one);
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (Look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                float lookX = InvertX ? -Look.x : Look.x;
                float lookY = InvertY ? -Look.y : Look.y;

                _cinemachineTargetYaw += lookX * CameraTurnSpeed.x * Time.deltaTime * CameraSensitivity;
                _cinemachineTargetPitch += lookY * CameraTurnSpeed.y * Time.deltaTime * CameraSensitivity;
            }

            // clamp our rotations so our values are limited to 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void UpdateCharacterActions()
        {
            _scheduler.characterActions.move = Move;
            _scheduler.characterActions.jump = Jump;
            _scheduler.characterActions.walk = Walk;
            _scheduler.characterActions.roll = Roll;
            _scheduler.characterActions.crouch = Crouch;
            _scheduler.characterActions.interact = Interact;
            _scheduler.characterActions.crawl = Crawl;
            _scheduler.characterActions.drop = Drop;

            // weapon
            _scheduler.characterActions.zoom = Zoom;
        }

        #region Input receiver

        [Header("Input")]
        public Vector2 Move = Vector2.zero;
        public Vector2 Look = Vector2.zero;
        public bool Jump = false;
        public bool Walk = false;
        public bool Roll = false;
        public bool Crouch = false;
        public bool Interact = false;
        public bool Crawl = false;
        public bool Zoom = false;
        public bool Drop = false;

        public void ResetActions()
        {
            Jump = false;
            Roll = false;
            Crawl = false;
            Interact = false;
            Drop = false;
        }

        public void LegacyInput()
        {
            if (isAltToggled) return; // Disable all input if Alt is toggled

            Move.x = Input.GetAxis("Horizontal");
            Move.y = Input.GetAxis("Vertical");

            Look.x = Input.GetAxis("Mouse X");
            Look.y = Input.GetAxis("Mouse Y");

            Walk = Input.GetButton("Walk");
            Jump = Input.GetButtonDown("Jump");
            Roll = Input.GetButtonDown("Roll");
            Crouch = Input.GetButton("Crouch");
            Crawl = Input.GetButtonDown("Crawl");
            Zoom = Input.GetButtonDown("Zoom");
            Interact = Input.GetButtonDown("Interact");

            // special actions for climbing
            Drop = Input.GetButtonDown("Drop");

            /*
            // special actions for shooter
            Fire = Input.GetButton("Fire");
            Reload = Input.GetButtonDown("Reload");
            Switch = Input.GetAxisRaw("Switch");
            Toggle = Input.GetButtonDown("Toggle");*/
        }

        public void OnMove(Vector2 value)
        {
            if (!isAltToggled) // Disable move input if Alt is toggled
            {
                Move = value;
            }
            else
            {
                Move = Vector2.zero; // Ensure movement stops when Alt is toggled
            }
        }
        public void OnLook(Vector2 value)
        {
            if (!isAltToggled) // Disable look input if Alt is toggled
            {
                Look = value;
            }
        }
        public void OnJump(bool value)
        {
            if (!isAltToggled) // Disable jump input if Alt is toggled
            {
                Jump = value;
            }
        }
        public void OnWalk(bool value)
        {
            if (!isAltToggled) // Disable walk input if Alt is toggled
            {
                Walk = value;
            }
        }
        public void OnRoll(bool value)
        {
            if (!isAltToggled) // Disable roll input if Alt is toggled
            {
                Roll = value;
            }
        }
        public void OnCrouch(bool value)
        {
            if (!isAltToggled) // Disable crouch input if Alt is toggled
            {
                Crouch = value;
            }
        }
        public void OnCrawl(bool value)
        {
            if (!isAltToggled) // Disable crawl input if Alt is toggled
            {
                Crawl = value;
            }
        }

        public void OnZoom(bool value)
        {
            if (!isAltToggled) // Disable zoom input if Alt is toggled
            {
                Zoom = value;
            }
        }
        public void OnInteract(bool value)
        {
            if (!isAltToggled) // Disable interact input if Alt is toggled
            {
                Interact = value;
            }
        }
        public void OnDrop(bool value)
        {
            if (!isAltToggled) // Disable drop input if Alt is toggled
            {
                Drop = value;
            }
        }

#if ENABLE_INPUT_SYSTEM
        private void OnMove(InputValue value)
        {
            OnMove(value.Get<Vector2>());
        }

        private void OnLook(InputValue value)
        {
            OnLook(value.Get<Vector2>());
        }

        private void OnJump(InputValue value)
        {
            OnJump(value.isPressed);
        }

        private void OnWalk(InputValue value)
        {
            OnWalk(value.isPressed);
        }

        private void OnRoll(InputValue value)
        {
            OnRoll(value.isPressed);
        }

        private void OnCrouch(InputValue value)
        {
            OnCrouch(value.isPressed);
        }

        private void OnCrawl(InputValue value)
        {
            OnCrawl(value.isPressed);
        }

        private void OnZoom(InputValue value)
        {
            OnZoom(value.isPressed);
        }

        private void OnInteract(InputValue value)
        {
            OnInteract(value.isPressed);
        }
        private void OnDrop(InputValue value)
        {
            OnDrop(value.isPressed);
        }

#endif

        #endregion
    }
}
