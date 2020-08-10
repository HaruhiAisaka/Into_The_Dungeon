using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface attached to items that can have more than one instance.
// Most items have only one instance that exists. 
// Such as a sword (there can't be two legendary swords).
// However, some items, like hearts, can have more than one instance that exists.
// In order for more than once instance to exist, the item itsself must be copied.
public interface IItemCopyable
{
    // Creates a new instance of the item.
    Item CopyItem(Vector2 newPosition);
}