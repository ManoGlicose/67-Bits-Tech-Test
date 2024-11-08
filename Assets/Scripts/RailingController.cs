using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailingController : MonoBehaviour
{
    [Tooltip("Only two corners allowed. Otherwise it will not work properly")]
    public List<Transform> corners = new List<Transform>();

    // Update is called once per frame
    void Update()
    {
        if(corners.Count == 2)
            SetRailingDimensions();
    }

    float CalculateCornersDistance()
    {
        float distance = 0;
        distance = Vector3.Distance(corners[0].position, corners[1].position);
        return distance;
    }

    void SetRailingDimensions()
    {
        transform.position = (corners[0].position + corners[1].position) / 2;
        transform.localScale = new Vector3(CalculateCornersDistance(), corners[0].transform.localScale.y * 2, 0.1f);
    }
}
