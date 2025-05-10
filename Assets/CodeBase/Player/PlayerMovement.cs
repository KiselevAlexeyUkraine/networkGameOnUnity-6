using Unity.Netcode;
using UnityEngine;
using System;

namespace CodeBase.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f; 
        [SerializeField] private float jumpHeight = 2f; 
        [SerializeField] private float gravity = -9.81f;

        [Header("References")]
        [SerializeField] private GameObject cameras;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private GameObject nameTextGameobject;
        [SerializeField] private GameObject hpTextGameobject;

        private CharacterController controller;
        private Vector3 velocity;
        private bool isGrounded;
        private bool isWalking;

        public event Action<bool> OnWalkStateChanged;
        public event Action OnJump;

        private void Start()
        {
            controller = GetComponent<CharacterController>();

            if (!IsOwner)
            {
                enabled = false;
                cameras.SetActive(false);
                hpTextGameobject.SetActive(false);
                return;
            }
            else
            {
                nameTextGameobject.SetActive(false);
            }
        }

        private void Update()
        {
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 move = camRight * moveX + camForward * moveZ;

            bool shouldWalk = move.magnitude > 0.1f;
            if (shouldWalk != isWalking)
            {
                isWalking = shouldWalk;
                OnWalkStateChanged?.Invoke(isWalking);
            }

            if (shouldWalk)
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
            }

            controller.Move(move * moveSpeed * Time.deltaTime);
            HandleJump();

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                OnJump?.Invoke();
            }
        }
    }

}