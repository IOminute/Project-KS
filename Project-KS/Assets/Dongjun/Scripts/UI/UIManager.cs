using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject behaviourContainer;
    public TMP_Text manaInfo;
    public GameObject[] behaviours;

    public TMP_Text deadBodiesCount;
    public TMP_Text allyCount;
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

    public Canvas necroCanvas;
    public Canvas knightCanvas;

    public TMP_Text clearText;

    public Button quit;

    public TMP_Text manaText;
    public TMP_Text enforceLevel;

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

        allyCount.alpha = 0;
        quit.gameObject.SetActive(false);

        manaText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        sec = (int)SceneManagement.Instance.time % 60;
        min = (int)SceneManagement.Instance.time / 60;

        // print(min + " : " + sec);

        gameTime.text = min.ToString("00") + " : " + sec.ToString("00");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused && isSettingsOn) SettingsOff();
            else if (isPaused) PauseOff();
            else if (!isPaused) PauseOn();
        }

        if (currentBehaviour == 2)
        {
            allyCount.alpha = 1f;
            deadBodiesCount.alpha = 0f;
        }
        else
        {
            allyCount.alpha = 0f;
            deadBodiesCount.alpha = 1f;
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

    public void AllyTextChange(int amount)
    {
        allyCount.text = "Undead Allies : " + amount.ToString();
    }

    public void BodyTextChange(int amount)
    {
        deadBodiesCount.text = "Dead Bodies : " + amount.ToString();
        if (amount == Necromancer.maxBodies) deadBodiesCount.color = Color.red;
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
        manaBar.fillAmount = mana / maxMana;
    }


    public void PauseOn()
    {
        if (Necromancer.isPossessioning)
        {
            GameObject knightCamera = GameObject.Find("Knight_Camera");
            knightCamera.GetComponent<KnightCamera>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        pauseContainer.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }

    public void PauseOff()
    {
        if (Necromancer.isPossessioning)
        {
            GameObject knightCamera = GameObject.Find("Knight_Camera");
            knightCamera.GetComponent<KnightCamera>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
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

    public void BackToTitle()
    {
        SceneManagement.Instance.SceneLoad("StartScene");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void DoPossess()
    {
        possessionBar.fillAmount = 0f;
    }

    public void GenPossess()
    {
        possessionBar.DOFillAmount(1f, 20f).SetEase(Ease.Linear);
    }

    public IEnumerator waveClear(string text)
    {
        clearText.text = "Stage Clear!";
        clearText.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.5f);
        clearText.text = text;
        yield return new WaitForSeconds(0.5f);
        clearText.DOFade(0f, 0.5f).SetEase(Ease.OutSine);
        yield return null;
    }

    public IEnumerator gameClear()
    {
        clearText.fontSize = 100f;
        clearText.text = "Clear!";
        clearText.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
        quit.gameObject.SetActive(true);
        yield return null;
    }

    public IEnumerator manaIndian()
    {
        manaText.enabled = true;
        yield return new WaitForSeconds(0.5f);
        manaText.enabled = false;
    }

    public void EnforceText(int level)
    {
        enforceLevel.text = level.ToString();
    }
}
