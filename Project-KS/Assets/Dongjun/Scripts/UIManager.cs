using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Image[] behaviours;

    private int currentBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        currentBehaviour = 0;  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) ChangeBehaviour(1);
        if (Input.GetMouseButtonDown(1)) ChangeBehaviour(-1);
    }

    void ChangeBehaviour(int direction)
    {
        behaviours[currentBehaviour].DOFade(0f, 0.1f).SetEase(Ease.InSine);
        currentBehaviour += direction;
        if (currentBehaviour == -1) currentBehaviour = 2;
        else if (currentBehaviour == 3) currentBehaviour = 0;
        behaviours[currentBehaviour].DOFade(1f, 0.1f).SetEase(Ease.OutSine).SetDelay(0.1f);
    }
}
