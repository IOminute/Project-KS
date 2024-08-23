using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class StartScene : MonoBehaviour
{
    private bool isOKToStart = false;

    public Image logo;
    public RectTransform left;
    public RectTransform right;
    public TMP_Text text;

    private bool startAnim = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!startAnim)
        //{
        //    startAnim = true;
        //    isOKToStart = false;
        //    logo.DOFade(1f, 1f).SetDelay(0.25f).SetEase(Ease.OutSine);
        //    left.DOScaleX(0f, 2f).SetDelay(1f).SetEase(Ease.OutQuart);
        //    right.DOScaleX(0f, 2f).SetDelay(1f).SetEase(Ease.OutQuart);
        //    text.DOFade(1f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetDelay(2f).SetEase(Ease.InOutSine).OnStart(() => ReadyToStart());
        //}
        if (Input.GetKeyDown(KeyCode.Return)) {
            GameStart();
        }
    }

    public void ReadyToStart()
    {
        isOKToStart=true;
    }

    void GameStart()
    {
        SceneManager.LoadScene("Main");
    }
}
