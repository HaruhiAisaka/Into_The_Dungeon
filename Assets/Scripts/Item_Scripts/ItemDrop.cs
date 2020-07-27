using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    private ItemDatabase itemDatabase;
    private Item item;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        itemDatabase = FindObjectOfType<ItemDatabase>();
        item = itemDatabase.GetItem(itemName);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.sprite;
    }

    public Item GetItem(){
        return item;
    }
}
