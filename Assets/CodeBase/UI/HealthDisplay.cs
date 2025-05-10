using CodeBase.Player;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        private TextMeshProUGUI healthText;
        [SerializeField] private NetworkHealth networkHealth;

        private void Awake()
        {
            if (networkHealth == null)
            {
                Debug.LogError("NetworkHealth not found in parent.");
                enabled = false;
                return;
            }
            healthText = GetComponent<TextMeshProUGUI>();

            networkHealth.Health.OnValueChanged += OnHealthChanged;
            OnHealthChanged(0, networkHealth.Health.Value);
        }

        private void OnHealthChanged(int oldValue, int newValue)
        {
            healthText.text = $"HP: {newValue}";
        }

        private void OnDestroy()
        {
            if (networkHealth != null)
                networkHealth.Health.OnValueChanged -= OnHealthChanged;
        }
    }
}