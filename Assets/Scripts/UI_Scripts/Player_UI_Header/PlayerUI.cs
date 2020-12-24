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
    [SerializeField] MainUI mainUI;

    [SerializeField] Player_UI_Map map; 


    /// Updates all UI components based on the player
    public void UpdateAll(Player player) {
        UpdateHealth(player.GetHealth());
    }

    public void UpdateHealth(int newCurrentHealth) {
        healthbar.UpdateHealth(newCurrentHealth);
    }


    public void UpdateEquippedWeapon(EquipableItem weapon) {
        equippedWeaponUI.UpdateItemImage(weapon);
    }

    public void UpdateEquippedItem(EquipableItem item) {
        equippedItemUI.UpdateItemImage(item);
    }

    public void UpdateKeys(List<Key> keysArr) {
        keysBar.UpdateKeys(keysArr);
    }

    public void EnableMainUI(){
        mainUI.EnableMainUI();
    }

    public void DisableMainUI(){
        mainUI.DisableMainUI();
    }
    
    public void SetMap(){
        map.SetMap();
    }
}
