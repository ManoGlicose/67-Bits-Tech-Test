using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rect;
    Rect safeArea;
    Vector2 _minAnchor;
    Vector2 _maxAnchor;

    private void Awake()
    {
        CalculateSafeArea();
    }

    void CalculateSafeArea()
    {
        rect = GetComponent<RectTransform>();
        safeArea = Screen.safeArea;
        _minAnchor = safeArea.position;
        _maxAnchor = _minAnchor + safeArea.size;
        _minAnchor.x /= Screen.width;
        _minAnchor.y /= Screen.height;
        _maxAnchor.x /= Screen.width;
        _maxAnchor.y /= Screen.height;
        rect.anchorMin = _minAnchor;
        rect.anchorMax = _maxAnchor;
    }
}
