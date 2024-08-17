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
            if (time > 0f && time < 30f) print("초록 망치든 애");
            else if (time > 30f && time < 60f) print("초록 망치든 애, 초록 기사");
            else if (time > 60f && time < 90f) print("초록 망치든 애, 초록 기사, 초록 창술사, 초록 정예기사");
            else if (time > 90f && time < 120f) print("초록 망치든 애, 초록 기사, 초록 기마기사, 초록 정예기사");
            else if (time > 120f && time < 150f) print("초록 한손에 칼 한손에 스태프");
            else if (time > 120f && time < 150f) print("초록 한손에 칼 한손에 스태프, 초록 기마 한손에 칼 한손에 스태프");
            else if (time > 180f && !wave1boss)
            {
                isBossFighting = true;
                print("웨이브1중간보스");
            }
            else if (time > 180f && wave1boss) print("이제 똑같음 흐름은 && isBossFighting 변수 false 처리");
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
        // 무작위 방향과 거리 생성
        Vector3 randomDirection = Random.insideUnitSphere * 50;
        // 기준 좌표에 추가하여 랜덤 좌표 계산
        return Vector3.zero + randomDirection;
    }


    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
