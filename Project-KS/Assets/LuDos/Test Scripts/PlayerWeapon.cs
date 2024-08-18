using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    private void Start()
    {
        UnitController unitController = GetComponentInParent<UnitController>();

        if (unitController != null)
        {
            damage = unitController.damage;
        }
    }
}