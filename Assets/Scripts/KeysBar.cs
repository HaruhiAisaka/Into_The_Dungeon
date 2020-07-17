using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysBar : MonoBehaviour
{

    // Key Images
    [SerializeField] Image key1;
    [SerializeField] Image key2;
    [SerializeField] Image key3;
    // Key Sprites
    [SerializeField] Sprite key;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateKeys(bool key1, bool key2, bool key3) {
        // update each key in turn
        if (key1) {
            this.key1.sprite = key;
        }

        if (key2) {
            this.key2.sprite = key;
        }

        if (key3) {
            this.key3.sprite = key;
        }
    }
}
