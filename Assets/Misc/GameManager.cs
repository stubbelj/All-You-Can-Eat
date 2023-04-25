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
    public List<string> itemNames = new List<string>{};
    public List<Sprite> itemSprites = new List<Sprite>();
    public Dictionary<string, int> itemIndices = new Dictionary<string, int>{
    };

    public System.Random r = new System.Random();

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
        //InitLevel();
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

        int[][] compositeQuantity = new int[][] {new int[] {1, 1}, new int[] {3, 1}, new int[] {6, 2}};
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
            (List<GameObject>, float[]) currComposite = GenerateComposite(curr);
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
                foreach (GameObject room in currComposite.Item1) {
                    //move each room in composite
                    room.transform.position += roomShift;
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
                    (List<GameObject>, float[]) adjComposite = GenerateComposite(node);
                    while (OverlapTransforms(adjComposite.Item2, spawnedCompositeBounds, errorMargin : compositeOverlapMargin) /*&& overlapDebugCounter2 > 0*/) {
                    
                        //check if newly spawned adj composite overlaps with any previous composites. if so, move it
                        float roomSizeShiftMod = 140f;
                        int j = r.Next(0, units.Length);
                        Vector3 roomShift = units[j] * roomSizeShiftMod;
                        foreach (GameObject room in adjComposite.Item1) {
                            //move each room in composite
                            room.transform.position += roomShift;
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

    (List<GameObject>, float[]) GenerateComposite(Node node) {
        float roomOverlapMargin = 500f;
        //minimum distance between rooms
        roomOverlapMargin *= 1;
        //generates a room composite, a series of 4 connected rooms
        string[][] normalCompositeTypes = new string[][] {
            new string[] {"enemySmall"},
            new string[] {"enemySmall", "enemyLarge"},
            new string[] {"enemySmall", "enemyLarge"}
        };
        //types of composites that can load on each level, VERY VAGUE types
        string[][] specialCompositeTypes = new string[][] {
            new string[] {"reward"},
            new string[] {"reward"},
            new string[] {"reward"}
        };
        //types of special composites that can load on each level, VERY VAGUE types
        Dictionary<string, int[]> roomIndexes = new Dictionary<string, int[]>{
            {"root", new int[] {0} },
            {"end", new int[] {1} },
            {"enemySmall", new int[] {2} },
            {"enemyLarge", new int[] {3} },
            {"reward", new int[] {4} }
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
                for(int i = 0; i < 4; i++) {
                    roomSpawnList.Add(normalCompositeTypes[level][r.Next(0, normalCompositeTypes[level].Length)]);
                    //add a random room of a valid type for that level
                }
                break;
            case "special":
                //4-5 normal rooms with a special room at the end
                for(int i = 0; i < 4; i++) {
                    roomSpawnList.Add(normalCompositeTypes[level][r.Next(0, normalCompositeTypes[level].Length)]);
                }
                roomSpawnList.Add(specialCompositeTypes[level][r.Next(0, specialCompositeTypes[level].Length)]);
                break;
        }
        Vector3[] units = new Vector3[] {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
        List<GameObject> spawnedRooms = new List<GameObject>();
        foreach(string roomType in roomSpawnList) {
            //spawn each room!
            GameObject newRoom = GameObject.Instantiate(roomPrefabs[roomIndexes[roomType][r.Next(0, roomIndexes[roomType].Length)]]);
            newRoom.name = "Composite of nodal type: " + node.data;
            while (OverlapTransforms(newRoom, spawnedRooms, errorMargin : roomOverlapMargin)) {
            
            //while overlapping with any of the rooms in the composite, keep shifting room
                int i = r.Next(0, units.Length);
                newRoom.transform.position += units[i] * (i <= 1 ? newRoom.GetComponent<Room>().height : newRoom.GetComponent<Room>().width);
            }

            spawnedRooms.Add(newRoom);
        }

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
