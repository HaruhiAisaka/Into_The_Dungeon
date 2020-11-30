using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysBar : MonoBehaviour
{

    // Key Images
    [SerializeField] List<Image> images = new List<Image>();

    [SerializeField] Sprite black_box;

    /// Render each key in keyList
    public void UpdateKeys(List<Key> keyList) {
        int cur = 0;
        // Add New
        foreach (Key key in keyList) {
            images[cur].sprite = key.sprite;
            cur ++;
        }
    }
}
