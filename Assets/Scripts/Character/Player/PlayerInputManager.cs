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

        [SerializeField] Vector2 movement;

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

                playerControls.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // 이 OBJECT를 삭제하면, EVENT로부터 DETACH
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
