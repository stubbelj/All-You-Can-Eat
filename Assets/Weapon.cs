using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //this is an abstract parent class that exists so calls can be made to weapons that inherit it for stuff like Attack()

    public float attackCooldown;
    public SpriteRenderer sr;
    public bool attacking;

    public abstract IEnumerator Attack();
}
