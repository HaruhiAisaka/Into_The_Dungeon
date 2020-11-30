using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeaponUI : MonoBehaviour
{

    [SerializeField] Image itemImage;

    // Start is called before the first frame update
    void Start()
    {
        // Animations could happen here
    }

    public void UpdateItemImage(Item item) {
        itemImage.sprite = item.sprite;
    }
}
