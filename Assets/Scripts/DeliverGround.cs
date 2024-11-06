using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverGround : MonoBehaviour
{
    [Header("Components")]
    WaveController waveController;

    [Header("Delivery")]
    public Transform deliverPoint;
    public List<Transform> bodies = new List<Transform>();

    public Transform pivotPoint;

    // Start is called before the first frame update
    void Start()
    {
        waveController = FindFirstObjectByType<WaveController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.position = new Vector3(pivotPoint.position.x, 0.01f, pivotPoint.position.z);
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
            if (bodies.Count <= 0) return;

            int allMoney = 0;
            int moneyMultiplier = 1;

            BodyStacking playerStack = other.GetComponentInChildren<BodyStacking>();
            if (playerStack.bodies.Count > 0)
            {
                playerStack.ClearBodyCount();
            }

            if (bodies.Count > 0)
            {
                for (int i = 0; i < bodies.Count; i++)
                {
                    allMoney += bodies[i].GetComponentInParent<EnemyBehaviour>().myCost;
                    //if (bodies.Contains(bodies[i]))
                    //    bodies.Remove(bodies[i]);

                    Destroy(bodies[i].parent.gameObject);
                }

                moneyMultiplier = bodies.Count;
                other.GetComponent<PlayerValues>().AddMoney(allMoney * moneyMultiplier);

                waveController.CheckClearWave(bodies.Count);
                bodies.Clear();
            }

            //waveController.NextWave();
            playerStack.ClearDelivery();
        }
    }

    public void DeliverBodies(List<Transform> newBodies)
    {
        bodies.AddRange(newBodies);
    }
}
