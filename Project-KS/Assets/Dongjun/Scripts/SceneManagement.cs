using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }

    public GameObject castle;
    public int summonInterval;

    private List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> greenEnemies = new List<GameObject>();
    public List<GameObject> whiteEnemies = new List<GameObject>();
    public List<GameObject> blackEnemies = new List<GameObject>();

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

    void Start()
    {
        StartCoroutine(GameFlow());
    }

    IEnumerator GameFlow()
    {
        float time = 0f;
        float intervalTime = 0f;
        bool isBossFighting = false;
        bool wave1boss = false;
        bool wave2boss = false;
        bool wave3boss = false;
        intervalTime = 0f;
        List<int> tosummon = new List<int>() { 6 }; // 초록 망치든애
        IntervalSummon(tosummon, greenEnemies);
        while (true)
        {
            if (!isBossFighting)
            {
                time += Time.deltaTime;
                intervalTime += Time.deltaTime;
            }
            if (time >= 0f && time < 30f)
            {
                if (intervalTime >= summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6 }; // 초록 망치든애
                    IntervalSummon(toSummon, greenEnemies);
                }
            }
            else if (time >= 30f && time < 60f)
            {
                if (intervalTime >= summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 1 }; // 초록 망치든애, 초록 기사
                    IntervalSummon(toSummon, greenEnemies);
                }
            }
            else if (time >= 60f && time < 90f)
            {
                if (intervalTime >= summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 1, 8, 9 }; // 초록 망치든애, 초록 기사, 초록 창술사, 초록 정예기사
                    IntervalSummon(toSummon, greenEnemies);
                }
            }
            else if (time >= 90f && time < 120f)
            {
                if (intervalTime >= summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 1, 5, 0, 9 }; // 초록 망치든 애, 초록 기사, 초록 기마기사, 초록 궁수, 초록 정예기사
                    IntervalSummon(toSummon, greenEnemies);
                }
            }
            else if (time >= 120f && time < 150f)
            {
                if (intervalTime >= summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 7 }; //초록 메이지
                    IntervalSummon(toSummon, greenEnemies);
                }
            }
            else if (time >= 150f && time < 180f)
            {
                if (intervalTime >= summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 7, 3 }; //초록 메이지, 초록 기마 메이지
                    IntervalSummon(toSummon, greenEnemies);
                }
            }
            else if (time >= 180f && !wave1boss)
            {
                isBossFighting = true;
                List<int> toSummon = new List<int>() { 4, 2, 3 }; // 초록 기마 창술사, 초록 기마궁수, 초록 메이지, 석궁(없음)
                SummonBoss(toSummon, greenEnemies);
                if (enemies.Count == 0)
                {
                    StartCoroutine(UIManager.instance.waveClear("Wave 2"));
                    time = 180f;
                    intervalTime = 0f;
                    wave1boss = true;
                }
            }
            else if (time >= 180f && time < 210f && wave1boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6 }; // 흰색 망치든애
                    IntervalSummon(toSummon, whiteEnemies);
                }
            }
            else if (time >= 210f && time < 240f && wave1boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 0 }; // 흰색 망치든애, 흰색 궁수
                    IntervalSummon(toSummon, whiteEnemies);
                }
            }
            else if (time >= 240f && time < 270f && wave1boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 0, 8 }; // 흰색 망치든애, 흰색 궁수, 흰색 창술사
                    IntervalSummon(toSummon, whiteEnemies);
                }
            }
            else if (time >= 270f && time < 300f && wave1boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 0, 8, 2 }; // 흰색 망치든애, 흰색 궁수, 흰색 창술사, 흰색 기마궁수
                    IntervalSummon(toSummon, whiteEnemies);
                }
            }
            else if (time >= 300f && time < 330f && wave1boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 7, 1 }; // 흰색 메이지, 흰색 기사
                    IntervalSummon(toSummon, whiteEnemies);
                }
            }
            else if (time >= 330f && time < 360f && wave1boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 7, 1, 5 }; // 흰색 망치든애, 흰색 기사, 흰색 기마기사
                    IntervalSummon(toSummon, whiteEnemies);
                }
            }
            else if (time >= 360f && !wave2boss && wave1boss)
            {
                isBossFighting = true;
                List<int> toSummon = new List<int>() { 9, 4, 3 }; // 흰색 정예기사, 흰색 기마창술사, 흰색 메이지, 석궁(없음)
                SummonBoss(toSummon, whiteEnemies);
                if (enemies.Count == 0)
                {
                    StartCoroutine(UIManager.instance.waveClear("Wave 3"));
                    time = 360f;
                    intervalTime = 0f;
                    wave2boss = true;
                }
            }
            else if (time >= 360f && time < 390f && wave1boss && wave2boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6 }; 
                    IntervalSummon(toSummon, blackEnemies);
                }
            }
            else if (time >= 390f && time < 420f && wave1boss && wave2boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 7, 1 }; 
                    IntervalSummon(toSummon, blackEnemies);
                }
            }
            else if (time >= 420f && time < 450f && wave1boss && wave2boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 7, 1, 0 }; 
                    IntervalSummon(toSummon, blackEnemies);
                }
            }
            else if (time >= 450f && time < 480f && wave1boss && wave2boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 6, 7, 1, 0, 2 };
                    IntervalSummon(toSummon, blackEnemies);
                }
            }
            else if (time >= 480f && time < 510f && wave1boss && wave2boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 9, 1 };
                    IntervalSummon(toSummon, blackEnemies);
                }
            }
            else if (time >= 510f && time < 540f && wave1boss && wave2boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 9, 1, 0 };
                    IntervalSummon(toSummon, blackEnemies);
                }
            }
            else if (time >= 540f && wave2boss && wave1boss && !wave3boss)
            {
                isBossFighting = true;
                List<int> toSummon = new List<int>() { 4, 5, 2, 3 };
                SummonBoss(toSummon, blackEnemies);
                if (enemies.Count == 0)
                {
                    StartCoroutine(UIManager.instance.waveClear("Final Wave"));
                    time = 540f;
                    intervalTime = 0f;
                    wave3boss = true;
                    isBossFighting = false;
                }
            }
            else if (time >= 540f && time < 560f && wave1boss && wave2boss && wave3boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 4, 2, 3 };
                    SummonBoss(toSummon, greenEnemies);
                }
            }
            else if (time >= 560f && time < 580f && wave1boss && wave2boss && wave3boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 9, 4, 3 };
                    SummonBoss(toSummon, whiteEnemies);
                }
            }
            else if (time >= 580f && time < 600f && wave1boss && wave2boss && wave3boss)
            {
                if (intervalTime > summonInterval)
                {
                    intervalTime = 0f;
                    List<int> toSummon = new List<int>() { 4, 5, 2, 3 };
                    SummonBoss(toSummon, blackEnemies);
                }
            }
            else if (time >= 600f && wave1boss && wave2boss && wave3boss)
            {
                if (enemies.Count == 0)
                {
                    StartCoroutine(UIManager.instance.gameClear());
                    yield break;
                }
            }
            yield return null;
        }
    }

    void SummonEnemy(GameObject enemyToSummon)
    {
        Vector3 randomPosition = GetRandomPosition();
        GameObject enemy = Instantiate(enemyToSummon, randomPosition, Quaternion.identity);
        // enemy.DoSomething
        enemy.SetActive(true);
        enemies.Add(enemy);
    }

     void IntervalSummon(List<int> summonList, List<GameObject> toSummonColor)
    {
        for (int p = 0; p < 10; p++)
        {
            for (int i = 0; i < summonList.Count; i++)
            {
                SummonEnemy(toSummonColor[summonList[i]]);
            }
        }
    }

    void SummonBoss(List<int> summonList, List<GameObject> toSummonColor)
    {
        for (int p = 0; p < 3; p++)
        {
            for (int i = 0; i < summonList.Count; i++)
            {
                SummonEnemy(toSummonColor[summonList[i]]);
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.onUnitSphere;

        // 무작위 거리 생성 (100에서 150 사이)
        float randomDistance = Random.Range(150f, 200f);

        // 기준 좌표에 추가하여 랜덤 좌표 계산
        return Vector3.one + randomDirection * randomDistance;
    }


    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
