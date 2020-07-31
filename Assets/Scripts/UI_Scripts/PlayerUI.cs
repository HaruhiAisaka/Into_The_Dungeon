using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Central control point for controlling/updating elements of the UI
 */
public class PlayerUI : MonoBehaviour
{

    // UI Fields
    [SerializeField] HealthBar healthbar;
    [SerializeField] EquippedWeaponUI equippedWeaponUI;
    [SerializeField] EquippedItemUI equippedItemUI;
    [SerializeField] KeysBar keysBar;

    /// Updates all UI components based on the player
    public void UpdateAll(Player player) {
        UpdateHealth(player.GetHealth(), 14);
    }

    public void UpdateHealth(int newCurrentHealth, int newMaxHealth) {
        healthbar.UpdateHearts(newCurrentHealth, newMaxHealth);
    }

    public void UpdateEquippedWeapon(EquipableItem weapon) {
        equippedWeaponUI.UpdateItemImage(weapon);
    }

    public void UpdateEquippedItem(EquipableItem item) {
        equippedItemUI.UpdateItemImage(item);
    }

    public void UpdateKeys(List<Key> keysArr) {
        if (keysBar != null) {
            keysBar.UpdateKeys(keysArr);
        }
    }
}
