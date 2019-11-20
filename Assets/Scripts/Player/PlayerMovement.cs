using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask walkableLayer;
    FindPathState_Minion findPath;

    float clickRate = 0.25f;
    float lastClick = 0;

    // Use this for initialization
    void Start()
    {
        findPath = GetComponent<FindPathState_Minion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time < lastClick + clickRate)
            {
                return;
            }

            lastClick = Time.time;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Walkable"))
                {
                    findPath.StopFollowPath();
                    findPath.FindPath(hit.point);
                }
            }
        }
    }
}

