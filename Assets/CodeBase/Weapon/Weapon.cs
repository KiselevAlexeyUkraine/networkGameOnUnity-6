using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace CodeBase.Weapon
{
    public class Weapon : NetworkBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        //[SerializeField] private NetworkAnimator netAnimator;

        void Update()
        {
            if (!IsOwner) return;

            if (Input.GetMouseButtonDown(0))
            {
                //netAnimator?.SetTrigger("Shoot");
                FireServerRpc();
            }
        }

        [ServerRpc]
        void FireServerRpc()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var bulletComp = bullet.GetComponent<Bullet>();
            bulletComp.SetOwner(OwnerClientId);
            bullet.GetComponent<NetworkObject>().Spawn();
        }
    }
}