using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyStacking : MonoBehaviour
{
    [Header("Components")]
    public PlayerController player;
    Transform target;
    public DeliverGround deliverGround;

    public List<Transform> bodies = new List<Transform>();

    [Header("Parameters")]
    public float offset = 0.1f;
    public Vector2 rateRange = new Vector2(0.8f, 0.8f);

    // Start is called before the first frame update
    void Start()
    {
        target = transform;

        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].eulerAngles = new Vector3(90, 0, 90);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bodies.Count > 0)
            Wobble();
    }

    public void AddBodyToPile(Transform body)
    {
        if (bodies.Count >= player.GetPlayerValues().maxBodiesToCarry) return;

        if (!bodies.Contains(body))
            bodies.Add(body);
    }

    void Wobble()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            float rate = Mathf.Lerp(rateRange.x, rateRange.y, (float)i / (float)bodies.Count);
            bodies[i].position = Vector3.Lerp(bodies[i].position, i > 0 ? bodies[i - 1].position + (-bodies[i - 1].forward * offset) : target.position, rate);
            bodies[i].rotation = Quaternion.Lerp(bodies[i].rotation, i > 0 ? bodies[i - 1].rotation : target.GetChild(0).rotation, rate);
        }
    }

    public void ThrowBodies()
    {
        if (bodies.Count <= 0 || !deliverGround) return;

        target = deliverGround.deliverPoint;
        deliverGround.DeliverBodies(bodies);
    }

    public void ClearBodyCount()
    {
        bodies.Clear();
    }

    public void ClearDelivery()
    {
        target = transform;
        deliverGround = null;
    }
}
