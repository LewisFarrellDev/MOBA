using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code adapated from Sebastion Lague's A* Pathfinding tutorial
// https://www.youtube.com/user/Cercopithecan 

public class AStar : BasePathalgorithm
{
    public override IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        double time = System.Environment.TickCount;
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node targetNode = grid.GetNodeFromWorldPosition(targetPos);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        if (!targetNode.isWalkable || !startNode.isWalkable)
            yield return null;

        // Lists to contain the nodes which have been explored/unexplored
        List<Node> unexplored = new List<Node>();
        HashSet<Node> explored = new HashSet<Node>();

        // Add the starting node
        unexplored.Add(startNode);

        // Loop while there are still nodes to be explored
        // if there are no nodes left every node has been explored
        // and the target should have been located
        while (unexplored.Count > 0)
        {
            // If the path request takes longer than 2 seconds, cancel out to prevent stalling
            if (System.Environment.TickCount >= time + 1000)
                break;

            // First node
            Node currentNode = unexplored[0];

            // Iterate through each node 
            // find the node with a lower fcost than the current node
            for (int i = 1; i < unexplored.Count; i++)
                if (unexplored[i].fCost < currentNode.fCost || unexplored[i].fCost == currentNode.fCost && unexplored[i].hCost < currentNode.hCost)
                    currentNode = unexplored[i];

            // Remove the node from the unexplored list
            unexplored.Remove(currentNode);

            // Add it to the explored list
            explored.Add(currentNode);

            // Is the node our target?? do something
            if (currentNode == targetNode)
            {
                pathSuccess = true;
                time = System.Environment.TickCount - time;
                break;
            }

            // Get all the neighbours
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                // Is the neighbour a valid node we can explore (Have we alrdy explored? is it walkable)
                if (!neighbour.isWalkable || explored.Contains(neighbour))
                    continue;

                int movementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (movementCostToNeighbour < neighbour.gCost || !unexplored.Contains(neighbour))
                {
                    neighbour.gCost = movementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!unexplored.Contains(neighbour))
                    {
                        unexplored.Add(neighbour);
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess)
            waypoints = TracePath(startNode, targetNode);

        pathRequester.FinishedProcessingPath(waypoints, pathSuccess, time);

    }

    public override void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
    {
        StartCoroutine(FindPath(pathStart, pathEnd));
    }

    public override Vector3[] TracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        grid.path = path;
        return waypoints;
    }

    public override Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld = Vector3.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 directionNew = new Vector3(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY, path[i - 1].gridZ - path[i].gridZ);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].position);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    public override int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        return distanceX * distanceX + distanceY * distanceY + distanceZ + distanceZ;

        //if (distanceX < distanceZ)
        //    return 14 * distanceX + 10 * (distanceZ - distanceX);
        //return 14 * distanceZ + 10 * (distanceX - distanceZ);
    }
}
