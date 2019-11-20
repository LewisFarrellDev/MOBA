using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code adapated from Sebastion Lague's A* Pathfinding tutorial
// https://www.youtube.com/user/Cercopithecan 

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector3 gridSize;
    public float nodeRadius;
    public float GizmoScale = 1;
    Node[,,] grid;
    public List<Node> path;

    int gridX;
    int gridY;
    int gridZ;
    float nodeDiameter;

    void Awake()
    {
        // Set the full size of the node
        nodeDiameter = nodeRadius * 2;

        // Define how many nodes can fit onto the grid
        // Round to int because we cannot have half a node
        gridX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        gridZ = Mathf.RoundToInt(gridSize.z / nodeDiameter);

        // Create the grid
        CreateGrid();
    }

    void CreateGrid()
    {
        // Create new grid of nodes with values defined in start
        grid = new Node[gridX, gridY, gridZ];

        // Find the bottom left node, this will be used as a reference point for calculating every other node
        Vector3 bottomLeft = transform.position - (Vector3.right * gridSize.x / 2) - (Vector3.up * gridSize.y / 2) - (Vector3.forward * gridSize.z / 2);

        // Iterate through potential node spot per axis
        for (int x = 0; x < gridX; x++)
        {
            for(int y = 0; y < gridY; y++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    // Calculate the node position based on the bottomLeft node
                    Vector3 nodePosition = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);

                    // Check if the node is walkable using the layermask on the sphereraycast
                    bool isWalkable = !Physics.CheckSphere(nodePosition, nodeRadius, unwalkableMask);

                    // Create the node
                    grid[x, y, z] = new Node(isWalkable, nodePosition, x, y, z);
                }
            }
        }
    }

    public int MaxSize
    {
        get
        {
            return gridX * gridY * gridZ;
        }
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        // Use percent to calculate how far from the center point they the agent is
        // 0.5 is center. 1 is far right, 0 is far left
        // We can use this to get the node from the array
        float percentX = (worldPosition.x + gridSize.x / 2 - transform.position.x) / gridSize.x;
        float percentY = (worldPosition.y + gridSize.y / 2 - transform.position.y) / gridSize.y;
        float percentZ = (worldPosition.z + gridSize.z / 2 - transform.position.z) / gridSize.z;


        // Clamp the values
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        // Rounbd the values
        int x = Mathf.RoundToInt((gridX - 1) * percentX);
        int y = Mathf.RoundToInt((gridY - 1) * percentY);
        int z = Mathf.RoundToInt((gridZ - 1) * percentZ);

        return grid[x, y, z];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <=1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    // Check if its the center node (the node passed in)
                    if (x == 0 && y == 0 && z == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    // Check if its within the grid
                    if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY && checkZ >= 0 && checkZ < gridZ)
                    {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }

        return neighbours;
    }

    void OnDrawGizmos1()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                // Ternary operator to check if the node is walkable, Change colour accordingly
                Gizmos.color = (node.isWalkable) ? new Color(1,1,1,0) : new Color(1, 0, 0, 1);

                // Check if the player is on this node, if true change the colour to blue
                //if (playernode == node)
                    //Gizmos.color = Color.blue;

                //if (path != null)
                //    if (path.Contains(node))
                //        Gizmos.color = Color.black;

                // Draw the grid
                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter * GizmoScale));
            }
        }
    }
}
