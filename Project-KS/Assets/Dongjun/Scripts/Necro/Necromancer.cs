using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Necromancer : MonoBehaviour
{
    public LayerMask planeLayer; // 클릭할 플레인의 레이어

    public GameObject cube;

    public Image temp;

    private Vector3 clickedPos;
    private Vector3 clickingPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // 특정 레이어에만 반응
            {
                clickedPos = hit.point; // 클릭한 위치의 월드 좌표
                Debug.Log("Clicked Position on Plane: " + clickedPos);
                // 여기서 클릭 위치를 사용하여 추가적인 작업을 수행할 수 있습니다.
            }

            
        }

        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // 특정 레이어에만 반응
            {
                clickingPos = hit.point; // 클릭한 위치의 월드 좌표
                Debug.Log("UnClicked Position on Plane: " + clickingPos);
                Rect();
                // 여기서 클릭 위치를 사용하여 추가적인 작업을 수행할 수 있습니다.
            }
        }

        if (Input.GetMouseButtonUp(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            //
        }
    }

    void Rect()
    {
        // 두 점 사이의 거리 계산
        float widthX = clickedPos.x - clickingPos.x;
        float widthZ = clickedPos.z - clickingPos.z;

        // 오브젝트의 크기 조정 (X축을 기준으로 너비를 설정)
        cube.transform.localScale = new Vector3(widthX, 1f, widthZ);

        // 두 점의 중간 지점을 계산하여 오브젝트 위치 조정
        Vector3 midPoint = (clickedPos + clickingPos) / 2;
        cube.transform.position = midPoint;

        // 방향을 맞추기 위해 오브젝트의 회전 조정
        //Vector3 direction = (pointB - pointA).normalized;
        //Quaternion rotation = Quaternion.LookRotation(direction);
        //cube.transform.rotation = rotation;
    }
}