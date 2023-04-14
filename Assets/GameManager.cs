using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager inst = null;

    public Player player;

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
        //InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void InitLevel(int level=0) {
        //initialize the floor layout for the level
        //first, create a sparse tree where nodes represent composites (chunks of several rooms) 
        //leaf nodes are composites that end in unique rooms (rewards, bosses,  shops) others are the normal gameplay loop
        //next, determine a composite to use for each node
        //generate each composite - necessitates generating rooms and connecting hallways
        //connect non-leaf composites by a single hallway at their origins/endpoints
        (int, int)[] compositeQuantity = new (int, int)[] {(1, 1), (3, 1), (6, 2)};
        //number of composites to load for each level BESIDES the start room, of normal and special types
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
        Graph graph = new Graph();
        graph.root.data = "root";
        int i = compositeQuantity[level].Item1;
        Node curr = graph.root;
        Node prev;
        while (i > 0) {
            //place normal nodes
            Node newNode = new Node(newData:"normal");
            curr.adj.Add(newNode);
            newNode.adj.Add(curr);
            curr = newNode;
            i--;
        }
        i = compositeQuantity[level].Item1;
        curr = graph.root.adj[0];
        while (i > 0) {
            //place special nodes
            Node newNode = new Node(newData:"special");
            curr.adj.Add(newNode);
            curr = curr.adj[0];
            i--;
        }
        print(curr.data);
        Node endNode = new Node(newData:"end");
        curr.adj.Add(endNode);
        endNode.adj.Add(curr);

        curr = graph.root;
        for(int j = 0; j < 2; j++) {
            print("adjacent nodes are: ");
            foreach(Node node in curr.adj) {
                print(node.data);
            }
            curr = curr.adj[0];
        }
    }*/
}
