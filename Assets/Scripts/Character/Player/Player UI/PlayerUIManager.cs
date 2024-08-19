using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;
        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        private void Awake()
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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // �ϴ� �˴ٿ�. Ÿ��Ʋ ȭ�� ���߿� HOST�� �����߱� �����̴�.
                NetworkManager.Singleton.Shutdown();
                // CLIENT�μ� RESTART
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}