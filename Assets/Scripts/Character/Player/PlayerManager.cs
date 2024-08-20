using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager playerLocomotionManager;
        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        protected override void Update()
        {
            base.Update();

            // GAMEOBJECT의 소유주가 아니면 접근불가하도록 설정
            if (!IsOwner)
                return;

            // 모든 움직인 관리
            playerLocomotionManager.HandleAllMovement();
        }
    }
}
