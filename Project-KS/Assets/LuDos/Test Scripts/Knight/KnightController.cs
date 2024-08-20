using UnityEngine;
using System.Collections;

public class KnightController : MonoBehaviour
{
    public float damage = 20f;
    private float runSpeed = 10f;
    private float dashSpeed = 30f;
    private float dashDuration = 0.4f;
    private float dashCooldown = 1f;

    private bool isDashing;
    private float lastDashTime;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private Animator animator;

    public Transform cameraTransform; // 카메라의 Transform 참조

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
        HandleSkill();
    }

    private void HandleMovement()
    {
        if (isDashing) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * moveVertical + right * moveHorizontal).normalized;

        // 대쉬 입력 처리
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
            return;
        }

        // 이동 입력이 있을 때 즉시 이동하고 애니메이션 변경
        if (moveDirection.magnitude > 0.1f)
        {
            animator.SetTrigger("Run");
            transform.rotation = Quaternion.LookRotation(moveDirection);
            Vector3 velocity = moveDirection * runSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        else
        {
            animator.SetTrigger("Idle");
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;
        lastDashTime = Time.time;

        Vector3 dashDirection = moveDirection;
        rb.velocity = dashDirection * dashSpeed;

        animator.SetTrigger("Dash");

        while (Time.time < startTime + dashDuration)
        {
            yield return null;
        }

        isDashing = false;
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void HandleSkill()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Skill");
        }
    }
}
