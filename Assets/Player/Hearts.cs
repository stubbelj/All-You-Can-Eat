using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{
    public GameObject heartPrefab;
    public Sprite[] heartSprites;
    public int displayedHP;

    GameManager gameManager;
    Player player;

    List<GameObject> hearts = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.inst;
        player = gameManager.player;
        displayedHP = player.currHP;
        UpdateHearts();
    }

    public void UpdateHearts() {
        print("updating hearts");
        while(player.maxHP < hearts.Count) {
            Destroy(hearts[hearts.Count - 1]);
            hearts.RemoveAt(hearts.Count - 1);
        }
        while(player.maxHP > hearts.Count) {
            GameObject newHeart = GameObject.Instantiate(heartPrefab, new Vector3(-500, (hearts.Count * -60) + 100, 0), Quaternion.identity);
            hearts.Add(newHeart);
            newHeart.transform.SetParent(transform, false);
        }
        while(player.currHP < displayedHP) {
            int heartToSet = hearts.Count - 1;
            while(hearts[heartToSet].GetComponent<Image>().sprite == heartSprites[0]) {
                heartToSet--;
            }
            hearts[heartToSet].GetComponent<Image>().sprite = heartSprites[0];
            displayedHP--;
        }
        while(player.currHP > displayedHP) {
            int heartToSet = 0;
            while(hearts[heartToSet].GetComponent<Image>().sprite == heartSprites[1]) {
                heartToSet++;
            }
            hearts[heartToSet].GetComponent<Image>().sprite = heartSprites[1];
            displayedHP++;
        }
    }
}
