using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysBar : MonoBehaviour
{

    // Key Images
    [SerializeField] Image[] keyImages = {};
    // Key Sprites
    [SerializeField] Sprite emptySlot;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateKeys(List<Key> keyList) {
        // update each key in turn
        for (int i = 0; i < keyImages.Length; i++) {
            keyImages[i].sprite = i < keyList.Count ? keyList[i].sprite : emptySlot;
        }
    }
}
