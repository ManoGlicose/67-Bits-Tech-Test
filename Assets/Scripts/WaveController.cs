using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Header("Fighting Ring")]
    public List<Corner> corners = new List<Corner>();
    float lerpTime = 6;
    public Transform ringPivots;
    public float ringSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ringPivots.localScale = new Vector3(ringSize, 1, ringSize);

        for (int i = 0; i < corners.Count; i++)
        {
            corners[i].corner.position = Vector3.Lerp(corners[i].corner.position, corners[i].cornerPivot.position, lerpTime * Time.deltaTime);
        }
    }
}

[System.Serializable]
public class Corner
{
    public Transform corner;
    public Transform cornerPivot;
    public Transform spawnPoint;
}
