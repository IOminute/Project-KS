using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    private void Start()
    {
        damage = GetComponent<UnitController>().damage;
    }
}