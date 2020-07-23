using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used for functions that affect all enemies.
public class Enemies : MonoBehaviour
{
    private Enemy[] enemies;
    private void Start() {
        enemies = this.GetComponentsInChildren<Enemy>();
        
    }
    //Freezes all the enemies.
    public void FreezeAllEnemies(){
        foreach (var enemy in enemies){
            enemy.FreezeEnemy();
        }
    }

    
}
