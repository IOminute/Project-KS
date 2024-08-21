using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Necromancer : MonoBehaviour
{
    private int kindredPoint;
    static int maxBodies;

    static List<GameObject> spirits;
    static List<GameObject> allies;
    private List<GameObject> boomList;

    public float maxMana = 1000;
    public float mana = 1000; //Temporary Value, It will be replaced with PlayerManager's Variable.

    private bool isSkillDoing;
    private bool isRushing;

    public GameObject boomer;

    private WaitForSeconds waitOneSec;
    private WaitForSeconds waitHalfSec;
    private WaitForSeconds waitThreeSec;

    public List<GameObject> Units;

    private int enforceAmount = 0;
    private int enforceAmountHealth = 0;
    private int requireKindredPointToEnforce = 10;
    private int requireKindredPointToOverMaxBodies = 100;

    public Camera necroCamera;
    public Camera knightCamera;

    private GameObject playerKnight;

    void Start()
    {
        spirits = new List<GameObject>();
        allies = new List<GameObject>();
        boomList = new List<GameObject>();
        isSkillDoing = false;
        isRushing = false;
        waitOneSec = new WaitForSeconds(1f);
        waitHalfSec = new WaitForSeconds(0.5f);
        waitThreeSec = new WaitForSeconds(3f);

        playerKnight = GameObject.FindWithTag("PlayerKnight");
        playerKnight.GetComponent<KnightController>().enabled = false;

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

        if (Input.GetKeyDown(KeyCode.Z)) Enforce();
        if (Input.GetKeyDown(KeyCode.C)) OverMaxBodies();
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
        StartCoroutine(UIManager.instance.SkillInitiated("Rushing In To The Deep Death.", 30f, 0));
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
            GameObject boom = Instantiate(boomer, allies[i].transform.position, Quaternion.identity);
            boomList.Add(boom);
            allies[i].GetComponent<UnitController>().Die();
            allies.Remove(allies[i]);
        }
        isSkillDoing = false;
        for (int i = 0; i < boomList.Count; i++)
        {
            StartCoroutine(DestroyBoom(boomList[i]));
            boomList.Remove(boomList[i]);
        }
        yield return null;
    }

    IEnumerator DestroyBoom(GameObject boomToDestroy)
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(boomToDestroy);
    }

    void Revive(GameObject soul)
    {
        Vector3 spawnAllyPos = soul.transform.position;

        // 스폰 포지션에 아군 소환하기
        GameObject ally = Instantiate(Units[soul.GetComponent<Soul>().soulIndex], spawnAllyPos, Quaternion.identity);
        ally.GetComponent<UnitController>().damage += enforceAmount;
        ally.GetComponent<UnitController>().health += enforceAmountHealth;
        spirits.Remove(soul);

        // 소환된 아군 리스트에 저장하기
        allies.Add(ally);
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

    void Enforce()
    {
        if (kindredPoint < requireKindredPointToEnforce)
        {
            print("Not enough Kindred Points!");
            return;
        }
        enforceAmount += 5;
        if (enforceAmount % 10 == 0) enforceAmountHealth += 50;
    }

    void OverMaxBodies()
    {
        if (kindredPoint < requireKindredPointToOverMaxBodies)
        {
            print("Not enough Kindred Points!");
            return;
        }
        maxBodies += 10;
    }

    void Possession()
    {
        playerKnight.GetComponent<KnightController>().enabled = true;
        necroCamera.gameObject.SetActive(false);
        knightCamera.gameObject.SetActive(true);
    }
}