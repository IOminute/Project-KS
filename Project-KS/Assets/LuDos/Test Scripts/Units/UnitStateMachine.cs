using UnityEngine;

public class UnitStateMachine : MonoBehaviour
{
    private UnitBaseState currentState;

    public void Initialize(UnitBaseState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(UnitBaseState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
}