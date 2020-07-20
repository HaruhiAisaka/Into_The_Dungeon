using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Central control point for controlling/updating elements of the UI
 */
public class PlayerUI : MonoBehaviour
{

    // player data
    [SerializeField] healthbar healthbar;
    [SerializeField] string equippedWeapon = ""; // TODO: replace with script class
    [SerializeField] string equippedItem = "";// TODO: replace with script class
    [SerializeField] KeysBar keys;

    // --- Debug -----
    // Testing Health Bar
    [SerializeField] int currentHealth = 10;
    [SerializeField] int maxHealth = 14;
    // ---------------

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // ----- Updating UI

    public void updateUI(Player player) {
        updateHealth(player.playerHealth, 14);
        // 14 As player does not have a max health field

        //updateEquippedWeapon(player.equippedWeapon); // TODO when we have weapons
        //updateEquippedItem(player.equippedItem); // TODO when we have Items

        updateKeys(new bool[] {true, true, false});
    }

    public void updateHealth(int newCurrentHealth, int newMaxHealth) {
        healthbar.updateHearts(newCurrentHealth, newMaxHealth);
    }

    public void updateEquippedWeapon(/* weapon class */) {
        //equippedWeapon = newWeapon;
        // TODO : update ui
    }

    public void updateEquippedItem(/* item class */) {
        //equippedItem = newItem;
        // TODO : update ui
    }

    public void updateKeys(bool[] keysArr) {
        keys.updateKeys(keysArr);
    }
}
