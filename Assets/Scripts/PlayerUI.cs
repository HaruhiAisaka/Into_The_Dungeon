using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Central control point for controlling/updating elements of the UI
 */
public class PlayerUI : MonoBehaviour
{

    // player data
    [SerializeField] int displayedHealth = 4; // 1 point = half heart
    [SerializeField] string equippedWeapon = ""; // TODO: replace with script class
    [SerializeField] string equippedItem = "";// TODO: replace with script class
    private string[] keys = {}; // TODO: replace with array of Key Objects

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // animation for weapon icons ???
    }

    // ----- Updating UI

    public updateUI(Player player) {
        updateHealth(player.health);
        updateEquippedWeapon(player.equippedWeapon);
        updateEquippedItem(player.equippedItem);
    }

    public updateHealth(int newHealth) {
        displayedHealth = newHealth;
        // update hearts display
    }

    public updateEquippedWeapon(/* weapon class */) {
        //equippedWeapon = newWeapon;
        // TODO : update ui
    }

    public updateEquippedItem(/* item class */) {
        equippedItem = newItem;
        // TODO : update ui
    }

    public updateEquippedWeapon(/* weapon class */) {
        equippedWeapon = newWeapon;
        // TODO : update ui
    }

    public updateKeys(/* Keys Object */) {
        //keys = newKeys;
        // TODO : update ui
        // key object should have fields for color, so this just draws it on
        // screen
    }
}
