using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemUI : MonoBehaviour
{
    [SerializeField] Image itemImage;

    // Update is called once per frame
    void Update()
    {
        // Animations could happen here
    }

    public void UpdateItemImage(Item item) {
        itemImage.sprite = item.sprite;
    }
}
