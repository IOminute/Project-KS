using UnityEngine;
using System.Collections;
public class KnightController : MonoBehaviour
{
    private float runSpeed = 10f;
    private float dashSpeed = 30f;
    private float dashDuration = 0.4f;
    private float dashCooldown = 1f;

    private bool isDashing;
    private float lastDashTime;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private Animator animator;

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

        moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // Shift 키를 눌렀을 때 대쉬 실행
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
            return;
        }

        // WASD 키를 눌렀을 때 Run 애니메이션 실행
        if (moveDirection.magnitude > 0.1f)
        {
            print("Run");
            animator.SetTrigger("Run");
        }
        else
        {
            print("Idle");
            animator.SetTrigger("Idle");
        }

        // 캐릭터 이동 처리
        float currentSpeed = runSpeed;

        Vector3 velocity = moveDirection * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1440 * Time.deltaTime);
        }

        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;
        lastDashTime = Time.time;

        Vector3 dashDirection = moveDirection;
        rb.velocity = dashDirection * dashSpeed;

        animator.SetTrigger("Dash");  // Dash 애니메이션 트리거 설정

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
            animator.SetTrigger("Attack");  // Attack 애니메이션 트리거 설정
            // 공격 로직 추가
        }
    }

    private void HandleSkill()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Skill");  // Skill 애니메이션 트리거 설정
            // 스킬 로직 추가
        }
    }
}