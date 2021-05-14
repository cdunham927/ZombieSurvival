using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, IDamageable<float>, IKillable
{
    //Singleton behavior for player
    public static PlayerController player;
    PlayerExtraStats extraStats;

    //Stats
    public float maxHp;
    float curHp;
    public float maxStam;
    public float curStam;
    public float money;

    //UI
    public Image hpImg;
    public Image stamImg;
    public float uiLerpSpd;
    public TextMeshProUGUI moneyText;

    //Movement
    public float spd;
    public float runSpd;
    public float aimSpd;
    float curSpd;
    Rigidbody2D bod;
    Vector2 move;

    //Weapon
    WeaponController weapon;

    //Player actions/Input
    public PlayerActions playerActions;
    float run;
    float RMB;

    //Stat modifiers
    public float speedMod;

    //Damage
    float iframes;
    float iframeTime = 0.25f;

    private void Awake()
    {
        if (player == null)
            player = this;
        else if (player != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        //playerActions = new PlayerActions();
        //playerActions.PlayerControls.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //playerActions.PlayerControls.Run.performed += ctx => run = ctx.ReadValue<float>();
        //playerActions.PlayerControls.RMB.performed += ctx => RMB = ctx.ReadValue<float>();

        extraStats = GetComponent<PlayerExtraStats>();
        weapon = GetComponentInChildren<WeaponController>();
        bod = GetComponent<Rigidbody2D>();
        curHp = maxHp;
        curStam = maxStam;

        money = extraStats.startMoney;
    }

    private void Update()
    {
        //bool myBool = Mathf.Approximately(RMB, 1);
        bool myBool = Input.GetButton("RMB");
        //bool myRunBool = Mathf.Approximately(run, 1);
        bool myRunBool = Input.GetButton("Run");
        //Movement
        //Get Inputs

        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Move if any move buttons are pressed
        if (move.x != 0) bod.AddForce(Vector2.right * curSpd * move.x * Time.deltaTime);
        if (move.y != 0) bod.AddForce(Vector2.up * curSpd * move.y * Time.deltaTime);

        //Sprinting
        if (myRunBool && curStam > 0 && (move.x != 0 || move.y != 0))
        {
            //Running
            curStam -= Time.deltaTime;
            curSpd = runSpd;
        }
        else if (myBool)
        {
            //Aiming
            curSpd = aimSpd;
        }
        else
        {
            //Normal movement
            curStam += Time.deltaTime;
            curSpd = spd;
        }
        curStam = Mathf.Clamp(curStam, 0, maxStam);

        //UI
        hpImg.fillAmount = Mathf.Lerp(hpImg.fillAmount, (curHp / maxHp), uiLerpSpd * Time.deltaTime);
        stamImg.fillAmount = Mathf.Lerp(stamImg.fillAmount, (curStam / maxStam), uiLerpSpd * Time.deltaTime);
        moneyText.text = "$" + money.ToString();

        //Debugging
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Break();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                ResetHp();
            }

            if (Input.GetKeyDown(KeyCode.Comma))
            {
                money = 0;
            }

            if (Input.GetKey(KeyCode.M))
            {
                money++;
            }
        }

        if (iframes > 0) iframes -= Time.deltaTime;
    }

    void ResetHp()
    {
        //Debug.Log("Hp = " + curHp);
        hpImg.color = Color.yellow;
        Invoke("hpToRed", 0.5f);
        curHp = maxHp;
    }

    void hpToRed()
    {
        hpImg.color = Color.red;
    }

    public void Damage(float amt)
    {
        if (iframes <= 0)
        {
            hpImg.color = Color.white;
            Invoke("hpToRed", 0.05f);
            curHp -= amt;
            iframes = iframeTime;
            //Debug.Log("Hp = " + curHp);
        }
    }

    public void Die()
    {

    }
}

public class QuestHolder : MonoBehaviour
{
    public List<ChallengeController> quests = new List<ChallengeController>();
    public List<ChallengeController> finishedQuests = new List<ChallengeController>();

    public void AddQuest(ChallengeController quest)
    {
        if (!quests.Contains(quest))
        {
            ChallengeController q = gameObject.AddComponent(System.Type.GetType(quest.name)) as ChallengeController;
            //Debug.Log("Added " + quest.QuestName + " to active quests!");
            quests.Add(q);
        }
        //else Debug.Log("Already have this quest");
    }

    public bool FindFinishedQuest(int id)
    {
        foreach (ChallengeController q in finishedQuests)
        {
            if (q.identifier == id) return true;
        }
        return false;
    }

    public bool HasFinishedQuest(ChallengeController quest)
    {
        return finishedQuests.Contains(quest);
    }

    public void Remove(ChallengeController quest)
    {
        ChallengeController q = FindQuest(quest.identifier);
        finishedQuests.Add(q);
        quests.Remove(q);
    }

    public bool HasQuest(int id)
    {
        foreach (ChallengeController q in quests)
        {
            if (q.identifier == id)
            {
                //Debug.Log(q.QuestName + ": " + q.identifier);
                return true;
            }
        }
        //Debug.Log("Quest with id " + id + " not found");
        return false;
    }

    public ChallengeController FindQuest(int identifier)
    {
        foreach (ChallengeController q in quests)
        {
            if (q.identifier == identifier) return q;
        }
        return null;
    }

    public ChallengeController FindQuest(ChallengeController quest)
    {
        foreach (ChallengeController q in quests)
        {
            if (q == quest) return q;
        }
        return null;
    }

    public ChallengeController FindQuest(string questName)
    {
        foreach (ChallengeController q in quests)
        {
            if (q.QuestName == questName) return q;
        }
        return null;
    }

    public void UpdateKillQuests(int id)
    {
        foreach (ChallengeController q in quests)
        {
            foreach (Goal g in q.Goals)
            {
                if (g.goalType == Goal.type.kill)
                {
                    g.Collect(id);
                }
            }
        }
    }

    public void UpdateCollectQuests(int id)
    {
        foreach (ChallengeController q in quests)
        {
            foreach (Goal g in q.Goals)
            {
                if (g.goalType == Goal.type.collect)
                {
                    g.Collect(id);
                }
            }
        }
    }

    //For testing
    public void TestKill()
    {
        ChallengeController q = FindQuest("Kill Bandits");
        if (q != null) q.CheckGoals();
        else Debug.Log("Don't have this quest!");
    }

    public void TestFetch()
    {
        ChallengeController q = FindQuest("Find Ring");
        if (q != null) q.CheckGoals();
        else Debug.Log("Don't have this quest!");
    }

    public void TestObjective()
    {
        ChallengeController q = FindQuest("ETC Objective");
        if (q != null) q.CheckGoals();
        else Debug.Log("Don't have this quest!");
    }
}
