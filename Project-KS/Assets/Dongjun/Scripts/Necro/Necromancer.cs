using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Necromancer : MonoBehaviour
{
    public LayerMask planeLayer; // Ŭ���� �÷����� ���̾�

    public GameObject cube;

    public Image temp;

    private Vector3 clickedPos;
    private Vector3 clickingPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ�� ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // Ư�� ���̾�� ����
            {
                clickedPos = hit.point; // Ŭ���� ��ġ�� ���� ��ǥ
                Debug.Log("Clicked Position on Plane: " + clickedPos);
                // ���⼭ Ŭ�� ��ġ�� ����Ͽ� �߰����� �۾��� ������ �� �ֽ��ϴ�.
            }

            
        }

        if (Input.GetMouseButton(0)) // ���콺 ���� ��ư Ŭ�� ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)) // Ư�� ���̾�� ����
            {
                clickingPos = hit.point; // Ŭ���� ��ġ�� ���� ��ǥ
                Debug.Log("UnClicked Position on Plane: " + clickingPos);
                Rect();
                // ���⼭ Ŭ�� ��ġ�� ����Ͽ� �߰����� �۾��� ������ �� �ֽ��ϴ�.
            }
        }

        if (Input.GetMouseButtonUp(0)) // ���콺 ���� ��ư Ŭ�� ��
        {
            //
        }
    }

    void Rect()
    {
        // �� �� ������ �Ÿ� ���
        float widthX = clickedPos.x - clickingPos.x;
        float widthZ = clickedPos.z - clickingPos.z;

        // ������Ʈ�� ũ�� ���� (X���� �������� �ʺ� ����)
        cube.transform.localScale = new Vector3(widthX, 1f, widthZ);

        // �� ���� �߰� ������ ����Ͽ� ������Ʈ ��ġ ����
        Vector3 midPoint = (clickedPos + clickingPos) / 2;
        cube.transform.position = midPoint;

        // ������ ���߱� ���� ������Ʈ�� ȸ�� ����
        //Vector3 direction = (pointB - pointA).normalized;
        //Quaternion rotation = Quaternion.LookRotation(direction);
        //cube.transform.rotation = rotation;
    }
}