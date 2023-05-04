using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Room : MonoBehaviour
{
    GameManager gameManager;
    public System.Random r;

    public float[] bounds;
    //xmin, xmax, ymin, ymax
    [HideInInspector]
    public float width = 140f;
    [HideInInspector]
    public float height = 140f;
    //width, height
    public List<Transform> spawnPoints = new List<Transform>();
    public List<Transform> doorways = new List<Transform>();
    public List<GameObject> doors = new List<GameObject>();
    public List<string> doorSide = new List<string>();

    public List<RoomObject> roomObjects = new List<RoomObject>();

    string state = "inactive";
    //inactive, active, complete

    void Awake()
    {
    }

    void Start() {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return new WaitForSeconds(0.1f);
    }

    void Update()
    {
        
    }

    public void DoorwayTrigger(GameObject doorway) {
        if (state == "inactive") {
            state = "active";
            Activate();
        }
    }

    public float[] GetBounds() {
        Bounds tilemapBounds = transform.Find("Grid").Find("Walls").GetComponent<Tilemap>().localBounds;
        bounds = new float[]{transform.position.x - tilemapBounds.extents.x, transform.position.x + tilemapBounds.extents.x, transform.position.y - tilemapBounds.extents.y, transform.position.y + tilemapBounds.extents.y};
        return bounds;
    }

    public void Init() {
        gameManager = GameManager.inst;
        r = gameManager.r;
        
        Bounds tilemapBounds = transform.Find("Grid").Find("Walls").GetComponent<Tilemap>().localBounds;
        width = tilemapBounds.extents.x * 2;
        height = tilemapBounds.extents.x * 2;
        bounds = new float[]{transform.position.x - tilemapBounds.extents.x, transform.position.x + tilemapBounds.extents.x, transform.position.y - tilemapBounds.extents.y, transform.position.y + tilemapBounds.extents.y};
        //bounds = new float[]{tilemapBounds.center.x - tilemapBounds.extents.x, tilemapBounds.center.x + tilemapBounds.extents.x, tilemapBounds.center.y - tilemapBounds.extents.y, tilemapBounds.center.y + tilemapBounds.extents.y};
        
        //shuffle door order - which are entrances, exits, etc.
        int[] shuffleIndices = new int[]{r.Next(0, doors.Count), r.Next(0, doors.Count)};
        GameObject tempDoor = doors[shuffleIndices[0]];
        doors[shuffleIndices[0]] = doors[shuffleIndices[1]];
        doors[shuffleIndices[1]] = tempDoor;

        for (int i = 0; i < doors.Count; i++) {
            //find which side the door is on
            string bestSide = "left";
            float bestMag = (transform.position + new Vector3(-tilemapBounds.extents.x, 0, 0) - doors[i].transform.position).magnitude;
            if ((transform.position + new Vector3(tilemapBounds.extents.x, 0, 0) - doors[i].transform.position).magnitude < bestMag) {
                bestSide = "right";
                bestMag = (transform.position + new Vector3(tilemapBounds.extents.x, 0, 0) - doors[i].transform.position).magnitude;
            }
            if ((transform.position + new Vector3(0, tilemapBounds.extents.y, 0) - doors[i].transform.position).magnitude < bestMag) {
                bestSide = "up";
                bestMag = (transform.position + new Vector3(0, tilemapBounds.extents.y, 0) - doors[i].transform.position).magnitude;
            }
            if ((transform.position + new Vector3(0, -tilemapBounds.extents.y, 0) - doors[i].transform.position).magnitude < bestMag) {
                bestSide = "down";
                bestMag = (transform.position + new Vector3(0, -tilemapBounds.extents.y, 0) - doors[i].transform.position).magnitude;
            }
            doorSide.Add(bestSide);
        }
    }

    public Vector3 SideCoordsFromSide(string sideName) {
        Bounds tilemapBounds = transform.Find("Grid").Find("Walls").GetComponent<Tilemap>().localBounds;
        if (sideName == "up") {
            return transform.position + new Vector3(0, tilemapBounds.extents.y, 0);
        } else if (sideName == "down") {
            return transform.position + new Vector3(0, -tilemapBounds.extents.y, 0);
        } else if (sideName == "left") {
            return transform.position + new Vector3(-tilemapBounds.extents.x, 0, 0);
        } else if (sideName == "right") {
            //right
            return transform.position + new Vector3(tilemapBounds.extents.x, 0, 0);
        }
        return Vector3.zero;
    }

    public void ActivateRoomObjects() {
        foreach (RoomObject robj in roomObjects) {
            robj.gameObject.SetActive(true);
        }
    }

    public void DeactivateRoomObjects() {
        foreach (RoomObject robj in roomObjects) {
            robj.gameObject.SetActive(false);
        }
    }

    public abstract void Activate();

    public abstract void Deactivate();
}
