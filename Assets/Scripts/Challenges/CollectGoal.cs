using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGoal : Goal
{
    public int ItemID { get; set; }

    public CollectGoal(ChallengeController quest, int itemID, string description, bool completed, int currentAmount, int requiredAmount, type goType)
    {
        this.Quest = quest;
        this.ItemID = itemID;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
        this.goalType = goType;
    }

    public override void Init()
    {
        base.Init();
        //CombatEvents.OnEnemyDeath += EnemyDied;
        //Delegate for when the enemy dies

        //QuestEvents.KillQuest += EnemyDied;
    }

    public override void Collect(int id)
    {
        if (id == this.ItemID)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}
