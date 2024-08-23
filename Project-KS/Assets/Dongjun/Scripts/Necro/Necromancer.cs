using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Necromancer : MonoBehaviour
{
    public static int kindredPoint;
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
    public static bool isPossessioning;
    private GameObject playerKnight;
    public GameObject explosion;

    void Start()
    {
        maxBodies = 20;

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
        if (!isPossessioning)
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
            if (Input.GetKeyDown(KeyCode.F)) Possession();
        }
    }

    IEnumerator Rise()
    {
        print(spirits.Count);
        if (spirits.Count == 0) 
        { 
            print("Not Enough Bodies!"); 
            yield break; 
        }
        //for (int i = 0; i < spirits.Count; i++)
        //{
        //    ParticleSystem.MainModule main = spirits[i].GetComponent<ParticleSystem>().main;
        //    main.startColor = new Color(72f, 61f, 139f);
        //}
        isSkillDoing = true;
        StartCoroutine(UIManager.instance.SkillInitiated("Make Them Immortal.", 0.5f, 20 * spirits.Count));
        yield return waitHalfSec;
        ManageMana(-20f * spirits.Count);
        int count = spirits.Count;
        for (int i = 0; i < count; i++) {
            Revive(spirits[0]);
            yield return null;
        }
        isSkillDoing = false;
        yield return null;
    }

    IEnumerator Rush()
    {
        if (mana < 300f)
        {
            print("Not enough Mana!");
        }
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
            int count = spirits.Count;
            for (int i = 0; i < count; i++)
            {
                Revive(spirits[0]);
                yield return null;
            }
        }
        isSkillDoing = false;
        isRushing = false;
        print("RushEnd");
        yield return null;
    }

    IEnumerator Explode()
    {
        if (allies.Count == 0)
        {
            print("Not Enough Bodies!");
            yield break;
        }
        if (mana < 500f)
        {
            print("Not Enough Mana!");
            yield break;
        }
        isSkillDoing = true;
        StartCoroutine(UIManager.instance.SkillInitiated("Time To Sleep Again.", 1f, 500));
        ManageMana(-500f);
        yield return waitOneSec;
        int count = allies.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject boom = Instantiate(boomer, allies[0].transform.position, Quaternion.identity);
            GameObject explosionvfx = Instantiate(explosion, allies[0].transform.position, Quaternion.identity);
            Destroy(explosionvfx, 3.0f);
            boomList.Add(boom);
            allies[0].GetComponent<UnitController>().Die();
            allies.Remove(allies[0]);
        }
        isSkillDoing = false;
        int boomCount = boomList.Count;
        for (int i = 0; i < boomCount; i++)
        {
            StartCoroutine(DestroyBoom(boomList[0]));
            boomList.Remove(boomList[0]);
        }
        yield return null;
    }

    IEnumerator DestroyBoom(GameObject boomToDestroy)
    {
        yield return new WaitForSeconds(0.2f);
        if(boomToDestroy != null)
        {
            Destroy(boomToDestroy);
        }
    }

    void Revive(GameObject soul)
    {
        Vector3 spawnAllyPos = soul.transform.position;
        spawnAllyPos.y = 0;

        // 스폰 포지션에 아군 소환하기
        GameObject ally = Instantiate(Units[soul.GetComponent<Soul>().soulIndex], spawnAllyPos, Quaternion.identity);
        ally.GetComponent<UnitController>().damage += enforceAmount;
        ally.GetComponent<UnitController>().health += enforceAmountHealth;
        spirits.Remove(soul);
        Destroy(soul);

        UIManager.instance.BodyTextChange(spirits.Count);
        // 소환된 아군 리스트에 저장하기
        allies.Add(ally);
    }

    public static void ManageKindredPoint(int amount)
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
        mana += requireMana;
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
        UIManager.instance.BodyTextChange(spirits.Count);
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
        ManageKindredPoint(-2 * enforceAmount);
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
        ManageKindredPoint(-100);
        maxBodies += 10;
    }

    void Possession()
    {
        if (UIManager.instance.possessionBar.fillAmount == 1f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            UIManager.instance.DoPossess();
            UIManager.instance.necroCanvas.gameObject.SetActive(false);
            UIManager.instance.knightCanvas.gameObject.SetActive(true);
            isPossessioning = true;
            playerKnight.GetComponent<KnightController>().enabled = true;
            StartCoroutine(playerKnight.GetComponent<KnightController>().ClockStart());
            necroCamera.gameObject.SetActive(false);
            knightCamera.gameObject.SetActive(true);
        }
        else
        {

        }
    }

    public static void EndPossesion()
    {
        Cursor.lockState = CursorLockMode.None;
        isPossessioning = false;
        UIManager.instance.GenPossess();
        UIManager.instance.necroCanvas.gameObject.SetActive(true);
        UIManager.instance.knightCanvas.gameObject.SetActive(false);
    }
}