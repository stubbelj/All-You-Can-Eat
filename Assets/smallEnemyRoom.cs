using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallEnemyRoom : Room
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public override void Activate() {
        StartCoroutine(spawnEnemies());
    }

    public IEnumerator spawnEnemies() {
        for(int i = 0; i < 5; i++) {
            Transform spawnPoint = spawnPoints[r.Next(0, spawnPoints.Count)];
            Instantiate(enemyPrefabs[r.Next(0, enemyPrefabs.Count)], spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(2f);
        }
    }
}
