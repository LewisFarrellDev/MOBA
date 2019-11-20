using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float AOEDamage = 10;
    Entity playerEntity;

    // Use this for initialization
    void Start()
    {
        playerEntity = GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            OverpoweredAOEAttack();
        }

        if (Input.GetKeyDown("2"))
        {
            playerEntity.AddHealth(20);
        }
    }

    void OverpoweredAOEAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.5f);
        foreach (Collider collider in hitColliders)
        {
            Entity entity;
            entity = collider.gameObject.GetComponentInParent<Entity>();

            if (entity == null || entity.team == GetComponent<Entity>().team)
                continue;


            if (entity.TakeDamage(AOEDamage))
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
