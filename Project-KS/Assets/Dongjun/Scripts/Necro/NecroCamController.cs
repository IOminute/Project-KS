using UnityEngine;

public class NecroCamController : MonoBehaviour
{
    public float moveSpeed = 5f; // ī�޶� �̵� �ӵ�

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // ȭ���� �𼭸� ��ġ�� üũ
        if (mousePosition.x <= 0) // ���� �𼭸�
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.x >= Screen.width) // ������ �𼭸�
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if (mousePosition.y <= 0) // �Ʒ��� �𼭸�
        {
            transform.position += new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.y >= Screen.height) // ���� �𼭸�
        {
            transform.position += new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime;
        }
    }
}
