using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public int EnemyID { get; set; }

    public KillGoal(ChallengeController quest, int enemyID, string description, bool completed, int currentAmount, int requiredAmount, type goType)
    {
        this.Quest = quest;
        this.EnemyID = enemyID;
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
        if (id == this.EnemyID)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}
