using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallEnemyRoom : Room
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    List<Enemy> roomEnemies = new List<Enemy>();

    public override void Activate() {
        r = gameManager.r;
        foreach(GameObject door in doors) {
            door.SetActive(true);
        }
        StartCoroutine(spawnEnemies());
    }

    public override void Deactivate() {
        foreach(GameObject door in doors) {
            door.SetActive(false);
        }
        gameManager.InitNextRoom();
    }

    public IEnumerator spawnEnemies() {
        for(int i = 0; i < 5; i++) {
            Transform spawnPoint = spawnPoints[r.Next(0, spawnPoints.Count)];
            GameObject newEnemy = GameObject.Instantiate(enemyPrefabs[r.Next(0, enemyPrefabs.Count)], spawnPoint.position, spawnPoint.rotation);
            newEnemy.GetComponent<Enemy>().parentRoom = this;
            roomObjects.Add(newEnemy.GetComponent<Enemy>());
            roomEnemies.Add(newEnemy.GetComponent<Enemy>());
            yield return new WaitForSeconds(2f);
        }
        StartCoroutine(WaitForRoomClear());
    }

    public IEnumerator WaitForRoomClear() {
        while (true) {
            yield return new WaitForSeconds(0.1f);
            foreach(Enemy enemy in roomEnemies) {
                if (enemy != null) {
                    continue;
                }
            }
            break;
        }
        Deactivate();
    }
}
