using UnityEngine;
using System.Collections;

public class KnightController : MonoBehaviour
{
    public float damage = 20f;
    private float runSpeed = 15f;
    private float dashSpeed = 40f;
    private float dashDuration = 0.4f;
    private float dashCooldown = 1f;
    private float attackCooldown = 3f;
    private float lastAttackTime;

    private bool isDashing;
    private bool isAttacking;
    private float lastDashTime;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private Animator animator;

    public Transform cameraTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
        HandleSkill();
    }

    private void HandleMovement()
    {
        if (isDashing || isAttacking) return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        moveDirection = (forward * moveVertical + right * moveHorizontal).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
            return;
        }

        if (moveDirection.magnitude > 0.1f)
        {
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsIdle", false);
            transform.rotation = Quaternion.LookRotation(moveDirection);
            Vector3 velocity = moveDirection * runSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsIdle", true);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;
        lastDashTime = Time.time;

        Vector3 dashDirection = moveDirection.magnitude > 0.1f ? moveDirection : transform.forward;
        rb.velocity = dashDirection * dashSpeed;

        animator.SetBool("IsDashing", true);

        while (Time.time < startTime + dashDuration)
        {
            yield return null;
        }

        animator.SetBool("IsDashing", false);
        isDashing = false;
    }

    private void HandleAttack()
    {
        if (isDashing || Time.time < lastAttackTime + attackCooldown) return;

        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            rb.velocity = Vector3.zero;
            lastAttackTime = Time.time;
            animator.SetBool("IsAttacking", true);
            StartCoroutine(AttackAndDash());
        }
    }

    private IEnumerator AttackAndDash()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 dashDirection = transform.forward;
            rb.velocity = dashDirection * 30f;

            if (i == 0)
            {
                yield return new WaitForSeconds(0.3f);
            }
            if (i == 1)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (i == 2)
            {
                yield return new WaitForSeconds(0.4f);
            }

            rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    private void HandleSkill()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isUsingSkill", true);
            StartCoroutine(ResetSkillTrigger());
        }
    }

    private IEnumerator ResetSkillTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isUsingSkill", false);
    }
}