using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Item item;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.sprite;
    }

    public Item GetItem(){
        return item;
    }

    // Makes sure that the reference to the item in Room is no longer there.
    private void OnDestroy() {
        FindObjectOfType<CurrentRoom>().GetCurrentRoom().items.Remove(item);
    }
}
