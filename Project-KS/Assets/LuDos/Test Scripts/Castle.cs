using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    bool isGameOver = false;
    float health = 1000;

    public Image healthBar;
    private void TakeDamage(float damageAmount)
    {
        if (!isGameOver)
        {
            health -= damageAmount;
            healthBar.fillAmount = health / 1000f;

            if (health <= 0)
            {
                health = 0;
                isGameOver = true;
                // �� �������� �ִϸ��̼�?
            }
        }
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
