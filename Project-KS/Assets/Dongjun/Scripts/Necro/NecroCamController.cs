using UnityEngine;

public class NecroCamController : MonoBehaviour
{
    public float moveSpeed = 5f; // 카메라 이동 속도

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // 화면의 모서리 위치를 체크
        if (mousePosition.x <= 50) // 왼쪽 모서리
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.x >= Screen.width - 50) // 오른쪽 모서리
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if (mousePosition.y <= 50) // 아래쪽 모서리
        {
            transform.position += new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.y >= Screen.height - 50) // 위쪽 모서리
        {
            transform.position += new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime;
        }
    }
}
