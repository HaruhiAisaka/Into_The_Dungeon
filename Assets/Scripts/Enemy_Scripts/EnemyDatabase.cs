using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase : MonoBehaviour
{

    [SerializeField] public GameObject SkullPrefab;
    [SerializeField] public GameObject BlobPrefab;
    [SerializeField] public GameObject BatPrefab;
    [SerializeField] public GameObject ShooterPrefab;


    void Start()
    {
        Debug.Log(BatPrefab.name);
        GameObject test = Instantiate(BatPrefab);
        Debug.Log("length: " + (new List<GameObject>() { SkullPrefab, BlobPrefab, ShooterPrefab }[0].name));

    }
    public List<GameObject> getEnemies()
    {
        Debug.Log("get enemies");
        return new List<GameObject>() { SkullPrefab, BlobPrefab, BatPrefab, ShooterPrefab };

    }
}
