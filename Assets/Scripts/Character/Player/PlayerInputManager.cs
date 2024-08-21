using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;


        PlayerControls playerControls;

        [Header("MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        private void Awake()
        {
            if (instance == null)
            {
                if (instance == null)
                {
                    instance = this;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            // SCENE�� �ٲ�� ���� ����
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // WORLD SCENE���� �ε����� �� PLAYER CONTROLS Ȱ��ȭ
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            // �װ� �ƴ϶��, PLAYER CONTROLS ��Ȱ��ȭ
            else
            {
                instance.enabled = false;
            }
        }
        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // �� OBJECT�� �����ϸ�, EVENT�κ��� DETACH
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));


            // CLAMPING THE VALUES, VALUE CAN ONLY BE 0, 0.5 OR 1
            if (moveAmount <= 0.5 && moveAmount > 0) 
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            // WHY 0? WE ONLY WANT NOT-STRAFING MOVEMENT;
            // WE USE THE HORIZONTAL WHEN WE ARE STRAFING OR LOCKED ON

            if (player == null)
                return;
            // IF WE ARE NOT LOCKED ON, ONLY USE THE MOVE AMOUNT
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);

            // IF WE ARE LOCKED ON PASSS THE HORIZONTAL MOVEMENT AS WELL
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
    }
}
