using Mirror;
using UnityEngine;

namespace Code
{
    public class PlayerController : NetworkBehaviour
    {
        [SyncVar] public PlayerState playerState;

        private Vector3 velocity;
        private bool isGrounded;

        private const float movementSpeed = 6.0f;
        private const float jumpHeight = 1.0f;
        private const float gravity = -12f;

        private Animator animator;
        private CameraBoom cameraBoom;
        private CharacterController characterController;
        private GameManager gameManager;
        
        private void Start()
        {
            animator = GetComponentInChildren<Animator>(true);
            cameraBoom = GetComponentInChildren<CameraBoom>(true);
            characterController = GetComponent<CharacterController>();
            gameManager = FindObjectOfType<GameManager>(true);

            if (!gameManager.singlePlayer)
            {
                var camera = GetComponentInChildren<Camera>(true);

                camera.gameObject.SetActive(isLocalPlayer);
            }

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            var pitch = Mathf.Lerp(-60f, 60f, (playerState.pitch + 1f) / 2f); 

            transform.localRotation = Quaternion.Euler(0, playerState.yaw, 0);
            cameraBoom.SetPitch(pitch);
            
            animator.SetFloat("VerticalMovement", playerState.verticalMovement);
            
            if (!isLocalPlayer)
            {
                return;
            }

            var mx = Input.GetAxis("Horizontal");
            var my = Input.GetAxis("Vertical");
            var mouseX = Input.GetAxis("Mouse Y");
            var mouseY = Input.GetAxis("Mouse X");
            
            CameraMovement(mouseX, mouseY);
            Movement(mx, my);
        }

        [Command]
        private void CameraMovement(float pitch, float yaw)
        {
            var pitchChange = pitch * 2f * Time.deltaTime;
            var yawChange = yaw * 140f * Time.deltaTime;
            
            playerState.yaw += yaw;
            playerState.pitch = Mathf.Clamp(playerState.pitch - pitchChange, -1f, 1f);
        }

        [Command]
        private void Movement(float mx, float my)
        {
            playerState.horizontalMovement = mx;
            playerState.verticalMovement = my;

            isGrounded = characterController.isGrounded;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0f;
            }
            
            var movement = new Vector3(mx, 0, my);

            characterController.Move(transform.TransformDirection(movementSpeed * Time.deltaTime * movement));
            
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}