using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Necromancer : MonoBehaviour
{
    private int kindredPoint;
    private int deadBodies;
    private int maxBodies;

    public float maxMana = 1000;
    public float mana = 1000; //Temporary Value, It will be replaced with PlayerManager's Variable.

    private bool isRushing;

    private WaitForSeconds waitOneSec;
    void Start()
    {
        isRushing = false;
        waitOneSec = new WaitForSeconds(1);
        StartCoroutine(ManaRegen());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) StartCoroutine(UIManager.instance.ChangeBehaviour());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (UIManager.instance.GetCurrentBehaviourIndex() == 0) Rise();
            else if (UIManager.instance.GetCurrentBehaviourIndex() == 1) Rush();
            else if (UIManager.instance.GetCurrentBehaviourIndex() == 2) Explode();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            deadBodies = 20;
        }
    }

    void Rise()
    {
        if (deadBodies== 0) { print("Not Enough Bodies!"); return; }
        StartCoroutine(UIManager.instance.SkillInitiated("Make Them Immortal.", 0.5f, 20 * deadBodies));
        ManageMana(-20f);
        ManageBodies(deadBodies);
        print("일어나라");
    }

    IEnumerator Rush()
    {
        StartCoroutine(UIManager.instance.SkillInitiated("Feeling Spirits Of Death...", 3f, 300));
        ManageMana(-300f);
        yield return new WaitForSeconds(3f);
        StartCoroutine(UIManager.instance.SkillInitiated("The Dead Bodies ", 3f, 0));
        print("카트라이더러쉬플러스");
    }

    void Explode()
    {
        StartCoroutine(UIManager.instance.SkillInitiated("Time To Sleep Again.", 1f, 500));
        ManageMana(-500f);
        print("펑- 유니티 터지는 소리");
    }

    void Revive()
    {
        print("살림");
    }

    void ManageBodies(int amount)
    {
        if (deadBodies + amount > maxBodies || deadBodies + amount < 0)
        {
            Debug.Log("Invalid Access! Necromancer.cs -> ManageBodies()");
        }
        else
        {
            deadBodies += amount;
        }
        UIManager.instance.BodyTextChange(deadBodies);
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
}