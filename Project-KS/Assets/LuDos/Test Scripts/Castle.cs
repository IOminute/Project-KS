using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Castle : MonoBehaviour
{
    public Image hpBar;

    bool isGameOver = false;
    float health = 1000;

    private void TakeDamage(float damageAmount)
    {
        if (!isGameOver)
        {
            health -= damageAmount;
            hpBar.DOFillAmount(health / 1000, 0.2f).SetEase(Ease.OutSine);
            if (health <= 0)
            {
                health = 0;
                isGameOver = true;
                // 성 무너지는 애니메이션?
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("PlayerWeapon_Projectile"))
        {
            PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
                Destroy(other.gameObject);
            }
        }
    }
}
