using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Animator animator;
    PlayerValues health;

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

    //Vector2 inputDirection;
    Vector3 direction;
    float turnSmoothTime = 7.5f;
    float turnSmoothVelocity;

    [Header("Attack")]
    public List<Attacks> attackPoints = new List<Attacks>();
    public LayerMask enemyMask;
    bool canMove = true;
    bool canAttack = true;
    //int numberOfClicks = 0;

    //float attackTimer = 0;

    [Header("Death")]
    public Collider collectTrigger;
    bool hasDied;
    public Transform hips;

    [Header("Value")]
    public int myCost = 10;

    [Header("UI")]
    public Transform canvas;
    public Image healthBar;
    public Image damageBar;

    // Start is called before the first frame update
    void Awake()
    {
        health = GetComponent<PlayerValues>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasDied) return;
        #region Movement

        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Move();

        #endregion


        hasDied = health.HasDied();
    }

    private void LateUpdate()
    {
        HandleUI();
    }

    public void SetParameters(Transform newTarget, float newHealth)
    {
        target = newTarget;
        health.maxHealth = newHealth;
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

    void HandleUI()
    {
        float thisHealth = (float)(health.GetHealth() / health.maxHealth);
        if (!canMove)
            damageBar.fillAmount = Mathf.Lerp(damageBar.fillAmount, healthBar.fillAmount, 5 * Time.deltaTime);

        healthBar.fillAmount = thisHealth;

        canvas.LookAt(Camera.main.transform, Vector3.up);
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

    public void Death()
    {
        if (hasDied) return;

        FindFirstObjectByType<WaveController>().KillEnemy();
        StartCoroutine(CollectDelay(3));
        hasDied = true;
    }

    IEnumerator CollectDelay(float timer)
    {
        yield return new WaitForSeconds(timer);

        collectTrigger.enabled = true;
        canvas.gameObject.SetActive(false);
        // Make collider available
    }

    public PlayerValues GetHealth()
    {
        return health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasDied) return;
        if (other.CompareTag("Player"))
        {
            // Add body to stack
            hips.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<PlayerController>().GetBodyStack().AddBodyToPile(hips);
        }
    }
}
