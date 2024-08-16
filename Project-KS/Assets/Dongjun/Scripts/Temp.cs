using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour
{
    public Image targetImage; // Ray를 쏘고자 하는 UI 이미지

    public LayerMask layerMask;

    private Vector3 clickedPos;
    private Vector3 clickingPos;
    private Vector3 unclickedPos;
    private Vector3 center;
    private Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E)) ShootRaysFromCorners();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, layerMask)) // Raycast를 통해 충돌된 오브젝트 확인
            {
                // 충돌한 오브젝트의 태그가 플레인 태그와 일치하는 경우
                if (hit.collider)
                {
                    clickedPos = hit.point; // 클릭한 위치의 월드 좌표
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, layerMask)) // Raycast를 통해 충돌된 오브젝트 확인
            {
                // 충돌한 오브젝트의 태그가 플레인 태그와 일치하는 경우
                if (hit.collider)
                {
                    clickingPos = hit.point; // 클릭한 위치의 월드 좌표
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, layerMask)) // Raycast를 통해 충돌된 오브젝트 확인
            {
                // 충돌한 오브젝트의 태그가 플레인 태그와 일치하는 경우
                if (hit.collider)
                {
                    unclickedPos = hit.point; // 클릭한 위치의 월드 좌표
                    ShootRaysFromCorners();
                    print(clickedPos + "and" + unclickedPos);
                }
            }
        }
    }

    void ShootRaysFromCorners()
    {


        center = (clickedPos + unclickedPos) / 2;
        size = new Vector3(unclickedPos.x - clickedPos.x, 1f, unclickedPos.z - clickedPos.z);

        print(center + "and the size is" + size);

        // OverlapBox를 사용하여 영역 내의 모든 오브젝트 감지
        Collider[] hitColliders = Physics.OverlapBox(center, size);

        print(hitColliders);

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("Hit Object: " + hitCollider.name + " with Tag: " + hitCollider.tag);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
