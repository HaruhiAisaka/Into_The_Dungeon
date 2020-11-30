using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    // Player/Displayed Health
    int currentHealth = 12; // 1 = 1 half heart
    int maxHealth;

    // Heart Images
    [SerializeField] List<Image> heartImages = new List<Image>();

    // Sprite Constants
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite emptyHeart;

    public void Start() {
        maxHealth = heartImages.Count * 2;
        UpdateHealth(currentHealth);
    }

    public void UpdateHealth(int currentHealth) {
        // Keep health in bounds
        this.currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        int numberOfHalfHearts = currentHealth % 2;
        int numberOfFullHearts = (currentHealth - numberOfHalfHearts) / 2;
        int numberOfEmptyHearts = (maxHealth / 2) - numberOfHalfHearts - numberOfFullHearts;

        int cur = 0;

        // full hearts
        for (int i = 0; i < numberOfFullHearts; i++) {
            heartImages[cur].sprite = fullHeart;
            cur++;
        }
        // half hearts
        for (int i = 0; i < numberOfHalfHearts; i++) {
            heartImages[cur].sprite = halfHeart;
            cur++;
        }
        // empty
        for (int i = 0; i < numberOfEmptyHearts; i++) {
            heartImages[cur].sprite = emptyHeart;
            cur++;
        }
    }

}
