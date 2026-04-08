using UnityEngine;

public class UpgradeItem : MonoBehaviour
{
    [SerializeField] private PlayerUpgradeData upgradeData;
    public string ItemName => upgradeData != null ? upgradeData.upgradeName : "Unknown";

    private bool pickedUp = false;

    // Called by PlayerController when pressing E
    public void Pickup(PlayerStats stats)
    {
        if (pickedUp) return;
        pickedUp = true;

        if (stats != null && upgradeData != null)
        {
            stats.ApplyUpgrade(upgradeData);
            Debug.Log($"Picked up {upgradeData.upgradeName} ({upgradeData.valueType}, {upgradeData.value})");
            Destroy(gameObject);
        }
    }
}
