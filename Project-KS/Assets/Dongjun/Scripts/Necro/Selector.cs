using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Selector : MonoBehaviour
{
    private List<GameObject> triggered;

    private MeshCollider meshCollider;
    public LayerMask planeLayer; // 클릭할 플레인의 레이어

    private Vector3 clickedPos;
    private Vector3 clickingPos;

    private List<GameObject> selected;
    private Vector3 currentPos;
    private bool isSelecting;


    void Start()
    {
        triggered = new List<GameObject>();
        selected = new List<GameObject>();
        isSelecting = false;
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSelecting) // 마우스 왼쪽 버튼 클릭 시
        {
            meshCollider.enabled = true;
            isSelecting = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // 특정 레이어에만 반응
            {
                clickedPos = hit.point; // 클릭한 위치의 월드 좌표
                //Debug.Log("Clicked Position on Plane: " + clickedPos);
                // 여기서 클릭 위치를 사용하여 추가적인 작업을 수행할 수 있습니다.
            }


        }

        if (Input.GetMouseButton(0) && !isSelecting) // 마우스 왼쪽 버튼 클릭 시
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // 특정 레이어에만 반응
            {
                clickingPos = hit.point; // 클릭한 위치의 월드 좌표
                //Debug.Log("UnClicked Position on Plane: " + clickingPos);
                Rect();
                // 여기서 클릭 위치를 사용하여 추가적인 작업을 수행할 수 있습니다.
            }
        }

        if (Input.GetMouseButtonUp(0) && !isSelecting) // 마우스 왼쪽 버튼 클릭 시
        {
            meshCollider.enabled = false;
            Select();
        }

        if (Input.GetMouseButtonUp(1) && isSelecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // 특정 레이어에만 반응
            {
                clickingPos = hit.point; // 클릭한 위치의 월드 좌표
                //Debug.Log("Move Position on Plane: " + clickingPos);
                MoveManager(clickingPos);
                // 여기서 클릭 위치를 사용하여 추가적인 작업을 수행할 수 있습니다.
            }
        }

        // print(selected);
    }

    void Rect()
    {
        // 두 점 사이의 거리 계산
        float widthX = clickedPos.x - clickingPos.x;
        float widthZ = clickedPos.z - clickingPos.z;

        // 오브젝트의 크기 조정 (X축을 기준으로 너비를 설정)
        transform.localScale = new Vector3(widthX, 1f, widthZ);

        // 두 점의 중간 지점을 계산하여 오브젝트 위치 조정
        Vector3 midPoint = (clickedPos + clickingPos) / 2;
        transform.position = midPoint;
    }


    void Select()
    {
        if (triggered.Count == 0) { transform.localScale = Vector3.zero; return; }
        currentPos = transform.position;
        selected = triggered;
        transform.localScale = Vector3.zero;
        isSelecting = true;
    }

    void MoveManager(Vector3 mousePos)
    {
        // print("currentPos : " + currentPos + " targetPos : " + mousePos);
        if (!isSelecting) { print("How did you enter this? fuck..."); return; }
        foreach (GameObject ally in selected)
        {
            Vector3 movePos = mousePos - currentPos;
            StartCoroutine(Move(ally, ally.transform.position + movePos));
        }
        isSelecting = false;
        selected.Clear();
    }

    IEnumerator Move(GameObject obj, Vector3 targetPos)
    {
        obj.transform.DOKill();
        obj.GetComponent<UnitController>().IsSelected = true;
        obj.transform.DOMove(targetPos, 20f).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => obj.GetComponent<UnitController>().IsSelected = false) ;
        //while (true)
        //{
        //    obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPos, 20 * Time.deltaTime);
        //    if (obj.transform.position == targetPos) break;
        //    yield return null;
        //}
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerUnit")
        {
            triggered.Add(other.gameObject);
        }
        print(other.gameObject);
    }
}
