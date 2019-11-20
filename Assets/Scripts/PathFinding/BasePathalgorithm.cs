using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePathalgorithm : MonoBehaviour
{
    [HideInInspector]
    public PathRequester pathRequester;
    [HideInInspector]
    public Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
        pathRequester = GetComponent<PathRequester>();
    }

    public abstract IEnumerator FindPath(Vector3 startPos, Vector3 targetPos);

    public abstract void StartFindPath(Vector3 pathStart, Vector3 pathEnd);

    public abstract Vector3[] TracePath(Node startNode, Node targetNode);

    public abstract Vector3[] SimplifyPath(List<Node> path);

    public abstract int GetDistance(Node nodeA, Node nodeB);

}
