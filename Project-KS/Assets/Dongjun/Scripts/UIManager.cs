using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Globalization;

public class UIManager : MonoBehaviour
{
    public Image[] behaviours;
    public TMP_Text deadBodiesCount;
    public TMP_Text kindredPointCount;

    private int kindredPoint;
    private int deadBodies;
    private int maxBodies;
    private int currentBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        maxBodies = 20;
        deadBodies = 0;
        currentBehaviour = 0;  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) ChangeBehaviour(1);
        if (Input.GetMouseButtonDown(1)) ChangeBehaviour(-1);

        if (Input.GetKeyDown(KeyCode.A)) ManageBodies(1);
        if (Input.GetKeyDown(KeyCode.S)) ManageBodies(-1);
        if (Input.GetKeyDown(KeyCode.D)) ManageKindredPoint(1);
        if (Input.GetKeyDown(KeyCode.F)) ManageKindredPoint(-1);
    }

    void ChangeBehaviour(int direction)
    {
        behaviours[currentBehaviour].DOFade(0f, 0.1f).SetEase(Ease.InSine);
        currentBehaviour += direction;
        if (currentBehaviour == -1) currentBehaviour = 2;
        else if (currentBehaviour == 3) currentBehaviour = 0;
        behaviours[currentBehaviour].DOFade(1f, 0.1f).SetEase(Ease.OutSine).SetDelay(0.1f);
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
}
