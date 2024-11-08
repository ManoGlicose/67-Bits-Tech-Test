using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Components
    public Transform target;
    public Vector3 offset;

    [Header("Camera")]
    public List<Transform> cameraPositions = new List<Transform>();
    public Transform gamePosition;
    public bool isInStore = false;

    // Values
    public float smoothSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - gamePosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (GameController.Instance.GameHasStarted())
        {
            Vector3 desiredPosition = target.position - offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        else
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, cameraPositions[isInStore ? 1 : 0].position, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }

    public void SetRotation()
    {
        transform.rotation = gamePosition.rotation;
    }
}
