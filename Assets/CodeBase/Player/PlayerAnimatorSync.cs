using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace CodeBase.Player
{
    [RequireComponent(typeof(NetworkAnimator))]
    public class PlayerAnimatorSync : NetworkBehaviour
    {
        private NetworkAnimator networkAnimator; 
        private PlayerMovement playerMovement;

        private void Awake()
        {
            networkAnimator = GetComponent<NetworkAnimator>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnEnable()
        {
            if (playerMovement != null)
            {
                playerMovement.OnWalkStateChanged += HandleWalkStateChanged;
                playerMovement.OnJump += HandleJump;
            }
        }

        private void OnDisable()
        {
            if (playerMovement != null)
            {
                playerMovement.OnWalkStateChanged -= HandleWalkStateChanged;
                playerMovement.OnJump -= HandleJump;
            }
        }

        private void HandleWalkStateChanged(bool isWalking)
        {
            if (IsOwner)
            {
                if (isWalking)
                {
                    networkAnimator.SetTrigger("StartWalk");
                }
                else
                {
                    networkAnimator.SetTrigger("StopWalk");
                }
            }
        }

        private void HandleJump()
        {
            if (IsOwner)
            {
                networkAnimator.SetTrigger("Jump");
            }
        }
    }

}