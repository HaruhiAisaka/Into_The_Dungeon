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

    // Pause
    [SerializeField] Menu menu;
    [SerializeField] Text pauseText;

    public void Start() {
        if (pauseText != null) {
            pauseText.gameObject.SetActive(false);
        }
        if (menu != null) {
            menu.gameObject.SetActive(false);
        }
    }

    /// Updates all UI components based on the player
    public void UpdateAll(Player player) {
        UpdateHealth(player.GetHealth(), 14);
    }

    public void UpdateHealth(int newCurrentHealth, int newMaxHealth) {
        if (healthbar != null) {
            healthbar.UpdateHealth(newCurrentHealth, newMaxHealth);
        }
    }

    public void UpdateHeartsDisplay(int newMaxHealth) {
        if (healthbar != null) {
            healthbar.UpdateHeartsDisplay(newMaxHealth);
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

    public void TogglePauseState() {
        GameState.TogglePause(delegate() {
            if (pauseText != null) {
                pauseText.gameObject.SetActive(GameState.IsPaused());
            }
        });
    }

    public void ToggleMenuState() {
        GameState.ToggleMenu(delegate() {
            if (menu != null) {
                menu.gameObject.SetActive(GameState.InMenu());
            }
        });
    }
}
