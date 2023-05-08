using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public Room parentRoom;

    protected void Wait(float waitTime) {
        while (waitTime > 0) {
            waitTime -= Time.unscaledDeltaTime;
            print("waiting");
        }
    }
}
