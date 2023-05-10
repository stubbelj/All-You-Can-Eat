using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile
{
    public static WorldTile[,] worldGraph = new WorldTile[300,300];
    //a tile of world space ~32 x 32 pixels

    public bool isTraversable = true;
    //if its a wall or other collidable, set to false
    //this is what you can use for the pathfinding
    public int[] coords = new int[2];
    //x and y coordinates of this worldTile in worldGraph

    void Init(bool newTraversable, int[] newCoords) {
        isTraversable = newTraversable;
        coords = newCoords;
    }
}
