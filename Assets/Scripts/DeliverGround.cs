using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverGround : MonoBehaviour
{
    [Header("Delivery")]
    public Transform deliverPoint;
    public List<Transform> bodies = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BodyStacking playerStack = other.GetComponentInChildren<BodyStacking>();
            playerStack.deliverGround = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BodyStacking playerStack = other.GetComponentInChildren<BodyStacking>();
            if (playerStack.bodies.Count > 0)
            {
                playerStack.ClearBodyCount();
            }

            if (bodies.Count > 0)
            {
                for (int i = 0; i < bodies.Count; i++)
                {
                    Destroy(bodies[i].parent.gameObject);
                }
                bodies.Clear();
            }

            // Sell bodies
            playerStack.deliverGround = null;
        }
    }

    public void DeliverBodies(List<Transform> newBodies)
    {
        bodies.AddRange(newBodies);
    }
}
