using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager inst = null;

    public Player player;
    public List<GameObject> roomPrefabs = new List<GameObject>();

    int level = 0;
    System.Random r = new System.Random();

    void Awake() {
        if (inst == null) {
            inst = this;
        } else {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitLevel() {
        //initialize the floor layout for the level
        float compositeOverlapMargin = 0;
        //minimum distance between composites
        foreach(GameObject roomPref in roomPrefabs) {
            float newMargin = Mathf.Max(roomPref.GetComponent<SpriteRenderer>().sprite.bounds.size.x, roomPref.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
            if (newMargin > compositeOverlapMargin) { compositeOverlapMargin = newMargin; }
        }
        compositeOverlapMargin *= 5;
        print("compositeOverlapMargin: " + compositeOverlapMargin);
        //factor of how many max size rooms apart composites are

        (int, int)[] compositeQuantity = new (int, int)[] {(1, 1), (3, 1), (6, 2)};
        //number of composites to load for each level BESIDES the start room, of normal and special types
        Graph graph = new Graph();
        //in this graph, node.adj[0] is the "previous" node in the normal node branch and node.adj[1] is the next one
        graph.root.data = "root";
        int i = compositeQuantity[level].Item1;
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
        i = compositeQuantity[level].Item1;
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
        List<((float, float), (float, float))> spawnedCompositeBounds = new List<((float, float), (float, float))>();
        curr = graph.root.adj[0];
        //initialize curr as first normal node
        while(curr.data != "end") {
            //traverse through nodes, generating them as you go
            (List<GameObject>, ((float, float), (float, float))) currComposite = GenerateComposite(curr);
            //generate current normal composite
            //int overlapDebugCounter1 = 100;
            while (OverlapTransforms(currComposite.Item2, spawnedCompositeBounds, errorMargin : compositeOverlapMargin) /*&& overlapDebugCounter1 > 0*/) {
                //check if newly spawned composite overlaps with any previous composites. if so, move it
                /*if (true) {
                    print("overlapDebugCounter1: " + overlapDebugCounter1);
                    print(currComposite.Item2);
                    foreach(((float, float), (float, float)) boundsItem in spawnedCompositeBounds) {
                        print(boundsItem);
                    }
                    print(OverlapTransforms(currComposite.Item2, spawnedCompositeBounds));
                }*/
                float maxSizeInComposite = 0;
                foreach(GameObject room in currComposite.Item1) {
                    float newSize = Mathf.Max(room.GetComponent<SpriteRenderer>().sprite.bounds.size.x, room.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
                    if (newSize > maxSizeInComposite) { maxSizeInComposite = newSize; }
                }
                int j = r.Next(0, units.Length);
                Vector3 roomShift = units[j] * maxSizeInComposite;
                foreach (GameObject room in currComposite.Item1) {
                    //move each room in composite
                    room.transform.position += roomShift;
                }
                currComposite.Item2.Item1.Item1 += roomShift.x;
                currComposite.Item2.Item1.Item2 += roomShift.x;
                currComposite.Item2.Item2.Item1 += roomShift.y;
                currComposite.Item2.Item2.Item2 += roomShift.y;
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
                    (List<GameObject>, ((float, float), (float, float))) adjComposite = GenerateComposite(node);
                    while (OverlapTransforms(adjComposite.Item2, spawnedCompositeBounds, errorMargin : compositeOverlapMargin) /*&& overlapDebugCounter2 > 0*/) {
                    
                        //check if newly spawned adj composite overlaps with any previous composites. if so, move it
                        float maxSizeInComposite = 0;
                        foreach(GameObject room in adjComposite.Item1) {
                            float newSize = Mathf.Max(room.GetComponent<SpriteRenderer>().sprite.bounds.size.x, room.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
                            if (newSize > maxSizeInComposite) { maxSizeInComposite = newSize; }
                        }
                        int j = r.Next(0, units.Length);
                        Vector3 roomShift = units[j] * maxSizeInComposite;
                        foreach (GameObject room in adjComposite.Item1) {
                            //move each room in composite
                            room.transform.position += roomShift;
                        }
                        adjComposite.Item2.Item1.Item1 += roomShift.x;
                        adjComposite.Item2.Item1.Item2 += roomShift.x;
                        adjComposite.Item2.Item2.Item1 += roomShift.y;
                        adjComposite.Item2.Item2.Item2 += roomShift.y;
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

    (List<GameObject>, ((float, float), (float, float))) GenerateComposite(Node node) {
        float roomOverlapMargin = 0;
        //minimum distance between rooms
        foreach(GameObject roomPref in roomPrefabs) {
            float newMargin = Mathf.Max(roomPref.GetComponent<SpriteRenderer>().sprite.bounds.size.x, roomPref.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
            if (newMargin > roomOverlapMargin) { roomOverlapMargin = newMargin; }
        }
        roomOverlapMargin *= 0;
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
                    //add a random composite of a valid type for that level
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
                newRoom.transform.position += units[i] * (i <= 1 ? newRoom.GetComponent<SpriteRenderer>().bounds.size.y : newRoom.GetComponent<SpriteRenderer>().bounds.size.x);
            }

            spawnedRooms.Add(newRoom);
        }

        List<GameObject> roomsListVal = new List<GameObject>();
        foreach(GameObject obj in spawnedRooms) {
            roomsListVal.Add(obj);
        }
        ((float, float), (float, float)) boundsVal = ((roomsListVal[0].transform.position.x, roomsListVal[0].transform.position.x), (roomsListVal[0].transform.position.y, roomsListVal[0].transform.position.y));
        foreach(GameObject obj in spawnedRooms) {
            if (obj.transform.position.x - obj.GetComponent<SpriteRenderer>().bounds.size.x / 2 < boundsVal.Item1.Item1) {
                boundsVal.Item1.Item1 = obj.transform.position.x - obj.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            }
            if (obj.transform.position.x + obj.GetComponent<SpriteRenderer>().bounds.size.x / 2 > boundsVal.Item1.Item2) {
                boundsVal.Item1.Item2 = obj.transform.position.x + obj.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            }
            if (obj.transform.position.y - obj.GetComponent<SpriteRenderer>().bounds.size.y / 2 < boundsVal.Item2.Item1) {
                boundsVal.Item2.Item1 = obj.transform.position.y - obj.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            }
            if (obj.transform.position.y + obj.GetComponent<SpriteRenderer>().bounds.size.y / 2 > boundsVal.Item2.Item2) {
                boundsVal.Item1.Item2 = obj.transform.position.y + obj.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            }
        }
        return (roomsListVal, boundsVal);

    }
}
