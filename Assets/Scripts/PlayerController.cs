using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Components
    public CharacterController controller;
    PlayerActions controls;
    public Animator animator;
    Camera mainCamera;

    // Values
    public float speed = 6;

    Vector2 inputDirection;
    Vector3 direction;
    public float turnSmoothTime = 0.25f;
    float turnSmoothVelocity;

    private void Awake()
    {
        controls = new PlayerActions();
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = controls.Controls.Move.ReadValue<Vector2>();
        direction = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;

        #region Move and Rotate

        if (direction.magnitude > 0.1f)
        {

            float actualSpeed = inputDirection.magnitude > 0.6f ? speed : speed / 2;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            controller.Move(moveDirection.normalized * actualSpeed * Time.deltaTime);
        }

        #endregion

        HandleAnimations();
    }

    void HandleAnimations()
    {
        //print(inputDirection.magnitude);
        animator.SetFloat("Speed", inputDirection.magnitude);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
