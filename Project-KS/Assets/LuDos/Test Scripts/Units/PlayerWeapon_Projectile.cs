using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon_Projectile : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    private float speed = 25f;
    private Transform targetTransform;
    Vector3 direction;

    public void Initialize(Transform target, GameObject unit)
    {
        targetTransform = target;

        UnitController unitController = unit.GetComponent<UnitController>();
        if (unitController != null)
        {
            damage = unitController.damage;
        }
    }
    private void Start()
    {
        if (targetTransform != null)
        {
            Vector3 tmp = targetTransform.position;
            tmp.y += 2.5f;

            direction = (tmp - transform.position).normalized;
            Destroy(gameObject, 5.0f);
        }
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
