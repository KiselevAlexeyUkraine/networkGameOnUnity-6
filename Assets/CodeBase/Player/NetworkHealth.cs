using Unity.Netcode;
using UnityEngine;

namespace CodeBase.Player
{
    public class NetworkHealth : NetworkBehaviour
    {
        public NetworkVariable<int> Health = new NetworkVariable<int>(100);

        public void TakeDamage(int amount, ulong attackerClientId)
        {
            if (!IsServer) return;

            Health.Value -= amount;
            if (Health.Value <= 0)
            {
                Debug.Log($"{OwnerClientId} died");
                GameManager.Instance?.AddKill(attackerClientId, OwnerClientId);
                NetworkObject.Despawn();
            }
        }
    }
}