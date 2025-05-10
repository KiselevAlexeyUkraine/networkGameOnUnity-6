using Unity.Netcode;
using UnityEngine;
using CodeBase.Player;

namespace CodeBase.Weapon
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private int damage = 25;

        private ulong ownerId;

        public void SetOwner(ulong id) => ownerId = id;

        private void Start()
        {
            if (IsServer)
                Invoke(nameof(Despawn), lifetime);
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer) return;

            var target = other.GetComponent<NetworkHealth>();
            if (target != null)
            {
                target.TakeDamage(damage, ownerId);
            }

            Despawn();
        }

        private void Despawn()
        {
            if (IsServer)
                NetworkObject.Despawn();
        }
    }
}