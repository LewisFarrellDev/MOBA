using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { TeamA, TeamB, NoTeam };

public class MinionSpawner : MonoBehaviour
{
    public GameObject minionPrefab;
    public Team team;
    public int numMinionsToSpawn = 6;
    public float spawnRate = 1;

    public List<Transform> minionTargets = new List<Transform>();

    void Start()
    {
        StartCoroutine(SpawnMinions());
    }

    IEnumerator SpawnMinions()
    {
        while (true)
        {
            for (int i = numMinionsToSpawn; i > 0; i--)
            {
                GameObject minion = Instantiate(minionPrefab, transform.position, transform.rotation);

                FindPathState_Minion findPathState = minion.GetComponent<FindPathState_Minion>();
                findPathState.SetTargets(minionTargets);
                Entity entity = minion.GetComponent<Entity>();
                entity.team = team;

                minion.name = "M_" + entity.team.ToString() + "_" + (numMinionsToSpawn - i).ToString();


                yield return new WaitForSeconds(spawnRate);
            }
            yield return new WaitForSeconds(30);
        }
    }
}
