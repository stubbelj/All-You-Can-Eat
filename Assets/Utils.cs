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

    public static bool OverlapTransforms(((float, float), (float, float)) bound, List<((float, float), (float, float))> boundList, float errorMargin = 0) {
        foreach (((float, float), (float, float)) testBound in boundList) {
            if (bound.Item1.Item2 < testBound.Item1.Item1 - errorMargin) { continue; }
            if (bound.Item1.Item1 > testBound.Item1.Item2 + errorMargin) { continue; }
            if (bound.Item2.Item2 < testBound.Item2.Item1 - errorMargin) { continue; }
            if (bound.Item2.Item1 > testBound.Item2.Item2 + errorMargin) { continue; }
            return true;
        }
        return false;
    }

    public static bool OverlapTransforms(GameObject gameObj, List<GameObject> gameObjList, float errorMargin = 0) {
        ((float, float), (float, float)) bound = BoundsFromGameObject(gameObj);
        List<((float, float), (float, float))> boundList = new List<((float, float), (float, float))>();
        foreach(GameObject obj in gameObjList) {
            boundList.Add(BoundsFromGameObject(obj));
        }
        foreach (((float, float), (float, float)) testBound in boundList) {
            if (bound.Item1.Item2 < testBound.Item1.Item1 - errorMargin) { continue; }
            if (bound.Item1.Item1 > testBound.Item1.Item2 + errorMargin) { continue; }
            if (bound.Item2.Item2 < testBound.Item2.Item1 - errorMargin) { continue; }
            if (bound.Item2.Item1 > testBound.Item2.Item2 + errorMargin) { continue; }
            return true;
        }
        return false;
    }

    public static ((float, float), (float, float)) BoundsFromGameObject(GameObject gameObj) {
        return ((gameObj.transform.position.x - gameObj.GetComponent<SpriteRenderer>().bounds.size.x / 2, gameObj.transform.position.x + gameObj.GetComponent<SpriteRenderer>().bounds.size.x / 2), (gameObj.transform.position.y - gameObj.GetComponent<SpriteRenderer>().bounds.size.y / 2, gameObj.transform.position.y + gameObj.GetComponent<SpriteRenderer>().bounds.size.y / 2));
    }
}
