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

    // Movement
    [Header("Movement")]
    public float speed = 6;
    // Gravity
    public float gravity = -9.81f;
    Vector3 velocity;
    float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;

    Vector2 inputDirection;
    Vector3 direction;
    float turnSmoothTime = 15f;
    float turnSmoothVelocity;

    // Attacks (my own system)
    [Header("Attack")]
    public List<Attacks> attackPoints = new List<Attacks>();
    public LayerMask enemyMask;
    bool canMove = true;
    bool canAttack = true;
    int numberOfClicks = 0;

    float attackTimer = 0;

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
        #region Move and Rotate

        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        inputDirection = controls.Controls.Move.ReadValue<Vector2>();
        direction = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;

        if (direction.magnitude > 0.1f)
        {
            float actualSpeed = inputDirection.magnitude > 0.6f ? speed : speed / 2;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime * Time.deltaTime);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            velocity.y += gravity * Time.deltaTime;

            if (canMove)
            {
                transform.rotation = Quaternion.Euler(0, angle, 0);
                controller.Move(moveDirection.normalized * actualSpeed * Time.deltaTime);
                controller.Move(velocity * Time.deltaTime);
            }
        }

        #endregion

        #region Attack controls

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            if(numberOfClicks > 0)
                canAttack = false;

            attackTimer = 0;
            numberOfClicks = 0;

        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            canAttack = true;
            canMove = true;
        }
        else
            canMove = false;

        #endregion

        HandleAnimations();
    }

    void HandleAnimations()
    {
        animator.SetFloat("Speed", inputDirection.magnitude);

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
    }

    public void Attack(int attackIndex)
    {
        Attacks thisAttack = attackPoints[attackIndex];

        Collider[] hitEnemies = Physics.OverlapSphere(thisAttack.attackPoint.position, thisAttack.attackRange, enemyMask);

        foreach (Collider item in hitEnemies)
        {
            print("Enemy attacked");
            PlayerValues thisValue = item.GetComponentInParent<PlayerValues>();
            thisValue.Damage(thisAttack.attackDamage);
        }
    }

    void CountAttacks()
    {
        if (!canAttack) return;

        if (numberOfClicks < 3)
        {
            numberOfClicks++;
            SetAttackTimer(numberOfClicks < 3 ? 0.5f : 1f);
        }
    }

    void SetAttackTimer(float newTime)
    {
        if (numberOfClicks > 3) return;
        attackTimer = newTime;
    }

    public void SetCanAttack(bool value)
    {
        canAttack = value;
    }

    IEnumerator DelayAttack(float timer)
    {
        yield return new WaitForSeconds(timer);

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

[System.Serializable]
public class Attacks
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
}
