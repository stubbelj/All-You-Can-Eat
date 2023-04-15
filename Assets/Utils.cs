using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
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

    public static bool OverlapTransforms(((float, float), (float, float)) transform, List<((float, float), (float, float))> transformList) {
        return true;
    }

    public static bool OverlapTransforms(GameObject gameObj, List<GameObject> gameObjList) {
        ((float, float), (float, float)) transform = BoundsFromGameObject(gameObj);
        List<((float, float), (float, float))> transformList = new List<((float, float), (float, float))>();
        foreach(GameObject obj in gameObjList) {
            transformList.Add(BoundsFromGameObject(obj));
        }
        return true;
    }

    public static ((float, float), (float, float)) BoundsFromGameObject(GameObject gameObj) {
        return ((gameObj.transform.position.x - gameObj.GetComponent<SpriteRenderer>().bounds.size.x / 2, gameObj.transform.position.x + gameObj.GetComponent<SpriteRenderer>().bounds.size.x / 2), (gameObj.transform.position.y - gameObj.GetComponent<SpriteRenderer>().bounds.size.y / 2, gameObj.transform.position.y + gameObj.GetComponent<SpriteRenderer>().bounds.size.y / 2));
    }
}
