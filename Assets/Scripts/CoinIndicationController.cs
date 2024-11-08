using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinIndicationController : MonoBehaviour
{
    [Header("Components")]
    public CanvasGroup alpha;
    public TMP_Text counter;

    [Header("Movement")]
    public float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        ControlMovement();
    }

    void ControlMovement()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
        transform.LookAt(Camera.main.transform, Vector3.up);
    }

    public void SetText(int value, int multiplier)
    {
        counter.text = value.ToString() + " x " + multiplier.ToString();
    }
}
