using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameMap : MonoBehaviour
{

    private Tilemap map;

    // Start is called before the first frame update
    void Start() {
        map = this.GetComponent<Tilemap>();
    }

}
