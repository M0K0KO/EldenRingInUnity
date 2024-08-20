using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;


        PlayerControls playerControls;

        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

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

            // SCENE이 바뀌면 로직 실행
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // WORLD SCENE으로 로딩중일 때 PLAYER CONTROLS 활성화
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            // 그게 아니라면, PLAYER CONTROLS 비활성화
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
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // 이 OBJECT를 삭제하면, EVENT로부터 DETACH
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
            HandleMovementInput();
        }

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (moveAmount <= 0.5 && moveAmount > 0) 
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
        }
    }
}
