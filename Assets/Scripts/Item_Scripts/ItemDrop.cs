using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private string itemName = null;

    [SerializeField]
    private int itemID = -1;

    private ItemDatabase itemDatabase;
    private Item item;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        if (itemID >= 0){
            itemDatabase = FindObjectOfType<ItemDatabase>();
            item = itemDatabase.GetItem(itemID);
        }
        else if (itemName.Length >= 0){
            itemDatabase = FindObjectOfType<ItemDatabase>();
            item = itemDatabase.GetItem(itemName);
        }
        else{
            throw new System.InvalidOperationException("Invalid itemID or itemName. Please specify atleast one of them when creating an ItemDrop object");
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.sprite;
    }

    public Item GetItem(){
        return item;
    }
}
