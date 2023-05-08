using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : Room
{

    public override void Activate() {
        foreach(GameObject door in doors) {
            door.SetActive(true);
        }
        Deactivate();
    }

    public override void Deactivate() {
        foreach(GameObject door in doors) {
            door.SetActive(false);
        }
    }
}
