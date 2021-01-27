using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public ChallengeController Quest;
    public string Description;
    public bool Completed;
    public int CurrentAmount;
    public int RequiredAmount;
    public enum type { kill, collect }
    public type goalType;

    public virtual void Init()
    {
        // default init stuff

    }

    public void Evaluate()
    {
        //Debug.Log("Evaluating goals...");
        //Debug.Log("Goals complete: " + CurrentAmount + "/" + RequiredAmount);
        if (CurrentAmount >= RequiredAmount)
        {
            //Debug.Log("Goals complete!");
            Complete();
        }
    }

    public void Complete()
    {
        Completed = true;
        Quest.CheckGoals();
    }

    public virtual void Collect(int id)
    {

    }
}
