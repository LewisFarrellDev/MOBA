using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    private float maxHealth;
    public float health = 100;
    public Team team = Team.TeamA;
    public Slider healthBar;

    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }

    // Returns if the entity died on the damage dealt
    public bool TakeDamage(float damage)
    {
        health -= damage;

        if (GetComponent<StateManager_Scuttle>() != null)
            GetComponent<StateManager_Scuttle>().FuzzyLogic();

        if (health <= 0)
        {
            if (tag == "Base" || tag == "Player")
            {
                SceneManager.LoadScene(0);
            }
            return true;
        }

        return false;
    }

    public void AddHealth(float health)
    {
        this.health += health;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
