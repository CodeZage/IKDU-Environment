using System;
using System.Collections;
using Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] [Range(0.0f, 10.0f)] private float movementSpeed = 6.0f;
        [SerializeField] [Range(0.0f, 20.0f)] private float jumpStrength = 10.0f;
        [SerializeField] [Range(0.0f, 5.0f)] private float gravityMultiplier = 2.0f;
        private CharacterController characterController;
        private Vector3 currentDirection = Vector3.zero;
        private bool isJumping;
        private float stickToGroundForce = 10;
        private InputMaster playerInputMaster;

        [Header("Interaction")]
        [SerializeField] [Range(0.0f, 5.0f)] private float interactDistance = 3.0f;
        private Transform playerPickupContainer;
        private Interactable targetInteractable;
        private Text targetUseHintText;
        private Image textBackground;

        [Header("Camera")]
        [SerializeField] [Range(0.0f, 1.0f)] private float mouseSensitivity = 0.2f;
        [SerializeField] private bool lockCursor = true;
        [SerializeField] private CanvasRenderer textRenderer;
        private Camera playerCamera;
        private float cameraPitch;

        [Header("Headbob")]
        [SerializeField] private bool headBobEnabled = true;
        [SerializeField] private float bobAmplitude = 0.07f;
        [SerializeField] private float bobFrequency = 12f;
        private float transitionSpeed = 20.0f;
        private Vector3 restPosition;
        private float timer;

        private bool IsDead { get; set; }
        private Vector3 SpawnPoint { get; set; }

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();
            playerPickupContainer = playerCamera.transform.GetChild(1);
            textRenderer = playerCamera.GetComponentInChildren<Canvas>().transform.GetChild(1).GetComponent<CanvasRenderer>();
            textBackground = textRenderer.GetComponent<Image>();
            targetUseHintText = textRenderer.GetComponentInChildren<Text>();
            playerInputMaster = new InputMaster();

            playerInputMaster.OnFoot.Interact.performed += ctx => Interact();
            playerInputMaster.OnFoot.Jump.performed += ctx => Jump();

            SpawnPoint = transform.position;
            restPosition = playerCamera.transform.localPosition;
        }

        private void Start()
        {
            playerInputMaster.OnFoot.Enable();

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

                Vector3 movement = UpdateMovement(); //Cache value to avoid calling update movement more than once per frame
                characterController.Move(movement * Time.deltaTime);
                if (headBobEnabled) UpdateHeadBob(movement);
            }
        }

        private void OnEnable()
        {
            playerInputMaster.Enable();
        }

        private void OnDisable()
        {
            playerInputMaster.Disable();
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(0.2f);

            IsDead = false;
        }

        private void UpdateMouseLook()
        {
            var mouseDelta = playerInputMaster.OnFoot.Camera.ReadValue<Vector2>();

            cameraPitch -= mouseDelta.y * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

            playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;
            transform.Rotate(Vector3.up * (mouseDelta.x * mouseSensitivity));
        }

        private void UpdateTarget()
        {
            if (Physics.Raycast(
                playerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, playerCamera.nearClipPlane)),
                playerCamera.transform.forward, out var hit, interactDistance))
            {
                if (hit.transform.GetComponent<Interactable>())
                {
                    var newTarget = hit.transform.GetComponent<Interactable>();
                    if (targetInteractable != null && targetInteractable != newTarget)
                        targetInteractable.RemoveTarget();
                    targetInteractable = newTarget;
                    targetInteractable.Target();
                }
                else
                {
                    if (targetInteractable != null) targetInteractable.RemoveTarget();
                    targetInteractable = null;
                }
            }
            else
            {
                if (targetInteractable != null) targetInteractable.RemoveTarget();
                targetInteractable = null;
            }

            UpdateInfoText();
        }

        private void UpdateInfoText()
        {
            textRenderer.gameObject.SetActive(false);

            if (targetInteractable)
            {
                textRenderer.gameObject.SetActive(true);
                targetUseHintText.text = targetInteractable.interactInfo;
                textBackground.rectTransform.sizeDelta = new Vector2(targetUseHintText.text.Length * 13, 30);
            }
            else
            {
                targetUseHintText.text = "";
            }
        }

        private Vector3 UpdateMovement()
        {
            var inputValues = playerInputMaster.OnFoot.Move.ReadValue<Vector2>();

            var targetDirection = new Vector3(inputValues.x, inputValues.y, 0.0f);
            targetDirection.Normalize();

            // Always move along the camera forward as it is the direction that it being aimed at
            var transform1 = transform;
            var desiredMove = transform1.forward * targetDirection.y + transform1.right * targetDirection.x;

            // Get a normal for the surface that is being touched to move along it
            Physics.SphereCast(transform1.position, characterController.radius, Vector3.down, out var hitInfo,
                characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            currentDirection.x = desiredMove.x * movementSpeed;
            currentDirection.z = desiredMove.z * movementSpeed;

            if (characterController.isGrounded)
            {
                currentDirection.y = -stickToGroundForce;

                if (isJumping) currentDirection.y = jumpStrength;
                isJumping = false;
            }
            else
            {
                currentDirection += Physics.gravity * (gravityMultiplier * Time.deltaTime);
            }

            return currentDirection;
        }

        private void UpdateHeadBob(Vector3 move)
        {
            if (!characterController.isGrounded) return;

            if (Mathf.Abs(move.x) > 0.1f || Mathf.Abs(move.z) > 0.1f)
            {
                timer += bobFrequency * Time.deltaTime;

                playerCamera.transform.localPosition = new Vector3(
                    Mathf.Lerp(playerCamera.transform.localPosition.x, Mathf.Cos(timer / 2) * bobAmplitude, transitionSpeed),
                    Mathf.Lerp(playerCamera.transform.localPosition.y, restPosition.y + Mathf.Sin(timer) * bobAmplitude, transitionSpeed),
                    playerCamera.transform.localPosition.z
                );
            }
            else
            {
                playerCamera.transform.localPosition = new Vector3(
                    Mathf.Lerp(playerCamera.transform.localPosition.x, restPosition.x, transitionSpeed * Time.deltaTime),
                    Mathf.Lerp(playerCamera.transform.localPosition.y, restPosition.y, transitionSpeed * Time.deltaTime),
                    restPosition.z
                );
            }
        }

        private void Jump()
        {
            isJumping = true;
        }

        private void Interact()
        {
            if (playerPickupContainer.childCount > 0)
                playerPickupContainer.GetComponentInChildren<Pickup>().Drop();

            else if (targetInteractable != null) targetInteractable.Interact();
        }
    }
}