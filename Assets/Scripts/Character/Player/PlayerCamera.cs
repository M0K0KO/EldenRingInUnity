using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG {
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;

        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1; // 클수록 늦게 따라온다
        [SerializeField] float leftAndRightRotationSpeed = 110;
        [SerializeField] float upAndDownRotationSpeed = 110;
        [SerializeField] float minimumPivot = -30; //THE LOWEST POINT
        [SerializeField] float maximumPivot = 60; //THE HIGHEST POINT
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; //MOVE THE CAMERA OBJECT TO THIS POSITION
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;
        private float targetCameraZPosition;

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
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                // FOLLOW THE PLAYER
                HandleFollowTarget();
                // ROTATE AROUND THE PLAYER
                HandleRotations();
                // COLLIDE WITH OBJECTS
                HandleCollisions();
            }

        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // IF LOCKED ON, FORCE ROTATIONS TOWARDS TARGET
            // ELSE ROTATE REGULARLY

            //NORMAL ROTATIONS
            // ROTATE LEFT AND RIGHT BASED ON HORIZONTAL MOVEMENT ON THE MOUSE MOVEMENT
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            // ROTATE UP AND DOWN BASED ON VERTICAL MOVEMENT ON THE MOUSE MOVEMENT
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            // CLAMP THE UP AND DOWN LOOK ANGLE BETWEEN A MIN AND MAX VALUE
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // ROTATE THIS GAMEOBJECT LEFT AND RIGHT
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // ROTATE THIS PIVOT GAMEOBJECT UP AND DOWN
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            // DIRECTION FOR COLLISION CHECK
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // CHECK IF THERE IS AN OBJECT
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                // IF THERE IS, GET DISTANCE FROM IT
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // WE THEN EQUATE OUR TARGET Z POSITION TO THE FOLLOWING
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            // IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADISU, WE SUBTRACT OUR COLLISION RADIUS 
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // APPLY OUR FINAL POSITION
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    }
}