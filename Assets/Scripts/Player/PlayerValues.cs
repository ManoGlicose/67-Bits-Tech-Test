using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    //PlayerController controller;

    [Header("Health")]
    [Range(0, 100)]
    public int maxHealth = 100;
    int health = 100;

    bool hasDied = false;
    bool canBeDamaged = true;

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
            GetComponent<EnemyBehaviour>().enabled = false;

        GetComponent<Animator>().enabled = false;

        hasDied = true;
    }

    IEnumerator DamageDelay(float timer)
    {
        yield return new WaitForSeconds(timer);

        canBeDamaged = true;
    }
}
