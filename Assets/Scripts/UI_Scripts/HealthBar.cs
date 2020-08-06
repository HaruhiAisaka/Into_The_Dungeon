using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    // Player/Displayed Health
    [SerializeField] int currentHealth = 0; // 1 = 1 half heart
    [SerializeField] int maxHealth = 14;
    [SerializeField] int maxHeartsDisplayed = 14; // Divide by 2 to get number of hearts

    // Heart Images
    List<Image> heartImages = new List<Image>();

    // Sprite Constants
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] Sprite lockedHeart;

    public void Start() {
        UpdateHeartsDisplay(maxHeartsDisplayed);
        UpdateHealth(currentHealth, maxHealth);
    }

    public void UpdateHeartsDisplay(int maxHeartsDisplayed) {
        // Destroy Existing
        foreach (Image image in heartImages) {
            Destroy(image.gameObject);
        }
        heartImages.Clear();

        // Add New
        for (int i = 0; i < maxHeartsDisplayed; i += 2) {
            GameObject obj = new GameObject();
            Image image = obj.AddComponent<Image>();
            obj.GetComponent<RectTransform>().SetParent(this.gameObject.transform);
            obj.SetActive(true);
            image.sprite = fullHeart;

            heartImages.Add(image);
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth) {
        // Keep health in bounds
        this.maxHealth = Mathf.Clamp(maxHealth, 0, maxHeartsDisplayed);
        this.currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        int numberOfHalfHearts = currentHealth % 2;
        int numberOfFullHearts = (currentHealth - numberOfHalfHearts) / 2;
        int numberOfEmptyHearts = (maxHealth / 2) - numberOfHalfHearts - numberOfFullHearts;
        // and the rest are locked

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
        // locked
        for (int i = cur; i < (maxHeartsDisplayed / 2); i++) {
            heartImages[cur].sprite = lockedHeart;
            cur++;
        }

    }

}
