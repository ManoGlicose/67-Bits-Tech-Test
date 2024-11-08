using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingJoystick : MonoBehaviour
{
    [HideInInspector]
    public RectTransform rect;
    public RectTransform knob;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
}
