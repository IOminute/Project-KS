using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour
{
    public Image targetImage; // Ray�� ����� �ϴ� UI �̹���

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, layerMask)) // Raycast�� ���� �浹�� ������Ʈ Ȯ��
            {
                // �浹�� ������Ʈ�� �±װ� �÷��� �±׿� ��ġ�ϴ� ���
                if (hit.collider)
                {
                    clickedPos = hit.point; // Ŭ���� ��ġ�� ���� ��ǥ
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, layerMask)) // Raycast�� ���� �浹�� ������Ʈ Ȯ��
            {
                // �浹�� ������Ʈ�� �±װ� �÷��� �±׿� ��ġ�ϴ� ���
                if (hit.collider)
                {
                    clickingPos = hit.point; // Ŭ���� ��ġ�� ���� ��ǥ
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, layerMask)) // Raycast�� ���� �浹�� ������Ʈ Ȯ��
            {
                // �浹�� ������Ʈ�� �±װ� �÷��� �±׿� ��ġ�ϴ� ���
                if (hit.collider)
                {
                    unclickedPos = hit.point; // Ŭ���� ��ġ�� ���� ��ǥ
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

        // OverlapBox�� ����Ͽ� ���� ���� ��� ������Ʈ ����
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
