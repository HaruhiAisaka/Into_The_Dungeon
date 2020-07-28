using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysBar : MonoBehaviour
{

    // Key Images
    [SerializeField] Image keyImage1;
    [SerializeField] Image keyImage2;
    [SerializeField] Image keyImage3;
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

    public void UpdateKeys(List<Item> keyList) {
        // update each key in turn
        keyImage1.sprite = keyList[0];
        keyImage2.sprite = keyList[1];
        keyImage3.sprite = keyList[2];
    }
}
