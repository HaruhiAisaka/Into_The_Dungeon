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
    private string[] keys = {}; // TODO: replace with array of Key Objects

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
        // animation for weapon icons ???
        // ...

        // DEBUG---------
         if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("[Player UI] adding 1 health");
            currentHealth += 1;
            updateHealth(currentHealth + 1, maxHealth);
        } else if (Input.GetKeyDown(KeyCode.Minus)) {
            Debug.Log("[Player UI] subbing 1 health");
            currentHealth -= 1;
            updateHealth(currentHealth - 1, maxHealth);
        }
    }

    // ----- Updating UI

    public void updateUI(Player player) {
        //updateHealth(player.health);
        //updateEquippedWeapon(player.equippedWeapon);
        //updateEquippedItem(player.equippedItem);
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

    public void updateKeys(/* Keys Object */) {
        //keys = newKeys;
        // TODO : update ui
        // key object should have fields for color, so this just draws it on
        // screen
    }
}
