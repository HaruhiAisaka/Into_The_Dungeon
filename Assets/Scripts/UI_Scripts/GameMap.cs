using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameMap : MonoBehaviour
{

    [SerializeField] Tilemap map;
    [SerializeField] Tile tile;

    // Start is called before the first frame update
    void Start() {
        //  Resize map
        map.ResizeBounds();
        // testing drawing
        Debug.Log("Adding to tilemap?");
        Debug.Log("cell bounds:");
        Debug.Log(map.cellBounds);

        Debug.Log("origin:");
        Debug.Log(map.origin);

        map.SetTile(new Vector3Int(1 , 1 , 0), tile);
        map.SetTile(new Vector3Int(2 , 2 , 0), tile);
        map.SetTile(new Vector3Int(3 , 3 , 0), tile);
        map.SetTile(new Vector3Int(4 , 4 , 0), tile);
        map.SetTile(new Vector3Int(5 , 5 , 0), tile);
        map.SetTile(new Vector3Int(6 , 6 , 0), tile);
        map.SetTile(new Vector3Int(7 , 7 , 0), tile);
        map.SetTile(new Vector3Int(8 , 8 , 0), tile);
        map.SetTile(new Vector3Int(9 , 9 , 0), tile);
        map.SetTile(new Vector3Int(10, 10, 0), tile);
        map.SetTile(new Vector3Int(11, 11, 0), tile);
        map.SetTile(new Vector3Int(12, 12, 0), tile);
        map.SetTile(new Vector3Int(13, 13, 0), tile);
        map.SetTile(new Vector3Int(14, 14, 0), tile);
        map.SetTile(new Vector3Int(15, 15, 0), tile);
        map.SetTile(new Vector3Int(16, 16, 0), tile);
        map.SetTile(new Vector3Int(17, 17, 0), tile);
        map.SetTile(new Vector3Int(18, 18, 0), tile);
        map.SetTile(new Vector3Int(19, 19, 0), tile);

        map.SetTile(new Vector3Int(100, 100, 0), tile);
        map.SetTile(new Vector3Int(200, 200, 0), tile);
        map.SetTile(new Vector3Int(300, 300, 0), tile);
        map.SetTile(new Vector3Int(400, 400, 0), tile);
    }

}
