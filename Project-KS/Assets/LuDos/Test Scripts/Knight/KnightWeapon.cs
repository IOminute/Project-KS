using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightWeapon : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    private void Start()
    {
        KnightController knightController = GetComponentInParent<KnightController>();

        if (knightController != null)
        {
            damage = knightController.damage;
        }
        else
        {
            damage = 800f;
        }
    }
}
