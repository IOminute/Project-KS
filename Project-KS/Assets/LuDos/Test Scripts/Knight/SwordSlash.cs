using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public float speed = 20f;
    private Vector3 direction;

    public void Initialize(Vector3 direction)
    {
        this.direction = direction.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}