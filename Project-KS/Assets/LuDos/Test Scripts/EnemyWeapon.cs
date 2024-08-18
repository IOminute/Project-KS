using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    private void Start()
    {
        EnemyController enemyController = GetComponentInParent<EnemyController>();

        if (enemyController != null)
        {
            damage = enemyController.damage;
        }
    }
}