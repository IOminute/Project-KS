using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Selector : MonoBehaviour
{
    private List<GameObject> triggered;

    private MeshCollider meshCollider;
    public LayerMask planeLayer; // Ŭ���� �÷����� ���̾�

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
        if (Input.GetMouseButtonDown(0) && !isSelecting) // ���콺 ���� ��ư Ŭ�� ��
        {
            meshCollider.enabled = true;
            isSelecting = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // Ư�� ���̾�� ����
            {
                clickedPos = hit.point; // Ŭ���� ��ġ�� ���� ��ǥ
                //Debug.Log("Clicked Position on Plane: " + clickedPos);
                // ���⼭ Ŭ�� ��ġ�� ����Ͽ� �߰����� �۾��� ������ �� �ֽ��ϴ�.
            }


        }

        if (Input.GetMouseButton(0) && !isSelecting) // ���콺 ���� ��ư Ŭ�� ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // Ư�� ���̾�� ����
            {
                clickingPos = hit.point; // Ŭ���� ��ġ�� ���� ��ǥ
                //Debug.Log("UnClicked Position on Plane: " + clickingPos);
                Rect();
                // ���⼭ Ŭ�� ��ġ�� ����Ͽ� �߰����� �۾��� ������ �� �ֽ��ϴ�.
            }
        }

        if (Input.GetMouseButtonUp(0) && !isSelecting) // ���콺 ���� ��ư Ŭ�� ��
        {
            meshCollider.enabled = false;
            Select();
        }

        if (Input.GetMouseButtonUp(1) && isSelecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // Ư�� ���̾�� ����
            {
                clickingPos = hit.point; // Ŭ���� ��ġ�� ���� ��ǥ
                //Debug.Log("Move Position on Plane: " + clickingPos);
                MoveManager(clickingPos);
                // ���⼭ Ŭ�� ��ġ�� ����Ͽ� �߰����� �۾��� ������ �� �ֽ��ϴ�.
            }
        }

        // print(selected);
    }

    void Rect()
    {
        // �� �� ������ �Ÿ� ���
        float widthX = clickedPos.x - clickingPos.x;
        float widthZ = clickedPos.z - clickingPos.z;

        // ������Ʈ�� ũ�� ���� (X���� �������� �ʺ� ����)
        transform.localScale = new Vector3(widthX, 1f, widthZ);

        // �� ���� �߰� ������ ����Ͽ� ������Ʈ ��ġ ����
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
