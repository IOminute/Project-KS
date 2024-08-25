using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Necromancer : MonoBehaviour
{
    public static int kindredPoint;
    public static int maxBodies;

    public static List<GameObject> spirits;
    public static List<GameObject> allies;
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
    public GameObject ReviveEffect;

    public AudioClip explosionSound;
    public AudioClip RushSound;
    public AudioClip RiseSound;

    public AudioSource bgm;
    public AudioSource rushBGM;
    private AudioSource audioSource;

    private bool isOverMaxBodies;

    void Start()
    {
        maxBodies = 20;

        isOverMaxBodies = false;

        audioSource = GetComponent<AudioSource>();
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
        if (spirits.Count == 0) 
        {
            StopCoroutine(UIManager.instance.manaIndian());
            StartCoroutine(UIManager.instance.manaIndian());
            yield break; 
        }
        if (mana < 20 * spirits.Count)
        //for (int i = 0; i < spirits.Count; i++)
        //{
        //    ParticleSystem.MainModule main = spirits[i].GetComponent<ParticleSystem>().main;
        //    main.startColor = new Color(72f, 61f, 139f);
        //}
        isSkillDoing = true;
        ManageMana(-20f * spirits.Count);
        StartCoroutine(UIManager.instance.SkillInitiated("Make Them Immortal.", 1.0f, 20 * spirits.Count));
        // yield return waitHalfSec;
        audioSource.PlayOneShot(RiseSound);
        int count = spirits.Count;
        for (int i = 0; i < count; i++) {
            StartCoroutine(Revive(spirits[0], 0));
            yield return null;
        }
        isSkillDoing = false;
        yield return null;
    }

    IEnumerator Rush()
    {
        if (mana < 300f)
        {
            StopCoroutine(UIManager.instance.manaIndian());
            StartCoroutine(UIManager.instance.manaIndian());
            yield break;
        }
        isSkillDoing = true;
        isRushing = true;
        rushBGM.volume = 1f;
        bgm.DOFade(0f, 2f).SetEase(Ease.Linear).OnComplete(() => rushBGM.Play());
        StartCoroutine(UIManager.instance.SkillInitiated("Feeling Spirits Of Death...", 3f, 300));
        ManageMana(-300f);
        yield return waitThreeSec;
        StartCoroutine(UIManager.instance.SkillInitiated("Rushing In To The Deep Death.", 30f, 0));
        float time = 0f;
        while (time <= 30f)
        {
            yield return null;
            time += Time.deltaTime;
            while (spirits.Count != 0) 
            {
                StartCoroutine(Revive(spirits[0], 1f));
                yield return null;
            }
        }
        isSkillDoing = false;
        isRushing = false;
        rushBGM.DOFade(0f, 1.5f).SetEase(Ease.Linear).OnComplete(() => rushBGM.Stop());
        bgm.DOFade(1f, 2f).SetEase(Ease.Linear);
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
            StopCoroutine(UIManager.instance.manaIndian());
            StartCoroutine(UIManager.instance.manaIndian());
            yield break;
        }
        isSkillDoing = true;
        StartCoroutine(UIManager.instance.SkillInitiated("Time To Sleep Again.", 1.5f, 500));
        ManageMana(-500f);
        audioSource.PlayOneShot(explosionSound);
        int count = allies.Count;
        for (int i = 0; i < count; i++)
        {
            if (allies[i] != null)
            {
                GameObject explosionvfx = Instantiate(explosion, allies[i].transform.position, Quaternion.identity);
                Destroy(explosionvfx, 3.0f);
                allies[i].GetComponent<UnitController>().Stop();
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.6f);
        count = allies.Count;
       for (int i =0; i<count; i++)
       {
            if (allies[i] != null)
            {
                GameObject boom = Instantiate(boomer, allies[i].transform.position, Quaternion.identity);
                boomList.Add(boom);
                allies[i].GetComponent<UnitController>().Die();
            }
            yield return null;
       }
        allies.Clear();
        isSkillDoing = false;
        while (boomList.Count != 0)
        {
            StartCoroutine(DestroyBoom(boomList[0]));
            yield return null;
        }
        boomList.Clear();
        UIManager.instance.AllyTextChange(allies.Count);
        yield return null;
    }

    IEnumerator DestroyBoom(GameObject boomToDestroy)
    {
        if(boomToDestroy != null)
        {
            boomList.Remove(boomToDestroy);
            yield return new WaitForSeconds(0.2f);
            Destroy(boomToDestroy);
        }
    }

    IEnumerator Revive(GameObject soul, float delay)
    {
        spirits.Remove(soul);
        if (soul == null) yield break;
        if (delay > 0) yield return new WaitForSeconds(delay);
        if (soul == null) yield break;

        // 스폰 포지션에 아군 소환하기
        Vector3 spawnAllyPos = soul.transform.position;
        spawnAllyPos.y = 0;
        GameObject reviveEffect = Instantiate(ReviveEffect, spawnAllyPos, Quaternion.identity);
        Destroy(reviveEffect.gameObject, 3.0f);
        yield return new WaitForSeconds(1.0f);
        GameObject ally = Instantiate(Units[soul.GetComponent<Soul>().soulIndex], spawnAllyPos, Quaternion.identity);
        ally.GetComponent<UnitController>().damage += enforceAmount;
        ally.GetComponent<UnitController>().health += enforceAmountHealth;
        Destroy(soul);

        UIManager.instance.BodyTextChange(spirits.Count);
        // 소환된 아군 리스트에 저장하기
        allies.Add(ally);
        UIManager.instance.AllyTextChange(allies.Count);
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
        if (mana < 0) mana = 0;
        UIManager.instance.ManaBarAnim(mana, maxMana);
    }

    IEnumerator ManaRegen()
    {
        while (true)
        {
            if (mana < maxMana)
            {
                int regenAmount = 10;
                if (isRushing) regenAmount = 5;
                ManageMana(regenAmount);
                yield return waitOneSec;
            }else
            {
                yield return null;
            }
        }
    }

    public static void AddSpirit(GameObject spirit)
    {
        if (spirits.Count == maxBodies) return;
        spirits.Add(spirit);
        UIManager.instance.BodyTextChange(spirits.Count);
    }

    void Enforce()
    {
        if (enforceAmount == 50)
        {
            return;
        }
        if (kindredPoint < requireKindredPointToEnforce)
        {
            print("Not enough Kindred Points!");
            return;
        }
        ManageKindredPoint(-1 * requireKindredPointToEnforce);
        requireKindredPointToEnforce += 10;
        enforceAmount += 5;
        UIManager.instance.EnforceText(enforceAmount / 5);
        if (enforceAmount % 10 == 0) enforceAmountHealth += 50;
        if (enforceAmount == 50)
        {
            UIManager.instance.EndEnforce();
        }
    }

    void OverMaxBodies()
    {
        if (isOverMaxBodies) return;
        if (kindredPoint < requireKindredPointToOverMaxBodies)
        {
            print("Not enough Kindred Points!");
            return;
        }
        ManageKindredPoint(-100);
        maxBodies += 10;
        isOverMaxBodies = true;
        UIManager.instance.EndMaxBodies();
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