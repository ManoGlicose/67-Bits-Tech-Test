using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Components
    [Header("Components")]
    public CharacterController controller;
    PlayerActions controls;
    public Animator animator;
    Camera mainCamera;

    // Values
    [Header("Values")]
    public float speed = 6;

    Vector2 inputDirection;
    Vector3 direction;
    float turnSmoothTime = 15f;
    float turnSmoothVelocity;

    // Attacks (my own system)
    bool canAttack = true;
    int numberOfClicks = 0;

    float attackTimer = 0;
    public float delayAttackTimer = 0.5f;

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
            float actualSpeed = inputDirection.magnitude > 0.6f ? speed : speed / 2;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime * Time.deltaTime);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            if (attackTimer <= 0)
            {
                transform.rotation = Quaternion.Euler(0, angle, 0);
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
            //canAttack = true;
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
            case 4:
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
        if (!canAttack) return;

        if (numberOfClicks < 4)// && canAttack)
        {
            //canAttack = true;
            numberOfClicks++;
        }
        else 
        { 
            canAttack = false;
            StartCoroutine(DelayAttack(1.05f + (1.05f * 0.1f) + delayAttackTimer));
        }

        SetAttackTimer(numberOfClicks < 3 ? 0.65f : 1.05f + (1.05f * 0.1f));
        //print("Attack n. " + numberOfClicks.ToString() + " performed");
    }

    void SetAttackTimer(float newTime)
    {
        if (numberOfClicks > 3) return;
        attackTimer = newTime;
    }

    IEnumerator DelayAttack(float timer)
    {
        yield return new WaitForSeconds(timer);

        canAttack = true;
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
