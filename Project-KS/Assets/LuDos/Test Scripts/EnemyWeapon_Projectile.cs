using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon_Projectile : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    private float speed = 25f;
    private Transform targetTransform;
    Vector3 direction;

    public void Initialize(Transform target, GameObject unit)
    {
        targetTransform = target;

        EnemyController enemyController = unit.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            damage = enemyController.damage;
        }
    }
    private void Start()
    {
        Vector3 tmp = targetTransform.position;
        tmp.y += 2.5f;

        direction = (tmp - transform.position).normalized;
        // direction.y = targetTransform.position.y;
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += direction * speed * Time.deltaTime;
    }
}
