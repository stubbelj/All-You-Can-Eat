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
}
