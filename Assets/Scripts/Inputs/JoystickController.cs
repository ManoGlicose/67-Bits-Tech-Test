using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class JoystickController : MonoBehaviour
{
    public FloatingJoystick joystick;
    public Vector2 joystickSize = new Vector2(100, 100);

    Finger movementFinger;
    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleFingerDown(Finger touchedFinger)
    {
        if(movementFinger == null && touchedFinger.screenPosition.x <= Screen.width / 2)
        {
            movementFinger = touchedFinger;
            movement = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.rect.sizeDelta = joystickSize;
            //joystick.rect.anchoredPosition = ClampStartPosirion(touchedFinger.screenPosition);
            joystick.rect.anchoredPosition = touchedFinger.screenPosition;
        }
    }

    Vector2 ClampStartPosirion(Vector2 startPosition)
    {
        if (startPosition.x < joystickSize.x / 2)
            startPosition.x = joystickSize.x / 2;

        if (startPosition.y < joystickSize.y / 2)
            startPosition.y = joystickSize.y / 2;
        else if (startPosition.y > Screen.height - joystickSize.y / 2)
            startPosition.y = Screen.height - joystickSize.y / 2;

        return startPosition;
    }

    void HandleFingerMove(Finger movedFinger)
    {
        if(movedFinger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, joystick.rect.anchoredPosition) > maxMovement)
                knobPosition = (currentTouch.screenPosition - joystick.rect.anchoredPosition).normalized * maxMovement;
            else
                knobPosition = currentTouch.screenPosition - joystick.rect.anchoredPosition;

            joystick.knob.anchoredPosition = knobPosition;
            movement = knobPosition / maxMovement;
        }
    }

    void HandleLoseFinger(Finger lostFinger)
    {
        if(lostFinger == movementFinger)
        {
            movementFinger = null;
            joystick.knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
            movement = Vector2.zero;
        }
    }

    public Vector2 MovementVector()
    {
        Vector2 newMovement = GameController.Instance.GameHasStarted() ? movement : Vector2.zero;

        return newMovement;
    }


    #region Enable Disable Inputs

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        EnhancedTouchSupport.Enable();
    }

    #endregion
}
