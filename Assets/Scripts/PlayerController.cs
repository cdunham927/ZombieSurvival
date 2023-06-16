using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerController : MonoBehaviour, IDamageable<float>, IKillable
{
    //Singleton behavior for player
    GameController cont;
    //public static PlayerController player;
    PlayerExtraStats extraStats;

    //Stats
    public float maxHp;
    public float overhealMaxHp;
    float curHp;
    public float maxStam;
    public float curStam;
    public float money;

    //UI
    public Image hpImg;
    public Image extraHpImg;
    public GameObject extraHpParent;
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
    //public PlayerActions playerActions;
    //float run;
    //float RMB;

    //Stat modifiers
    float speedMod;

    //Damage
    float iframes;
    [Range(0, 1f)]
    public float iframeTime = 0.25f;

    Animator anim;

    public ScriptableFloat curMagnet;
    public ScriptableFloat badMagnet;
    public ScriptableFloat goodMagnet;

    public ScriptableFloat scriptSpd;
    float spdMod;
    public float magnetCooldownTime;
    float magnetCooldown;
    public float coffeeCooldownTime;
    float coffeeCooldown;

    //Player can't move or shoot whilst in menus
    public bool canMove = true;

    public SpriteRenderer bloodRend;
    public Sprite[] bloodSprites;

    public ParticleSystem dmgParts;
    public int dmgPartsAmt = 5;

    public ParticleSystem healParts;
    public int healNums = 5;

    public Sprite deadSprite;
    public GameObject deadPlayer;

    AudioSource src;
    public AudioClip hitClip;
    public float lowPitch;
    public float highPitch;

    //Different characters
    public enum characters { doctor }
    [Space]
    [Header("Different characters and their variables")]
    public characters thisCharacter;
    float specialCooldown;
    [Header("Doctor special stats")]
    public float doctorSpecialCooldown;
    public float hpRevertLerp;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        cont = FindObjectOfType<GameController>();
        anim = GetComponent<Animator>();

        //if (player == null)
        //    player = this;
        //else if (player != this)
        //{
        //    Destroy(gameObject);
        //}

        DontDestroyOnLoad(gameObject);

        //playerActions = new PlayerActions();
        //playerActions.PlayerControls.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //playerActions.PlayerControls.Run.performed += ctx => run = ctx.ReadValue<float>();
        //playerActions.PlayerControls.RMB.performed += ctx => RMB = ctx.ReadValue<float>();

        extraStats = GetComponent<PlayerExtraStats>();
        weapon = GetComponentInChildren<WeaponController>();
        bod = GetComponent<Rigidbody2D>();
        curHp = maxHp;
        overhealMaxHp = maxHp * 2;
        curStam = maxStam;

        money = extraStats.startMoney;
        specialCooldown = 0;

        if (Application.isEditor)
        {
            money = 999;
        }
    }

    void OnEnable()
    {
        //Enable player collider
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
    }

    public void SetCoffee()
    {
        coffeeCooldown = coffeeCooldownTime;
    }

    public void SetMagnet()
    {
        magnetCooldown = magnetCooldownTime;
    }

    public void AddMoney(float amt)
    {
        money += amt;
    }

    public float GetHealth()
    {
        return curHp;
    }

    public void InMenu()
    {
        canMove = false;
    }

    public void ExitMenu()
    {
        canMove = true;
    }

    private void Update()
    {
        if (curHp > 0)
        {
            if (coffeeCooldown > 0)
            {
                spdMod = scriptSpd.val;
                coffeeCooldown -= Time.deltaTime;
            }
            else
            {
                spdMod = 0;
            }

            if (magnetCooldown > 0)
            {
                curMagnet.val = goodMagnet.val;
                magnetCooldown -= Time.deltaTime;
            }
            else
            {
                curMagnet.val = badMagnet.val;
            }

            //bool myBool = Mathf.Approximately(RMB, 1);
            bool myBool = Input.GetButton("Aim");
            //bool myRunBool = Mathf.Approximately(run, 1);
            bool myRunBool = Input.GetButton("Run");
            //Movement
            //Get Inputs

            move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //Move if any move buttons are pressed
            if (move.x != 0 && canMove) bod.AddForce(Vector2.right * (curSpd + (curSpd * spdMod)) * move.x * Time.deltaTime);
            if (move.y != 0 && canMove) bod.AddForce(Vector2.up * (curSpd + (curSpd * spdMod)) * move.y * Time.deltaTime);

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

            if (Input.GetButton("Fire2") && specialCooldown <= 0)
            {
                UseSpecial();
            }

            if (specialCooldown > 0) specialCooldown -= Time.deltaTime;

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

            //anim.SetFloat("moveX", bod.velocity.x);
            //anim.SetFloat("moveY", bod.velocity.y);

            if (iframes > 0) iframes -= Time.deltaTime;
        }

        if (curHp > maxHp)
        {
            curHp -= Time.deltaTime * hpRevertLerp;
        }

        //UI
        hpImg.fillAmount = Mathf.Lerp(hpImg.fillAmount, (curHp / maxHp), uiLerpSpd * Time.deltaTime);
        //if (curHp > maxHp)
        //{
        //    extraHpParent.gameObject.SetActive(true);
        //    extraHpImg.fillAmount = Mathf.Lerp(extraHpImg.fillAmount, ((curHp) / (maxHp * 2)), uiLerpSpd * Time.deltaTime);
        //}
        //else extraHpParent.SetActive(false);
        stamImg.fillAmount = Mathf.Lerp(stamImg.fillAmount, (curStam / maxStam), uiLerpSpd * Time.deltaTime);
        moneyText.text = "$" + money.ToString();
    }

    void ResetHp()
    {
        //Debug.Log("Hp = " + curHp);
        hpImg.color = Color.yellow;
        //if (extraHpParent.activeInHierarchy) extraHpImg.color = Color.yellow;
        Invoke("hpToRed", 0.5f);
        curHp = maxHp;
    }

    public void Heal(float amt)
    {
        if (curHp + amt > maxHp) curHp = maxHp;
        else curHp += amt;

        healParts.Emit(healNums);

        CheckHpAnim();
    }

    void hpToRed()
    {
        if (curHp > maxHp) hpImg.color = Color.yellow;
        else hpImg.color = Color.red;
        //if (extraHpParent.activeInHierarchy) extraHpImg.color = Color.red;
    }

    void CheckHpAnim()
    {
        if (curHp < (maxHp / 3))
        {
            bloodRend.sprite = bloodSprites[2];
        }
        else if (curHp < (2 * maxHp / 3))
        {
            bloodRend.sprite = bloodSprites[1];
        }
        else
        {
            bloodRend.sprite = bloodSprites[0];
        }
    }

    public void Damage(float amt)
    {
        //Put iframes in enemy instead
        if (curHp > 0)
        {
            src.pitch = Random.Range(lowPitch, highPitch);
            src.PlayOneShot(hitClip);
            src.pitch = 1f;
            anim.SetTrigger("Hurt");
            hpImg.color = Color.white;
            //if (extraHpParent.activeInHierarchy) extraHpImg.color = Color.white;
            //if (extraHpParent.activeInHierarchy && curHp <= maxHp)
            //{
            //    Invoke("DeactivateExtraHpUI", 2f);
            //}
            Invoke("hpToRed", 0.05f);
            curHp -= amt;
            iframes = iframeTime;
            //Debug.Log("Hp = " + curHp);

            dmgParts.Emit(dmgPartsAmt);
            CheckHpAnim();
        }

        if (curHp <= 0) Die();
    }

    void DeactivateExtraHpUI()
    {
        //extraHpParent.SetActive(false);
    }

    public void Die()
    {
        //Disable... collider for player?
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;

        BodyController body = Instantiate(deadPlayer, transform.position, Quaternion.identity).GetComponent<BodyController>();
        body.ChangeSprite(deadSprite);

        gameObject.SetActive(false);
        cont.GameOver();
    }

    public void UseSpecial()
    {
        switch(thisCharacter)
        {
            case characters.doctor:
                Overheal();
                break;
        }
    }

    //Overheals the player to double their max hp
    //Buffs the player for a short time
    //Hp reverts to normal over time
    public void Overheal()
    {
        hpImg.color = Color.yellow;
        curHp = overhealMaxHp;
        specialCooldown = doctorSpecialCooldown;
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
