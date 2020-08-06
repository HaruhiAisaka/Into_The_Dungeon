using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysBar : MonoBehaviour
{

    // Key Images
    List<Image> images = new List<Image>();

    /// Render each key in keyList
    public void UpdateKeys(List<Key> keyList) {
        // Destroy Existing
        foreach (Image image in images) {
            Destroy(image.gameObject);
        }
        images.Clear();

        // Add New
        foreach (Key key in keyList) {
            GameObject obj = new GameObject();
            Image image = obj.AddComponent<Image>();
            image.sprite = key.sprite;
            obj.GetComponent<RectTransform>().SetParent(this.gameObject.transform);
            obj.SetActive(true);

            images.Add(image);
        }
    }
}
