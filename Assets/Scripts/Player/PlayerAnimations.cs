using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(int attackNumber)
    {
        controller.Attack(attackNumber);
    }
}
