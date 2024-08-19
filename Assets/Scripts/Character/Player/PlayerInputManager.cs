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

                playerControls.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // �� OBJECT�� �����ϸ�, EVENT�κ��� DETACH
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
