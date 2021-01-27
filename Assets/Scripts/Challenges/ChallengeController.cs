using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[SerializeField]
public class ChallengeController : MonoBehaviour
{
    public List<Goal> Goals = new List<Goal>();
    public int identifier;
    public string QuestName;
    public string Description;
    public float ExperienceReward;
    public float GoldReward;
    //public Item ItemReward { get; set; }
    public bool Completed;

    public void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
        //if (Completed) Debug.Log("Completed quest! Go turn it in!");

        //if (Completed) GiveReward();
    }

    public void GiveReward()
    {
        //QuestEvents.FinishQuest += GiveReward;
        PlayerController player = FindObjectOfType<PlayerController>();
        //player.GainExp(ExperienceReward);
        FindObjectOfType<QuestHolder>().Remove(this);
        //player.AddGold(GoldReward);
    }
}
