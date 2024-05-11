using Unity.FPS.Game;
using UnityEngine;
using MikrosClient;
using MikrosClient.Analytics;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public class HealthPickup : Pickup
    {
        [Header("Parameters")]
        [Tooltip("Amount of health to heal on pickup")]
        public float HealAmount;

        protected override void OnPicked(PlayerCharacterController player)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth && playerHealth.CanPickup())
            {
                playerHealth.Heal(HealAmount);
                PlayPickupFeedback();
                Destroy(gameObject);

                // Log event for picking up health to Mikros analytics
                MikrosManager.Instance.AnalyticsController.LogEvent(
                    "health_pickup_mikros",
                    "heal_amount",
                    HealAmount.ToString(),
                    (Hashtable customEventData) =>
                    {
                        // Handle success
                        Debug.Log("Mikros: Health pickup event logged successfully");
                    },
                    onFailure =>
                    {
                        // Handle failure
                        Debug.LogWarning("Mikros: Failed to log health pickup event");
                    });

                // Log event for picking up health to Firebase analytics
                Firebase.Analytics.FirebaseAnalytics.LogEvent(
                    Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
                    new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterValue, HealAmount));

                Debug.Log("Firebase: Health pickup event logged successfully");
            }
        }
    }
}
