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

    public Image possessionBar;

    private string[] manas;

    
    private int currentBehaviour;

    private bool isProgressing;

    public TMP_Text gameTime;
    private float sec;
    private float min;

    public GameObject pauseContainer;
    public GameObject settingsContainer;
    private bool isPaused;
    private bool isSettingsOn;

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

        pauseContainer.SetActive(false);
        settingsContainer.SetActive(false);
        isPaused = false;
        isSettingsOn = false;
    }

    // Update is called once per frame
    void Update()
    {

        sec += Time.deltaTime;
        if (sec >= 60f)
        {
            min += 1;
            sec = 0;
        }

        gameTime.text = min.ToString("00") + " : " + sec.ToString("00");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused && isSettingsOn) SettingsOff();
            else if (isPaused) PauseOff();
            else if (!isPaused) PauseOn();
        }
    }
    public IEnumerator ChangeBehaviour()
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

    public int GetCurrentBehaviourIndex() { return currentBehaviour; }

    public void BodyTextChange(int amount)
    {
        deadBodiesCount.text = "Dead Bodies : " + amount.ToString();
        if (amount == 20) deadBodiesCount.color = Color.red;
        else deadBodiesCount.color = Color.white;
    }

    public void KindredPointTextChange(int amount)
    {
        kindredPointCount.text = "KindredPoint : " + amount.ToString();
    }

    public IEnumerator SkillInitiated(string pgName, float duration, float requireMana)
    {
        if (isProgressing)
        {
            print("Other Progress is onloading!");
            yield break;
        }
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

    public void ManaBarAnim(float mana, float maxMana)
    {
        manaBar.DOFillAmount(mana/maxMana, 0.1f);
    }


    public void PauseOn()
    {
        pauseContainer.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }

    public void PauseOff()
    {
        pauseContainer.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    public void SettingsOn()
    {
        settingsContainer.SetActive(true);
        isSettingsOn = true;
    }
    public void SettingsOff()
    {
        settingsContainer.SetActive(false);
        isSettingsOn = false;
    }

    public void DoPossess()
    {
        possessionBar.fillAmount = 0f;
    }

    public void GenPossess()
    {
        possessionBar.DOFillAmount(1f, 20f).SetEase(Ease.Linear);
    }
}
