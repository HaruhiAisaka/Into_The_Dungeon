using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{

    // Player/Displayed Health
    [SerializeField] int currentHealth = 0; // 1 = 1 half heart
    [SerializeField] int maxHealth = 14;
    [SerializeField] int maxDisplayedHealth = 14; // Divide by 2 to get number of hearts

    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] Sprite lockedHeart;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update() {
    }

    public void updateHearts(int currentHealth, int maxHealth) {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;

        Debug.Log("Starting this");

        // Assert Ranges
        Debug.Assert(this.currentHealth >= 0 && this.currentHealth < maxDisplayedHealth, "Current Health OOB in health bar script.");
        Debug.Assert(this.maxHealth >= 0 && this.currentHealth < maxDisplayedHealth, "Max Health OOB in health bar script");

        int numberOfHalfHearts = currentHealth % 2;
        int numberOfFullHearts = (currentHealth - numberOfHalfHearts) / 2;
        int numberOfEmptyHearts = (maxHealth / 2) - numberOfHalfHearts - numberOfFullHearts;

        // and the rest are locked
        int cur = 0;

        // full hearts
        for (int i = 0; i < numberOfFullHearts; i++) {
            hearts[cur].sprite = fullHeart;
            cur++;
        }
        // half hearts
        for (int i = 0; i < numberOfHalfHearts; i++) {
            hearts[cur].sprite = halfHeart;
            cur++;
        }
        // empty
        for (int i = 0; i < numberOfEmptyHearts; i++) {
            hearts[cur].sprite = emptyHeart;
            cur++;
        }
        // locked
        for (int i = cur; i < (maxHealth / 2); i++) {
            hearts[cur].sprite = lockedHeart;
            cur++;
        }

    }

}
