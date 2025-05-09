using Unity.Netcode;
using UnityEngine;

namespace CodeBase.UI
{
    public class LookAtCamera : NetworkBehaviour
    {
        private Transform mainCameraTransform;

        private void Start()
        {
            if (IsOwner == true) return;
            Invoke(nameof(InitializeCamera), 3f);
        }

        private void InitializeCamera()
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                mainCameraTransform = cam.transform;
                InvokeRepeating(nameof(RotateToCamera), 0f, 0.5f);
            }
            else
            {
                Debug.LogWarning("Main Camera not found in the scene.");
            }
        }

        private void RotateToCamera()
        {
            if (mainCameraTransform == null) return;

            Vector3 direction = mainCameraTransform.position - transform.position;
            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 180, 0f);
        }
    }
}
