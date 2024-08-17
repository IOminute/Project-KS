using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }

    public List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    IEnumerator GameFlow()
    {
        float time = 0f;
        bool isBossFighting = false;
        bool wave1boss = false;
        bool wave2boss = false;
        bool wave3boss = false;
        while (true)
        {
            if (!isBossFighting) time += Time.deltaTime;
            if (time > 0f && time < 30f) print("�ʷ� ��ġ�� ��");
            else if (time > 30f && time < 60f) print("�ʷ� ��ġ�� ��, �ʷ� ���");
            else if (time > 60f && time < 90f) print("�ʷ� ��ġ�� ��, �ʷ� ���, �ʷ� â����, �ʷ� �������");
            else if (time > 90f && time < 120f) print("�ʷ� ��ġ�� ��, �ʷ� ���, �ʷ� �⸶���, �ʷ� �������");
            else if (time > 120f && time < 150f) print("�ʷ� �Ѽտ� Į �Ѽտ� ������");
            else if (time > 120f && time < 150f) print("�ʷ� �Ѽտ� Į �Ѽտ� ������, �ʷ� �⸶ �Ѽտ� Į �Ѽտ� ������");
            else if (time > 180f && !wave1boss)
            {
                isBossFighting = true;
                print("���̺�1�߰�����");
            }
            else if (time > 180f && wave1boss) print("���� �Ȱ��� �帧�� && isBossFighting ���� false ó��");
        }
    }

    void SummonEnemy(GameObject enemyToSummon)
    {
        Vector3 randomPosition = GetRandomPosition();
        GameObject enemy = Instantiate(enemyToSummon, randomPosition, Quaternion.identity);
        // enemy.DoSomething
        enemies.Add(enemy);
    }

    Vector3 GetRandomPosition()
    {
        // ������ ����� �Ÿ� ����
        Vector3 randomDirection = Random.insideUnitSphere * 50;
        // ���� ��ǥ�� �߰��Ͽ� ���� ��ǥ ���
        return Vector3.zero + randomDirection;
    }


    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
