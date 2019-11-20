using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code adapated from Sebastion Lague's A* Pathfinding tutorial
// https://www.youtube.com/user/Cercopithecan 

public class Node
{
    public bool isWalkable;

    public Vector3 position;

    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public int gridZ;
    int heapIndex;

    public Node parent;

    public Node (bool isWalkable, Vector3 position, int gridX, int gridY, int gridZ)
    {
        this.isWalkable = isWalkable;
        this.position = position;

        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
            compare = hCost.CompareTo(nodeToCompare.hCost);

        return -compare;
    }
}
