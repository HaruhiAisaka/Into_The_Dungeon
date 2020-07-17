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

    public void updateKeys(bool[] keysArr) {
        // update each key in turn
        if (keysArr[0]) {
            this.key1.sprite = key;
        }

        if (keysArr[1]) {
            this.key2.sprite = key;
        }

        if (keysArr[2]) {
            this.key3.sprite = key;
        }
    }
}
