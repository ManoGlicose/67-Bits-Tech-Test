using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    [Header("Health")] [Range(0, 100)]
    public int health = 100;

    bool hasDied = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody[] rb = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody item in rb)
        {
            item.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            if (!hasDied)
                Death();
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
    }

    public void Death()
    {
        Rigidbody[] rb = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody item in rb)
        {
            item.isKinematic = false;
        }

        GetComponentInChildren<Animator>().enabled = false;

        hasDied = true;
    }
}
