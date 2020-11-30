using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        UpdateHealth(player.GetHealth());
    }

    public void UpdateHealth(int newCurrentHealth) {
        if (healthbar != null) {
            healthbar.UpdateHealth(newCurrentHealth);
        }
    }


    public void UpdateEquippedWeapon(EquipableItem weapon) {
        if (equippedWeaponUI != null) {
            equippedWeaponUI.UpdateItemImage(weapon);
        }
    }

    public void UpdateEquippedItem(EquipableItem item) {
        if (equippedItemUI != null) {
            equippedItemUI.UpdateItemImage(item);
        }
    }

    public void UpdateKeys(List<Key> keysArr) {
        if (keysBar != null) {
            keysBar.UpdateKeys(keysArr);
        }
    }

}
