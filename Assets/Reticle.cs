using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    //public Sprite reticleSprite;
    Image img;
    Camera cam;

    void Start() {
        img = GetComponent<Image>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
    }

    public void ChangeReticle(Sprite newReticle) {
        img.sprite = newReticle;
        Color tempColor = img.color;
        tempColor.a = 0.5f;
        img.color = tempColor;
    }
}
