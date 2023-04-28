using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    //this doesn't go on an object, so it doesnt inherit from Monobehaviour. just a normal class. Use "using static Utils" to make use of stuff in it.
    //this class holds useful utility functions that are used across multiple scripts - easier than remembering which script had a function or just putting it all in GameManager

    public class Graph {
        public Node root = new Node();
    }

    public class Node {
        public List<Node> adj = new List<Node>();
        public string data;
        public Node(string newData = "") {
            data = newData;
        }
    }

    public static bool OverlapTransforms(float[] bound, List<float[]> boundList, float errorMargin = 0) {
        //checks if two rectangles overlap
        foreach (float[] testBound in boundList) {
            if (bound[1] < testBound[0] - errorMargin) { continue; }
            if (bound[0] > testBound[1] + errorMargin) { continue; }
            if (bound[3] < testBound[2] - errorMargin) { continue; }
            if (bound[2] > testBound[3] + errorMargin) { continue; }
            return true;
        }
        return false;
    }

    public static bool OverlapTransforms(GameObject gameObj, List<GameObject> gameObjList, float errorMargin = 0) {
        //checks if the bounds of two gameObjects overlap

        float[] bound = BoundsFromRoom(gameObj.GetComponent<Room>());
        List<float[]> boundList = new List<float[]>();
        foreach(GameObject obj in gameObjList) {
            if (obj != gameObj) {
                //dont check if an obj intersects with itself
                boundList.Add(BoundsFromRoom(obj.GetComponent<Room>()));
            }
        }
        foreach (float[] testBound in boundList) {
            if (bound[1] < testBound[0] - errorMargin) { continue; }
            if (bound[0] > testBound[1] + errorMargin) { continue; }
            if (bound[3] < testBound[2] - errorMargin) { continue; }
            if (bound[2] > testBound[3] + errorMargin) { continue; }
            return true;
        }
        return false;
    }

    public static bool OverlapTransformsX(GameObject a, GameObject b, float errorMargin = 0) {
        //checks if the bounds of two gameObjects overlap
        float[] boundA = BoundsFromRoom(a.GetComponent<Room>());
        float[] boundB = BoundsFromRoom(b.GetComponent<Room>());
        if (boundA[0] > boundB[0] - errorMargin && boundA[0] < boundB[1] + errorMargin) {
            return true;
        }
        return false;
    }

    public static bool OverlapTransformsY(GameObject a, GameObject b, float errorMargin = 0) {
        //checks if the bounds of two gameObjects overlap
        float[] boundA = BoundsFromRoom(a.GetComponent<Room>());
        float[] boundB = BoundsFromRoom(b.GetComponent<Room>());
        if (boundA[2] > boundB[2] - errorMargin && boundA[2] < boundB[3] + errorMargin) {
            return true;
        }
        return false;
    }

    public static float[] BoundsFromGameObject(GameObject gameObj) {
        //returns coords of the bounds of the gameObject's sprite
        //note that the use of this function for spacing out rooms in InitLevel() is a temporary solution, because the room will probably not just be one giant sprite
        return new float[]{gameObj.transform.position.x - gameObj.GetComponent<SpriteRenderer>().bounds.size.x / 2, gameObj.transform.position.x + gameObj.GetComponent<SpriteRenderer>().bounds.size.x / 2, gameObj.transform.position.y - gameObj.GetComponent<SpriteRenderer>().bounds.size.y / 2, gameObj.transform.position.y + gameObj.GetComponent<SpriteRenderer>().bounds.size.y / 2};
    }
    
    public static float[] BoundsFromRoom(Room room) {
        return room.GetBounds();
    }
}
