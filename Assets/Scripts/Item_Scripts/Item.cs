using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public string name;
    public int itemID;
    public string description;
    public Sprite sprite;
    public Vector2 localPosition {get; protected set;}
    
    public void SetItemPosition(int x, int y){
        if(!Room.IsInRoom(x,y,1,1)){
            throw new System.ArgumentException("Coordinates provided not inside the room");
        } 
        else{
            localPosition = new Vector2(x,y); 
        }
    }

    public void SetItemPosition(Vector2 localPosition){
        if(!Room.IsInRoom(localPosition,1,1)){
            throw new System.ArgumentException("Coordinates provided not inside the room");
        }
        else{
            this.localPosition = localPosition;
        }
    }

    public abstract void OnPickUp(PlayerInventory playerInventory);
    
}
