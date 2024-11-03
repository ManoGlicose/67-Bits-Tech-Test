using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Animator animator;

    [Header("AI")]
    public Transform target;
    public float attackRange = 1;

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

    [Header("Attack")]
    public List<Attacks> attackPoints = new List<Attacks>();
    public LayerMask enemyMask;
    bool canMove = true;
    bool canAttack = true;
    //int numberOfClicks = 0;

    float attackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement

        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Move();

        #endregion
    }

    void Move()
    {
        if (!target) return;
        direction = (target.position - transform.position);
        float distance = Vector3.Distance(transform.position, target.position);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime * Time.deltaTime);

        if (distance > attackRange)
        {
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            velocity.y += gravity * Time.deltaTime;

            if (canMove)
            {
                controller.Move(moveDirection.normalized * speed * Time.deltaTime);
                animator.SetFloat("Speed", 1);
            } else
                animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 0);

            if (canAttack)
                AttackPlayer();
        }

        controller.Move(velocity * Time.deltaTime);

        if(canMove)
            transform.rotation = Quaternion.Euler(0, angle, 0);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            canAttack = true;
            canMove = true;
        }
        else
            canMove = false;
    }

    void AttackPlayer()
    {
        if (!canAttack) return;

        animator.SetTrigger("Attack");
        canAttack = false;
    }

    public void Attack(int attackIndex)
    {
        Attacks thisAttack = attackPoints[attackIndex];

        Collider[] hitPlayers = Physics.OverlapSphere(thisAttack.attackPoint.position, thisAttack.attackRange, enemyMask);

        foreach (Collider item in hitPlayers)
        {
            if (item != null)
            {
                //print(item.name + " attacked with " + thisAttack.attackPoint.name);
                PlayerValues thisValue = item.GetComponentInParent<PlayerValues>();
                thisValue.Damage(thisAttack.attackDamage, 1.5f);
            }
        }
    }
}
