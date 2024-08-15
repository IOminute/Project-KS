using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject behaviourContainer;
    public TMP_Text manaInfo;
    public GameObject[] behaviours;

    public TMP_Text deadBodiesCount;
    public TMP_Text kindredPointCount;

    public TMP_Text progressName;
    public GameObject progressBarContainer;
    public Image progressBar;
    public Image manaBar;

    private string[] manas;

    private int kindredPoint;
    private int deadBodies;
    private int maxBodies;
    private int currentBehaviour;

    private bool isProgressing;

    public TMP_Text gameTime;
    private float sec;
    private float min;

    private float maxMana = 1000;
    private float mana = 1000; //Temporary Value, It will be replaced with PlayerManager's Variable.

    private WaitForSeconds waitPointOne;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        manas = new string[] {"20 / Body", "300", "500"};
        maxBodies = 20;
        deadBodies = 0;
        currentBehaviour = 0;

        for (int i = 0; i < behaviours.Length; i++)
        {
            if (i == currentBehaviour) behaviours[i].SetActive(true);
            else behaviours[i].SetActive(false);
        }

        progressBar.gameObject.SetActive(false);
        progressName.gameObject.SetActive(false);
        progressBarContainer.SetActive(false);
        progressBar.fillAmount = 0f;

        waitPointOne = new WaitForSeconds(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) StartCoroutine(ChangeBehaviour());

        if (Input.GetKeyDown(KeyCode.A)) ManageBodies(1);
        if (Input.GetKeyDown(KeyCode.S)) ManageBodies(-1);
        if (Input.GetKeyDown(KeyCode.D)) ManageKindredPoint(1);
        if (Input.GetKeyDown(KeyCode.F)) ManageKindredPoint(-1);


        if (Input.GetKeyDown(KeyCode.Q)) StartCoroutine(SkillInitiated("Necromance", 0.5f, 20f));
        if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(SkillInitiated("Possesion", 2f, 100f));

        sec += Time.deltaTime;
        if (sec >= 60f)
        {
            min += 1;
            sec = 0;
        }

        gameTime.text = min.ToString("00") + " : " + sec.ToString("00");
    }

    IEnumerator ChangeBehaviour()
    {
        behaviourContainer.GetComponent<CanvasGroup>().DOFade(0f, 0.1f).SetEase(Ease.InSine);
        currentBehaviour += 1;
        if (currentBehaviour == 3) currentBehaviour = 0;
        yield return waitPointOne;
        manaInfo.text = "Mana : " + manas[currentBehaviour];
        for (int i = 0; i < behaviours.Length; i++) 
        {
            if (i  == currentBehaviour) behaviours[i].SetActive(true);
            else behaviours[i].SetActive(false);
        }
        behaviourContainer.GetComponent<CanvasGroup>().DOFade(1f, 0.1f).SetEase(Ease.OutSine);
    }

    void ManageBodies(int amount)
    {
        if (deadBodies + amount > maxBodies || deadBodies + amount < 0)
        {
            Debug.Log("Invalid Access! UIManager.cs -> ManageBodies()");
        } else 
        {
            deadBodies += amount;
        }
        deadBodiesCount.text = "Dead Bodies : " + deadBodies.ToString();
        if (deadBodies == 20) deadBodiesCount.color = Color.red;
        else deadBodiesCount.color = Color.white;
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
        kindredPointCount.text = "KindredPoint : " + kindredPoint.ToString();
    }

    IEnumerator SkillInitiated(string pgName, float duration, float requireMana)
    {
        if (isProgressing)
        {
            print("Other Progress is onloading!");
            yield break;
        }
        float currentMana = mana - requireMana;
        manaBar.DOFillAmount(currentMana / maxMana, 0.1f);
        mana = currentMana;
        isProgressing = true;
        progressBar.gameObject.SetActive(true);
        progressName.gameObject.SetActive(true);
        progressBarContainer.SetActive(true);
        progressName.text = pgName;
        progressBar.DOFillAmount(1f, duration).SetEase(Ease.Linear).OnComplete(() => print("Do Something"));
        yield return new WaitForSeconds(duration);
        isProgressing = false;
        progressBarContainer.SetActive(false);
        progressBar.gameObject.SetActive(false);
        progressName.gameObject.SetActive(false);
        progressBar.fillAmount = 0f;
        yield return null;
    }

}
