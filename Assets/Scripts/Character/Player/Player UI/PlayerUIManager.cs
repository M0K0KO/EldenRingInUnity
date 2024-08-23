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

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;

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
            
            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
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
                // 일단 셧다운. 타이틀 화면 도중에 HOST로 시작했기 때문이다.
                NetworkManager.Singleton.Shutdown();
                // CLIENT로서 RESTART
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}