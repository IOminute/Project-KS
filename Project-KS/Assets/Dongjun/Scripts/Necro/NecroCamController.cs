using UnityEngine;

public class NecroCamController : MonoBehaviour
{
    float moveSpeed = 20f;
    float zoomSpeed = 8.0f;
    float minZoom = 60f;
    float maxZoom = 80f;

    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = new Vector3(0, 40f, -30f);
        }

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

    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        cam.fieldOfView -= scrollData * zoomSpeed * 100f * Time.deltaTime;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
    }
}
