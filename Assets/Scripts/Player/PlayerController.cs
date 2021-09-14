using System.Collections;
using Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 10.0f)] private float movementSpeed = 6.0f;
        [SerializeField] private float stickToGroundForce = 10;
        [SerializeField] [Range(0.0f, 20.0f)] private float jumpStrength = 10.0f;
        [SerializeField] [Range(0.0f, 5.0f)] private float gravityMultiplier = 2.0f;
        [SerializeField] [Range(0.0f, 5.0f)] private float interactDistance = 2.0f;
        [SerializeField] private bool lockCursor = true;
        [SerializeField] private CanvasRenderer textRenderer;

        private new Camera camera;
        private float cameraPitch;
        private CharacterController characterController;
        private Vector3 currentDirection = Vector3.zero;
        private bool isJumping;
        private float mouseSensitivity;
        private InputMaster playerInputMaster;
        private Transform playerPickupContainer;
        private Interactable targetInteractable;
        private Text targetUseHintText;
        private Image textBackground;

        private bool IsDead { get; set; }
        private Vector3 SpawnPoint { get; set; }

        private void Awake()
        {
            mouseSensitivity = 0.2f;
            characterController = GetComponent<CharacterController>();
            camera = GetComponentInChildren<Camera>();
            playerPickupContainer = camera.transform.GetChild(1);
            targetUseHintText = camera.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
            textBackground = textRenderer.GetComponent<Image>();
            playerInputMaster = new InputMaster();

            playerInputMaster.OnFoot.Interact.performed += _ => Interact();
            playerInputMaster.OnFoot.Jump.performed += _ => Jump();

            SpawnPoint = transform.position;
        }

        private void Start()
        {
            playerInputMaster.Enable();
            
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
                characterController.Move(UpdateMovement() * Time.deltaTime);
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

        private void SkipScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

            camera.transform.localEulerAngles = Vector3.right * cameraPitch;
            transform.Rotate(Vector3.up * (mouseDelta.x * mouseSensitivity));
        }

        private void UpdateTarget()
        {
            if (Physics.Raycast(
                camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, camera.nearClipPlane)),
                camera.transform.forward, out var hit, interactDistance))
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
                targetUseHintText.text = targetInteractable.UseInfo;
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