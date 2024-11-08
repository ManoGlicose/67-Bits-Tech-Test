using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliverGround : MonoBehaviour
{
    [Header("Components")]
    WaveController waveController;

    [Header("Delivery")]
    public Transform deliverPoint;
    public List<Transform> bodies = new List<Transform>();

    public Transform pivotPoint;

    [Header("UI")]
    public CoinIndicationController coinText;


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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BodyStacking playerStack = other.GetComponentInChildren<BodyStacking>();
            playerStack.deliverGround = this;

            playerStack.ThrowBodies();
        }
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

                    Destroy(bodies[i].parent.gameObject);
                }

                moneyMultiplier = bodies.Count;
                other.GetComponent<PlayerValues>().AddMoney(allMoney * moneyMultiplier);

                ShowValueSpent(new Vector3(other.transform.position.x, other.transform.position.y + 3, other.transform.position.z), allMoney, moneyMultiplier);

                waveController.CheckClearWave(bodies.Count);
                bodies.Clear();
            }

            playerStack.ClearDelivery();
        }
    }

    void ShowValueSpent(Vector3 position, int money, int multiplier)
    {
        GameObject valueText = Instantiate(coinText.gameObject, position, transform.rotation, null);
        valueText.GetComponent<CoinIndicationController>().SetText(money, multiplier);
    }

    public void DeliverBodies(List<Transform> newBodies)
    {
        bodies.AddRange(newBodies);
    }
}
