using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    private void Start()
    {
        damage = GetComponent<EnemyController>().damage;
    }
}