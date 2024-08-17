using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : MonoBehaviour
{
    private int kindredPoint;
    static int maxBodies;

    static List<GameObject> spirits;
    static List<GameObject> allies;

    public float maxMana = 1000;
    public float mana = 1000; //Temporary Value, It will be replaced with PlayerManager's Variable.

    private bool isSkillDoing;
    private bool isRushing;

    private WaitForSeconds waitOneSec;
    private WaitForSeconds waitHalfSec;
    private WaitForSeconds waitThreeSec;
    void Start()
    {
        spirits = new List<GameObject>();
        allies = new List<GameObject>();
        isSkillDoing = false;
        isRushing = false;
        waitOneSec = new WaitForSeconds(1f);
        waitHalfSec = new WaitForSeconds(0.5f);
        waitThreeSec = new WaitForSeconds(3f);
        StartCoroutine(ManaRegen());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) StartCoroutine(UIManager.instance.ChangeBehaviour());

        if (Input.GetKeyDown(KeyCode.Space) && !isSkillDoing)
        {
            if (UIManager.instance.GetCurrentBehaviourIndex() == 0) StartCoroutine(Rise());
            else if (UIManager.instance.GetCurrentBehaviourIndex() == 1) StartCoroutine(Rush());
            else if (UIManager.instance.GetCurrentBehaviourIndex() == 2) StartCoroutine(Explode());
        }
    }

    IEnumerator Rise()
    {
        if (spirits.Count == 0) 
        { 
            print("Not Enough Bodies!"); 
            yield break; 
        }
        isSkillDoing = true;
        StartCoroutine(UIManager.instance.SkillInitiated("Make Them Immortal.", 0.5f, 20 * spirits.Count));
        yield return waitHalfSec;
        ManageMana(-20f * spirits.Count);
        for (int i = 0; i < spirits.Count; i++) Revive(spirits[i]);
        isSkillDoing = false;
        yield return null;
    }

    IEnumerator Rush()
    {
        isSkillDoing = true;
        isRushing = true;
        StartCoroutine(UIManager.instance.SkillInitiated("Feeling Spirits Of Death...", 3f, 300));
        ManageMana(-300f);
        yield return waitThreeSec;
        StartCoroutine(UIManager.instance.SkillInitiated("Rushing In To The Deep Death.", 3f, 0));
        float time = 0f;
        while (time <= 30f)
        {
            yield return null;
            time += Time.deltaTime;
            if (spirits.Count > 0)
            {
                for (int i = 0; i < spirits.Count; i++)
                {
                    Revive(spirits[i]);
                }
            }
        }
        isSkillDoing = false;
        isRushing = false;
        print("RushEnd");
        yield return null;
    }

    IEnumerator Explode()
    {
        isSkillDoing = true;
        StartCoroutine(UIManager.instance.SkillInitiated("Time To Sleep Again.", 1f, 500));
        ManageMana(-500f);
        yield return waitOneSec;
        for (int i = 0; i < allies.Count; i++)
        {
            //아군 킬
            //아군 터진 자리에 데미지
            allies.Remove(allies[i]);
        }
        isSkillDoing = false;
        yield return null;
    }

    void Revive(GameObject deadEnemy)
    {
        Vector3 spawnAllyPos = deadEnemy.transform.position;
        // 적의 종류를 받아와 아군의 종류와 맞추기
        spirits.Remove(deadEnemy);
        // 스폰 포지션에 아군 소환하기
        // 소환된 아군 리스트에 저장하기
    }

    void ManageKindredPoint(int amount)
    {
        if (kindredPoint + amount < 0)
        {
            Debug.Log("Invalid Access! UIManager.cs -> ManageKindredPoint()");
        }
        else
        {
            kindredPoint += amount;
        }
        UIManager.instance.KindredPointTextChange(kindredPoint);
    }

    void ManageMana(float requireMana)
    {
        float currentMana = mana + requireMana;
        mana = currentMana;
        UIManager.instance.ManaBarAnim(mana, maxMana);
    }

    IEnumerator ManaRegen()
    {
        while (true)
        {
            int regenAmount = 10;
            if (isRushing) regenAmount = 5;
            ManageMana(regenAmount);
            yield return waitOneSec;
        }
    }

    public static void AddSpirit(GameObject spirit)
    {
        if (spirits.Count == maxBodies) return;
        spirits.Add(spirit);
    }

    public static void ClearSpirit()
    {
        spirits.Clear();
    }

    public static void AllyDies(GameObject ally)
    {
        allies.Remove(ally);
    }
}