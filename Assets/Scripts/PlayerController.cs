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

    // Attacks (my own system)
    public int numberOfClicks = 0;
    float resetComboTime = 0.5f;

    public float attackTimer = 0;

    private void Awake()
    {
        controls = new PlayerActions();
        mainCamera = Camera.main;

        controls.Controls.Attack.performed += ctx => CountAttacks();
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
            if (numberOfClicks <= 0)
            {
                float actualSpeed = inputDirection.magnitude > 0.6f ? speed : speed / 2;

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);

                Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

                controller.Move(moveDirection.normalized * actualSpeed * Time.deltaTime);
            }
        }

        #endregion

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else 
        { 
            attackTimer = 0;
            numberOfClicks = 0;
        }

        HandleAnimations();
    }

    void HandleAnimations()
    {
        //print(inputDirection.magnitude);
        animator.SetFloat("Speed", inputDirection.magnitude);

        //if (numberOfClicks >= 3 && attackTimer > 0) return;

        switch (numberOfClicks)
        {
            case 0:
                animator.SetBool("Hit1", false);
                animator.SetBool("Hit2", false);
                animator.SetBool("Hit3", false);
                break;
            case 1:
                animator.SetBool("Hit1", true);
                break;
            case 2:
                animator.SetBool("Hit2", true);
                break;
            case 3:
                animator.SetBool("Hit3", true);
                break;
            default:
                animator.SetBool("Hit1", false);
                animator.SetBool("Hit2", false);
                animator.SetBool("Hit3", false);
                break;
        }

        //animator.SetInteger("Hits", numberOfClicks);
    }

    void CountAttacks()
    {
        if(numberOfClicks < 3)
            numberOfClicks++;
        //float newWaitTimer = 0;

        //for (int i = 0; i < numberOfClicks; i++)
        //{
        //    switch (i)
        //    {
        //        case 1:
        //            newWaitTimer = 0.2f;
        //            break;
        //        case 2:
        //            newWaitTimer = 0.27f;
        //            break;
        //        case 3:
        //            newWaitTimer = 0.35f;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        SetAttackTimer(numberOfClicks < 3 ? 0.65f - (0.65f * 0.25f) : 0.9f);
        //print("Attack n. " + numberOfClicks.ToString() + " performed");
    }

    void SetAttackTimer(float newTime)
    {
        attackTimer = newTime;
    }

    #region Enable Disable Input System
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    #endregion
}
