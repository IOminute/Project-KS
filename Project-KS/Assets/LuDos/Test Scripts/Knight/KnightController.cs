using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KnightController : MonoBehaviour
{
    public float damage = 20f;

    public float maxHealth = 100f;
    private float currentHealth;

    private float runSpeed = 17f;
    private float dashSpeed = 40f;
    private float dashDuration = 0.4f;
    private float dashCooldown = 1f;
    private float attackCooldown = 3f;
    private float skillCooldown = 2f;
    private float lastAttackTime;
    private float lastSkillTime;
    private float comboResetTime = 1.0f;
    private int comboStep = 0;

    private bool isDashing;
    private bool isAttacking;
    private bool isUsingSkill;
    private bool isDead;
    private float lastDashTime;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private Animator animator;

    public Transform cameraTransform;

    public GameObject swordSlashPrefab;
    public Transform swordSpawnPoint;

    public Collider weaponColliderL;
    public Collider weaponColliderR;

    public Camera necroCamera;
    public Camera knightCamera;

    public Image clock;
    public Image healthBar;

    public List<GameObject> skillEffectPrefabs;
    public GameObject WeaponL;
    public GameObject WeaponR;

    public float lifeTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        lastAttackTime = -attackCooldown;
        lastSkillTime = -skillCooldown;

        DisabledWeaponCollider();

        StartCoroutine(ClockStart());
    }

    private void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleAttack();
        HandleSkill();
    }

    void EnabledWeaponCollider()
    {
        if (weaponColliderL != null)
        {
            weaponColliderL.enabled = true;
        }
        if (weaponColliderR != null)
        {
            weaponColliderR.enabled = true;
        }
    }
    void DisabledWeaponCollider()
    {
        if (weaponColliderL != null)
        {
            weaponColliderL.enabled = false;
        }
        if (weaponColliderR != null)
        {
            weaponColliderR.enabled = false;
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Death");

        necroCamera.gameObject.SetActive(true);
        knightCamera.gameObject.SetActive(false);
        enabled = false;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void EndPossession()
    {
        if (isDead) return;
        Necromancer.EndPossesion();
        Die();
    }

    private void HandleMovement()
    {
        if (isDashing || isAttacking || isUsingSkill) return;

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
            Vector3 effectPosition = transform.position;
            effectPosition.y += 3.0f;
            GameObject effectInstance = Instantiate(skillEffectPrefabs[4], effectPosition, transform.rotation);
            Destroy(effectInstance, 1.0f);
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
        if (isUsingSkill) yield break;

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
        if (isDashing || isUsingSkill || Time.time < lastAttackTime + attackCooldown) return;

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

            GameObject effectInstance = null;

            if (i == 0)
            {
                EnabledWeaponCollider();
                yield return new WaitForSeconds(0.3f);
                Quaternion rotation = WeaponR.transform.rotation * Quaternion.Euler(0, 180f, 0);

                effectInstance = Instantiate(skillEffectPrefabs[1], WeaponR.transform.position, rotation);

                yield return new WaitForSeconds(0.1f);
                DisabledWeaponCollider();
            }
            else if (i == 1)
            {
                EnabledWeaponCollider();
                yield return new WaitForSeconds(0.15f);
                Quaternion rotation = WeaponL.transform.rotation * Quaternion.Euler(0, 180f, 0);

                effectInstance = Instantiate(skillEffectPrefabs[1], WeaponL.transform.position, rotation);

                DisabledWeaponCollider();
            }
            else if(i == 2)
            {
                EnabledWeaponCollider();
                yield return new WaitForSeconds(0.25f);
                Vector3 effectPosition = transform.position;
                effectPosition.y += 3.0f;

                effectInstance = Instantiate(skillEffectPrefabs[2], effectPosition, transform.rotation);

                yield return new WaitForSeconds(0.15f);
                DisabledWeaponCollider();
            }

            if (effectInstance != null)
            {
                Destroy(effectInstance, 1.0f);
            }

            rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
        }

        Vector3 jumpDirection = transform.forward * 30f + Vector3.up * 10f;
        rb.velocity = jumpDirection;

        yield return new WaitForSeconds(0.4f);

        EnabledWeaponCollider();

        GameObject effect = Instantiate(skillEffectPrefabs[3], transform.position, transform.rotation);
        Destroy(effect, 1.0f);

        rb.velocity = new Vector3(rb.velocity.x, -30f, rb.velocity.z);
        yield return new WaitForSeconds(0.1f);

        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);
        DisabledWeaponCollider();
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    private void HandleSkill()
    {
        if (Time.time < lastSkillTime + skillCooldown || isAttacking || isDashing) return;

        if (Input.GetMouseButtonDown(1))
        {
            rb.velocity = Vector3.zero;
            lastSkillTime = Time.time;
            StartCoroutine(LaunchSwordSlash());
        }
    }

    private IEnumerator LaunchSwordSlash()
    {
        isUsingSkill = true;
        animator.SetBool("IsUsingSkill", true);

        Quaternion rotation = transform.rotation * Quaternion.Euler(0, -90f, 0);

        Vector3 effectPosition = transform.position;
        effectPosition += transform.forward * 2.5f;
        effectPosition.y += 3.0f;

        yield return new WaitForSeconds(0.7f);
        GameObject effect = Instantiate(skillEffectPrefabs[0], effectPosition, rotation);
        Destroy(effect, 1.0f);

        GameObject swordSlash = Instantiate(swordSlashPrefab, swordSpawnPoint.position, rotation);

        SwordSlash slashScript = swordSlash.GetComponent<SwordSlash>();

        if (slashScript != null)
        {
            slashScript.Initialize(transform.forward);
        }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsUsingSkill", false);
        isUsingSkill = false;
    }

    public IEnumerator ClockStart()
    {
        float realTime = 0f;
        while (realTime < lifeTime)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                yield break;
            }
            realTime += Time.deltaTime;
            clock.fillAmount = realTime / lifeTime;
            yield return null;
        }
        EndPossession();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon"))
        {
            EnemyWeapon weapon = other.GetComponent<EnemyWeapon>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
            }
        }
        else if (other.CompareTag("EnemyWeapon_Projectile"))
        {
            EnemyWeapon_Projectile weapon = other.GetComponent<EnemyWeapon_Projectile>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
                Destroy(other.gameObject);
            }
        }
    }
}