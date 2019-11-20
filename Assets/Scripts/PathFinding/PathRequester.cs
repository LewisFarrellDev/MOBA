using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

// Code adapated from Sebastion Lague's A* Pathfinding tutorial
// https://www.youtube.com/user/Cercopithecan 

public class PathRequester : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequester instance;
    int currentAlgorithimIndex = 0;
    BasePathalgorithm currentAlgorithm;
    BasePathalgorithm[] algorithms;
    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        algorithms = GetComponents<BasePathalgorithm>();
        currentAlgorithm = algorithms[currentAlgorithimIndex];
    }

    public void IncrementAlgorithmIndex()
    {
        currentAlgorithimIndex++;
        if (currentAlgorithimIndex > algorithms.Length - 1)
            currentAlgorithimIndex = 0;

        currentAlgorithm = algorithms[currentAlgorithimIndex];
    }

    public static void RequestPath(Vector3 start, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(start, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            currentAlgorithm.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success, double time)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
        {
            pathStart = start;
            pathEnd = end;
            this.callback = callback;
        }
    }
}
