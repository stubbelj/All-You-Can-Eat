using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager inst = null;
    //there is always ONLY ONE GameManger object, which stores important values for other scripts. in cases like this it is called a "singleton" and GameManager.inst is a reference to that
    //object. this is great because having only ONE object means that all scripts will have the same value for something like gameManager.currentFloor, whereas if you made a new GameManager
    //for each script you would have to update every script manually every time you changed the value of currentFloor.
    public Camera mainCam;
    public Canvas uiCanvas;
    public GameObject reticlePrefab;
    public Player player;
    public List<GameObject> roomPrefabs = new List<GameObject>();
    public List<GameObject> hallwayPrefabs = new List<GameObject>();
    public List<string> itemNames = new List<string>{};
    public List<Sprite> itemSprites = new List<Sprite>();
    public Dictionary<string, int> itemIndices = new Dictionary<string, int>{
    };
    public GameObject[] debugPrefab;

    public System.Random r = new System.Random(430);

    int level = 1;
    Reticle reticle = null;

    void Awake() {
        if (inst == null) {
            inst = this;
        } else {
            Destroy(gameObject);
        }

        int i = 0;
        foreach(string itemName in itemNames) {
            itemIndices.Add(itemName, i);
            i++;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
        GameObject newReticle = GameObject.Instantiate(reticlePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newReticle.transform.SetParent(uiCanvas.transform);
        reticle = newReticle.GetComponent<Reticle>();
    }

    public IEnumerator LateStart() {
        yield return new WaitForSeconds(0.5f);
        InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitLevel() {
        //initialize the floor layout for the level
        float compositeOverlapMargin = 1000f;
        //minimum distance between composites
        compositeOverlapMargin *= 1;

        float roomOverlapMargin = 600f;

        int[][] compositeQuantity = new int[][] {new int[] {1, 0}, new int[] {3, 1}, new int[] {6, 2}};
        //number of composites to load for each level BESIDES the start room, of normal and special types
        Graph graph = new Graph();
        //in this graph, node.adj[0] is the "previous" node in the normal node branch and node.adj[1] is the next one
        graph.root.data = "root";
        int i = compositeQuantity[level][0];
        Node curr = graph.root;
        while (i > 0) {
            //place normal nodes
            Node newNode = new Node(newData:"normal");
            curr.adj.Add(newNode);
            newNode.adj.Add(curr);
            curr = newNode;
            i--;
        }
        Node endNode = new Node(newData:"end");
        curr.adj.Add(endNode);
        endNode.adj.Add(curr);
        //place end node, which is connected to last normal node
        i = compositeQuantity[level][0];
        curr = graph.root.adj[0];
        while (i > 0) {
            //place special nodes
            Node newNode = new Node(newData:"special");
            curr.adj.Add(newNode);
            curr = curr.adj[1];
            i--;
        }

        /*curr = graph.root;
        for(int j = 0; j < 2; j++) {
            print("adjacent nodes to " + curr.data + "are: ");
            foreach(Node node in curr.adj) {
                print(node.data);
            }
            curr = curr.adj[0];
        }*/

        Vector3[] units = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right};
        List<float[]> spawnedCompositeBounds = new List<float[]>();
        curr = graph.root.adj[0];
        //initialize curr as first normal node
        while(curr.data != "end") {
            //traverse through nodes, generating them as you go
            (List<GameObject>, float[]) currComposite = GenerateComposite(curr, roomOverlapMargin);
                //generate current normal composite
                //int overlapDebugCounter1 = 100;
                while (OverlapTransforms(currComposite.Item2, spawnedCompositeBounds, errorMargin : compositeOverlapMargin) /*&& overlapDebugCounter1 > 0*/) {
                    //check if newly spawned composite overlaps with any previous composites. if so, move it
                    /*if (true) {
                        print("overlapDebugCounter1: " + overlapDebugCounter1);
                        print(currComposite.Item2);
                        foreach(float[] boundsItem in spawnedCompositeBounds) {
                            print(boundsItem);
                        }
                        print(OverlapTransforms(currComposite.Item2, spawnedCompositeBounds));
                    }*/
                    float roomSizeShiftMod = 140f;
                    int j = r.Next(0, units.Length);
                    Vector3 roomShift = units[j] * roomSizeShiftMod;
                    foreach (Transform obj in currComposite.Item1[0].transform.parent) {
                        //move each room in composite
                        obj.position += roomShift;
                    }
                    currComposite.Item2[0] += roomShift.x;
                    currComposite.Item2[1] += roomShift.x;
                    currComposite.Item2[2] += roomShift.y;
                    currComposite.Item2[3] += roomShift.y;
                    //overlapDebugCounter1--;
                }
            //print(overlapDebugCounter1);

            spawnedCompositeBounds.Add(currComposite.Item2);
            //add current normal composite to composite bounds list
            foreach(Node node in curr.adj) {
                //generate composites adjacent to the normal composite
                if (node.data != "normal") {
                    //don't generate the next normal composite yet
                    //int overlapDebugCounter2 = 100;
                    (List<GameObject>, float[]) adjComposite = GenerateComposite(node, roomOverlapMargin);
                    while (OverlapTransforms(adjComposite.Item2, spawnedCompositeBounds, errorMargin : compositeOverlapMargin) /*&& overlapDebugCounter2 > 0*/) {
                    
                        //check if newly spawned adj composite overlaps with any previous composites. if so, move it
                        float roomSizeShiftMod = 140f;
                        int j = r.Next(0, units.Length);
                        Vector3 roomShift = units[j] * roomSizeShiftMod;
                        foreach (Transform obj in adjComposite.Item1[0].transform.parent) {
                            //move each room in composite
                            obj.position += roomShift;
                        }
                        adjComposite.Item2[0] += roomShift.x;
                        adjComposite.Item2[1] += roomShift.x;
                        adjComposite.Item2[2] += roomShift.y;
                        adjComposite.Item2[3] += roomShift.y;
                        //overlapDebugCounter2--;
                    }
                    //print(overlapDebugCounter2);
                    spawnedCompositeBounds.Add(adjComposite.Item2);
                    //add current adj composite to composite bounds list
                }
            }
            curr = curr.adj[1];
            //move to the next normal node
        }
    }

    (List<GameObject>, float[]) GenerateComposite(Node node, float OverlapMargin) {
        float hallwayWidth = 64f;
        float roomOverlapMargin = OverlapMargin;
        //generates a room composite, a series of 4 connected rooms
        string[][] normalRoomTypes = new string[][] {
            new string[] {"enemySmall"},
            new string[] {"enemySmall", "enemyLarge"},
            new string[] {"enemySmall", "enemyLarge"}
        };
        //types of composites that can load on each level, VERY VAGUE types
        string[][] specialRoomTypes = new string[][] {
            new string[] {"reward"},
            new string[] {"reward"},
            new string[] {"reward"}
        };
        //types of special composites that can load on each level, VERY VAGUE types
        Dictionary<string, int[]> roomIndexes = new Dictionary<string, int[]>{
            {"root", new int[] {0} },
            {"end", new int[] {1} },
            {"three", new int[] {2} },
            {"enemySmall", new int[] {3} },
            {"enemyLarge", new int[] {4} },
            {"reward", new int[] {5} }
        };
        List<string> roomSpawnList = new List<string>();
        switch(node.data) {
            case "root":
                //just the starting room
                roomSpawnList.Add("root");
                break;
            case "end":
                //just the end room
                roomSpawnList.Add("end");
                break;
            case "normal":
                //4-5 normal rooms
                roomSpawnList.Add("three");
                for(int i = 0; i < 3; i++) {
                    roomSpawnList.Add(normalRoomTypes[level][r.Next(0, normalRoomTypes[level].Length)]);
                    //add a random room of a valid type for that level
                }
                break;
            case "special":
                //4-5 normal rooms with a special room at the end
                roomSpawnList.Add("three");
                for(int i = 0; i < 3; i++) {
                    roomSpawnList.Add(normalRoomTypes[level][r.Next(0, normalRoomTypes[level].Length)]);
                }
                roomSpawnList.Add(specialRoomTypes[level][r.Next(0, specialRoomTypes[level].Length)]);
                break;
        }

        Vector3[] units = new Vector3[] {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
        List<GameObject> spawnedRooms = new List<GameObject>();
        List<string> spawnedRoomNames = new List<string>(){"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M"};
        GameObject compositeParent = new GameObject("composite parent for nodal type: " + node.data);


        int debugCounter = 10;
        bool noOverlaps = false;
        while(!noOverlaps && debugCounter > 0){
            int ij = 0;
            foreach(string roomType in roomSpawnList) {
                //spawn each room!
                GameObject newRoom = GameObject.Instantiate(roomPrefabs[roomIndexes[roomType][r.Next(0, roomIndexes[roomType].Length)]]);
                newRoom.name = spawnedRoomNames[ij];
                newRoom.transform.SetParent(compositeParent.transform);
                newRoom.GetComponent<Room>().Init();
                /*while (OverlapTransforms(newRoom, spawnedRooms, errorMargin : roomOverlapMargin)) {
                //while overlapping with any of the rooms in the composite, keep shifting room
                    int i = r.Next(0, units.Length);
                    newRoom.transform.position += units[i] * (i <= 1 ? newRoom.GetComponent<Room>().height : newRoom.GetComponent<Room>().width);
                }*/

                spawnedRooms.Add(newRoom);
                ij++;
            }

            bool[] swapFlags = new bool[spawnedRooms.Count - 1];

            for (int i = 0; i < spawnedRooms.Count - 1; i++) {
                swapFlags[i] = false;
                //line up doorways
                /*for each room except the last room, line up the exit with the next entrance. if those points are on incompatible room sides, swap the entrance and exit
                you can always just swap the entrance and exit because there is at most 1 incompatible side*/
                string startDir = spawnedRooms[i].GetComponent<Room>().doorSide[1];
                string endDir = spawnedRooms[i + 1].GetComponent<Room>().doorSide[0];
                if (startDir == endDir) {
                    GameObject debugger = GameObject.Instantiate(debugPrefab[2], spawnedRooms[i + 1].transform.position, Quaternion.identity);
                    debugger.transform.SetParent(compositeParent.transform);
                    debugger.name = spawnedRooms[i + 1].name + "tomato debugger";
                    //swap entrance and exit if incompatible
                    string temp = spawnedRooms[i + 1].GetComponent<Room>().doorSide[0];
                    spawnedRooms[i + 1].GetComponent<Room>().doorSide[0] = spawnedRooms[i + 1].GetComponent<Room>().doorSide[1];
                    spawnedRooms[i + 1].GetComponent<Room>().doorSide[1] = temp;
                    GameObject tempDoor = spawnedRooms[i + 1].GetComponent<Room>().doors[0];
                    spawnedRooms[i + 1].GetComponent<Room>().doors[0] = spawnedRooms[i + 1].GetComponent<Room>().doors[1];
                    spawnedRooms[i + 1].GetComponent<Room>().doors[1] = tempDoor;
                    endDir = spawnedRooms[i + 1].GetComponent<Room>().doorSide[0];
                    swapFlags[i] = true;
                }
                spawnedRooms[i + 1].transform.position = spawnedRooms[i].transform.position;
                if (startDir == "left" || startDir == "right") {
                    while(OverlapTransformsX(spawnedRooms[i], spawnedRooms[i + 1], errorMargin : roomOverlapMargin)) {
                        spawnedRooms[i + 1].transform.position += new Vector3(64 * (startDir == "left" ? -1 : 1), 0, 0);
                    }
                    if (endDir == "up" || endDir == "down") {
                        while(OverlapTransformsY(spawnedRooms[i], spawnedRooms[i + 1], errorMargin : roomOverlapMargin)) {
                            spawnedRooms[i + 1].transform.position += new Vector3(0, 64 * (endDir == "up" ? -1 : 1), 0);
                        }
                    }
                } else if (startDir == "up" || startDir == "down") {
                    while(OverlapTransformsY(spawnedRooms[i], spawnedRooms[i + 1], errorMargin : roomOverlapMargin)) {
                        spawnedRooms[i + 1].transform.position += new Vector3(0, 64 * (startDir == "up" ? 1 : -1), 0);
                    }
                    if (endDir == "left" || endDir == "right") {
                        while(OverlapTransformsX(spawnedRooms[i], spawnedRooms[i + 1], errorMargin : roomOverlapMargin)) {
                            spawnedRooms[i + 1].transform.position += new Vector3(64 * (endDir == "left" ? 1 : -1), 0, 0);
                        }
                    }
                }
                //smooth out small gaps!
                Vector3 startPos = spawnedRooms[i].GetComponent<Room>().doors[1].transform.position;
                Vector3 endPos = spawnedRooms[i + 1].GetComponent<Room>().doors[0].transform.position;
                if ((endPos.x - startPos.x) % hallwayWidth != 0) {
                    spawnedRooms[i + 1].transform.position += new Vector3((endPos.x > startPos.x ? -1 : 1) * Mathf.Abs((endPos.x - startPos.x) % hallwayWidth), 0, 0);
                }
                if ((endPos.y - startPos.y) % hallwayWidth != 0) {
                    spawnedRooms[i + 1].transform.position += new Vector3(0, (endPos.y > startPos.y ? -1 : 1) * Mathf.Abs((endPos.y - startPos.y) % hallwayWidth), 0);
                }
            }


            //connect rooms in composite by hallways
            //draw hallways directly into rooms
        
            for (int i = 0; i < spawnedRooms.Count - 1; i++) {
                string dir = spawnedRooms[i].GetComponent<Room>().doorSide[1];
                string endDir = spawnedRooms[i + 1].GetComponent<Room>().doorSide[0];
                Vector3 startPos = spawnedRooms[i].GetComponent<Room>().SideCoordsFromSide(spawnedRooms[i].GetComponent<Room>().doorSide[1]);
                Vector3 endPos = spawnedRooms[i + 1].GetComponent<Room>().SideCoordsFromSide(spawnedRooms[i + 1].GetComponent<Room>().doorSide[0]);
                Vector3 curr = startPos;
                if (swapFlags[i]) {
                    //in theory nothing needs to happen here bc the refs were swapped

                }
                if (dir == "right") {
                        curr += new Vector3(hallwayWidth, 0, 0);
                } else if (dir == "left") {
                        curr += new Vector3(-hallwayWidth, 0, 0);
                } else if (dir == "up") {
                        curr += new Vector3(0, hallwayWidth, 0);
                } else if (dir == "down") {
                        curr += new Vector3(0, -hallwayWidth, 0);
                }
                GameObject hallwayParent = new GameObject("hallwayParent");
                hallwayParent.transform.SetParent(compositeParent.transform);
                GameObject debug1 = GameObject.Instantiate(debugPrefab[0], startPos, Quaternion.identity);
                debug1.transform.SetParent(hallwayParent.transform);
                GameObject debug2 = GameObject.Instantiate(debugPrefab[1], endPos, Quaternion.identity);
                debug2.transform.SetParent(hallwayParent.transform);

                int debugCounter3 = 100;
                while (Mathf.Abs((curr - endPos).magnitude) > hallwayWidth && debugCounter3 > 0) {
                    //while not at end point
                    Vector3 transformation = new Vector3(1f, 1f, 1f);
                    if (dir == "right") {
                        transformation = new Vector3(hallwayWidth, 0, 0);
                    } else if (dir == "left") {
                        transformation = new Vector3(-hallwayWidth, 0, 0);
                    } else if (dir == "up") {
                        transformation = new Vector3(0, hallwayWidth, 0);
                    } else if (dir == "down") {
                        transformation = new Vector3(0, -hallwayWidth, 0);
                    }
                    GameObject newHallway = GameObject.Instantiate(hallwayPrefabs[dir == "right" || dir == "left" ? 0 : 1], curr + transformation, Quaternion.identity);
                    newHallway.name = spawnedRoomNames[i] + "hallway";
                    newHallway.transform.SetParent(hallwayParent.transform);
                    curr = newHallway.transform.position;
                    
                    int prefabIndex = 0;
                    bool flip = false;
                    if (dir == "left" || dir == "right") {
                        if (Mathf.Abs(curr.x - endPos.x) < hallwayWidth) {
                            //if done with this axis of movement
                            if (curr.y > endPos.y) {
                                if (dir == "left") {
                                    //left -> down
                                    prefabIndex = 2;
                                    flip = true;
                                } else if (dir == "right") {
                                    //right -> down
                                    prefabIndex = 2;

                                }
                                dir = "down";
                            } else {
                                if (dir == "left") {
                                    //left -> up
                                    prefabIndex = 3;
                                    flip = true;
                                } else if (dir == "right") {
                                    //right -> up
                                    prefabIndex = 3;
                                }
                                dir = "up";
                            }
                            //Destroy(newHallway);
                            newHallway = GameObject.Instantiate(hallwayPrefabs[prefabIndex], curr + transformation, Quaternion.identity);
                            newHallway.name = spawnedRoomNames[i] + "hallway corner";
                            newHallway.transform.SetParent(hallwayParent.transform);
                            curr = newHallway.transform.position;
                            newHallway.transform.localScale = new Vector3((flip ? 1 : -1) * newHallway.transform.localScale.x, newHallway.transform.localScale.y, newHallway.transform.localScale.z);
                        }
                    } else {
                        if (Mathf.Abs(curr.y - endPos.y) < hallwayWidth) {
                            //if done with this axis of movement
                            if (curr.x > endPos.x) {
                                if (dir == "up") {
                                    //up -> left
                                    prefabIndex = 2;
                                    flip = true;
                                } else if (dir == "down") {
                                    //down -> left
                                    prefabIndex = 3;
                                    flip = true;
                                }
                                dir = "left";
                            } else {
                                if (dir == "up") {
                                    //up -> right
                                    prefabIndex = 2;
                                } else if (dir == "down") {
                                    //down -> right
                                    prefabIndex = 3;
                                }
                                dir = "right";
                            }
                            //Destroy(newHallway);
                            newHallway = GameObject.Instantiate(hallwayPrefabs[prefabIndex], curr + transformation, Quaternion.identity);
                            newHallway.name = spawnedRoomNames[i] + "hallway corner";
                            newHallway.transform.SetParent(hallwayParent.transform);
                            curr = newHallway.transform.position;
                            newHallway.transform.localScale = new Vector3((flip ? 1 : -1) * newHallway.transform.localScale.x, newHallway.transform.localScale.y, newHallway.transform.localScale.z);
                        }
                    }
                    
                debugCounter3--;
                }
                print(debugCounter3);
            }

            foreach(GameObject room in spawnedRooms) {
                //if any rooms within the composite overlap with each other, regenerate the entire composite
                if (OverlapTransforms(room, spawnedRooms, errorMargin: roomOverlapMargin / 4)) {
                    noOverlaps = false;                        
                    break;
                } else {
                    noOverlaps = true;
                }
            }
            if (!noOverlaps) {
                Destroy(compositeParent);
                compositeParent = new GameObject("composite parent for nodal type: " + node.data);
                spawnedRooms = new List<GameObject>();
            }
            debugCounter--;
        }

        print(debugCounter);

        List<GameObject> roomsListVal = new List<GameObject>();
        foreach(GameObject obj in spawnedRooms) {
            roomsListVal.Add(obj);
        }
        float[] boundsVal = {roomsListVal[0].transform.position.x, roomsListVal[0].transform.position.x, roomsListVal[0].transform.position.y, roomsListVal[0].transform.position.y};
        foreach(GameObject obj in spawnedRooms) {
            Room objRoom = obj.GetComponent<Room>();
            if (obj.transform.position.x - objRoom.width / 2 < boundsVal[0]) {
                boundsVal[0] = obj.transform.position.x - objRoom.width / 2;
            }
            if (obj.transform.position.x + objRoom.width / 2 > boundsVal[1]) {
                boundsVal[1] = obj.transform.position.x + objRoom.width / 2 ;
            }
            if (obj.transform.position.y - objRoom.height / 2 < boundsVal[2]) {
                boundsVal[2] = obj.transform.position.y - objRoom.height / 2;
            }
            if (obj.transform.position.y + objRoom.height / 2 > boundsVal[3]) {
                boundsVal[3] = obj.transform.position.y + objRoom.height / 2;
            }
        }
        return (roomsListVal, boundsVal);
    }

    public void ChangeReticle(Sprite newReticle) {
        if (newReticle == null) {
            newReticle = itemSprites[itemIndices["blank"]];
        }
        reticle.ChangeReticle(newReticle);
    }

    public void ChangeReticle(string newReticle) {
        if (newReticle == null || newReticle == "") {
            newReticle = "blank";
        }
        reticle.ChangeReticle(itemSprites[itemIndices[newReticle]]);
    }

    public void EndDrag(bool term) {
        if (term) {
            player.inventory.EndDrag(true);
        }
    }
}
