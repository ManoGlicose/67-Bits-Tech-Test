using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPile : MonoBehaviour
{
    public List<Rigidbody> bodies = new List<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].position = (new Vector3(transform.position.x, transform.position.y + (i * 0.8f), transform.position.z));
            bodies[i].rotation = transform.rotation;
        }
    }
}
