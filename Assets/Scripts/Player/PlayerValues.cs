using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    //PlayerController controller;

    [Header("Health")]
    [Range(0, 100)]
    public float maxHealth = 100;
    float health = 100;

    [Header("Weight")]
    public int maxBodiesToCarry = 2;

    bool hasDied = false;
    bool canBeDamaged = true;

    [Header("Money")]
    public int money;

    public bool HasDied()
    {
        return hasDied;
    }

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody[] rb = GetComponentsInChildren<Rigidbody>();

        health = maxHealth;

        foreach (Rigidbody item in rb)
        {
            item.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (!hasDied)
                Death();
        }
    }

    public void Damage(int damage, float damageDelay)
    {
        if (!canBeDamaged) return;
        health -= damage;
        print("I got hit");
        canBeDamaged = false;
        GetComponent<Animator>().SetTrigger(damage > 20 ? "Heavy Hit" : "Light Hit");

        StartCoroutine(DamageDelay(damageDelay));
    }

    public void Death()
    {
        if (hasDied) return;

        Rigidbody[] rb = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody item in rb)
        {
            item.isKinematic = false;
        }

        if (gameObject.CompareTag("Player"))
            GetComponent<PlayerController>().enabled = false;
        else if (gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = GetComponent<EnemyBehaviour>();
            enemy.Death();
        }

        GetComponent<CharacterController>().enabled = false;
        GetComponent<Animator>().enabled = false;

        hasDied = true;
    }

    public float GetHealth()
    {
        return health;
    }

    IEnumerator DamageDelay(float timer)
    {
        yield return new WaitForSeconds(timer);

        canBeDamaged = true;
    }

    public void AddMoney(int newMoney)
    {
        money += newMoney;
    }
}
