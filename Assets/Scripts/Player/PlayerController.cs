using System.Collections;
using Interaction;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float movementSpeed = 6.0f;

        [SerializeField] [Range(0.0f, 20.0f)] private float jumpStrength = 10.0f;
        [SerializeField] [Range(0.0f, 5.0f)] private float gravityMultiplier = 2.0f;
        private CharacterController _characterController;
        private Vector3 _currentDirection = Vector3.zero;
        private bool _isJumping;
        private float _stickToGroundForce = 10;
        private InputMaster _playerInputMaster;

        [Header("Interaction")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float interactDistance = 3.0f;

        private Transform _playerPickupContainer;
        private Interactable _targetInteractable;
        private Text _targetUseHintText;
        private Image _textBackground;

        [Header("Camera")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mouseSensitivity = 0.2f;

        [SerializeField] private bool lockCursor = true;
        [SerializeField] private CanvasRenderer textRenderer;
        private Camera _playerCamera;
        private float _cameraPitch;

        [Header("Head-bob")] [SerializeField] private bool headBobEnabled = true;
        [SerializeField] private float bobAmplitude = 0.07f;
        [SerializeField] private float bobFrequency = 12f;
        private float _transitionSpeed = 20.0f;
        private Vector3 _restPosition;
        private float _timer;

        private bool IsDead { get; set; }
        private Vector3 SpawnPoint { get; set; }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerCamera = GetComponentInChildren<Camera>();
            _playerPickupContainer = _playerCamera.transform.GetChild(1);
            textRenderer = _playerCamera.GetComponentInChildren<Canvas>().transform.GetChild(1)
                .GetComponent<CanvasRenderer>();
            _textBackground = textRenderer.GetComponent<Image>();
            _targetUseHintText = textRenderer.GetComponentInChildren<Text>();
            _playerInputMaster = new InputMaster();

            _playerInputMaster.OnFoot.Interact.performed += ctx => Interact();
            _playerInputMaster.OnFoot.AltInteract.performed += ctx => AltInteract();
            _playerInputMaster.OnFoot.Jump.performed += ctx => Jump();

            SpawnPoint = transform.position;
            _restPosition = _playerCamera.transform.localPosition;
        }

        private void Start()
        {
            _playerInputMaster.OnFoot.Enable();

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            if (IsDead)
            {
                transform.position = SpawnPoint;
                StartCoroutine(Respawn());
            }
            else
            {
                UpdateMouseLook();
                UpdateTarget();

                var movement = UpdateMovement(); //Cache value to avoid calling update movement more than once per frame
                _characterController.Move(movement * Time.deltaTime);
                if (headBobEnabled) UpdateHeadBob(movement);
            }
        }

        private void OnEnable()
        {
            _playerInputMaster.Enable();
        }

        private void OnDisable()
        {
            _playerInputMaster.Disable();
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(0.2f);

            IsDead = false;
        }

        private void UpdateMouseLook()
        {
            var mouseDelta = _playerInputMaster.OnFoot.Camera.ReadValue<Vector2>();

            _cameraPitch -= mouseDelta.y * mouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

            _playerCamera.transform.localEulerAngles = Vector3.right * _cameraPitch;
            transform.Rotate(Vector3.up * (mouseDelta.x * mouseSensitivity));
        }

        private void UpdateTarget()
        {
            if (Physics.Raycast(
                _playerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f,
                    _playerCamera.nearClipPlane)),
                _playerCamera.transform.forward, out var hit, interactDistance))
            {
                if (hit.transform.GetComponent<Interactable>())
                {
                    var newTarget = hit.transform.GetComponent<Interactable>();

                    if (!newTarget.isInteractable)
                    {
                        textRenderer.gameObject.SetActive(false);
                        _targetInteractable?.RemoveTarget();
                        _targetInteractable = null;
                        return;
                    }

                    if (_targetInteractable != null && _targetInteractable != newTarget)
                        _targetInteractable.RemoveTarget();

                    _targetInteractable = newTarget;

                    _targetInteractable.Target();
                }
                else
                {
                    if (_targetInteractable != null) _targetInteractable.RemoveTarget();
                    _targetInteractable = null;
                }
            }
            else
            {
                if (_targetInteractable != null) _targetInteractable.RemoveTarget();
                _targetInteractable = null;
            }

            UpdateInfoText();
        }

        private void UpdateInfoText()
        {
            textRenderer.gameObject.SetActive(false);

            if (_targetInteractable)
            {
                textRenderer.gameObject.SetActive(true);
                _targetUseHintText.text = _targetInteractable.interactInfo;
                _textBackground.rectTransform.sizeDelta = new Vector2(_targetUseHintText.text.Length * 13, 30);
            }
            else
            {
                _targetUseHintText.text = "";
            }
        }

        private Vector3 UpdateMovement()
        {
            var inputValues = _playerInputMaster.OnFoot.Move.ReadValue<Vector2>();

            var targetDirection = new Vector3(inputValues.x, inputValues.y, 0.0f);
            targetDirection.Normalize();

            // Always move along the camera forward as it is the direction that it being aimed at
            var transform1 = transform;
            var desiredMove = transform1.forward * targetDirection.y + transform1.right * targetDirection.x;

            // Get a normal for the surface that is being touched to move along it
            Physics.SphereCast(transform1.position, _characterController.radius, Vector3.down, out var hitInfo,
                _characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            _currentDirection.x = desiredMove.x * movementSpeed;
            _currentDirection.z = desiredMove.z * movementSpeed;

            if (_characterController.isGrounded)
            {
                _currentDirection.y = -_stickToGroundForce;

                if (_isJumping) _currentDirection.y = jumpStrength;
                _isJumping = false;
            }
            else
            {
                _currentDirection += Physics.gravity * (gravityMultiplier * Time.deltaTime);
            }

            return _currentDirection;
        }

        private void UpdateHeadBob(Vector3 move)
        {
            if (!_characterController.isGrounded) return;

            if (Mathf.Abs(move.x) > 0.1f || Mathf.Abs(move.z) > 0.1f)
            {
                _timer += bobFrequency * Time.deltaTime;

                var localPosition = _playerCamera.transform.localPosition;
                localPosition = new Vector3(
                    Mathf.Lerp(localPosition.x, Mathf.Cos(_timer / 2) * bobAmplitude, _transitionSpeed),
                    Mathf.Lerp(localPosition.y, _restPosition.y + Mathf.Sin(_timer) * bobAmplitude, _transitionSpeed),
                    localPosition.z
                );
                _playerCamera.transform.localPosition = localPosition;
            }
            else
            {
                var localPosition = _playerCamera.transform.localPosition;
                localPosition = new Vector3(
                    Mathf.Lerp(localPosition.x, _restPosition.x, _transitionSpeed * Time.deltaTime),
                    Mathf.Lerp(localPosition.y, _restPosition.y, _transitionSpeed * Time.deltaTime),
                    _restPosition.z
                );
                _playerCamera.transform.localPosition = localPosition;
            }
        }

        private void Jump()
        {
            _isJumping = true;
        }

        private void Interact()
        {
            if (_playerPickupContainer.childCount > 0)
                _playerPickupContainer.GetComponentInChildren<Pickup>().Drop();

            else if (_targetInteractable != null) _targetInteractable.Interact();
        }

        private void AltInteract()
        {
            (_targetInteractable as IAltInteractable)?.AltInteract();
        }
    }
}
