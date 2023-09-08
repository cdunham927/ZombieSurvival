using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding;

public class NPCController : MonoBehaviour
{
    public enum npcstates { idle, follow, chasePickup, getPickup, attack, die }
    [Header("Basics")]
    public npcstates curState = npcstates.follow;
    public bool inRange = false;
    Rigidbody2D bod;
    //PlayerController player;
    public float maxDisLow = 1.25f;
    public float maxDisHigh = 3f;
    float maxFollowDistance;
    float dis;

    //[Space]
    //[Header("Dialogue")]
    public string npcName;
    //public float timeBetweenTalks = 0.1f;
    //float talkCools;
    public float idleRange = 10f;
    public bool canFollow = true;

    GameObject uiParent;
    public GameObject uiPrefab;
    TMP_Text nameText;
    Image hpBar;
    GameObject h;

    GameController cont;

    //Animations
    Animator anim;

    protected float hp;
    public float maxHp;
    [HideInInspector]
    public float spd;


    //AI stuff
    public Transform target;
    public float nextWaypointDistance = 2f;
    public float slowSpd;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    float distance;
    Vector3 startPos;

    Seeker seeker;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        startPos = transform.position;
        //anim = GetComponent<Animator>();
        cont = FindObjectOfType<GameController>();

        uiParent = GameObject.FindGameObjectWithTag("NPCText");
        h = Instantiate(uiPrefab);
        nameText = h.GetComponentInChildren<TMP_Text>();
        h.transform.SetParent(uiParent.transform);
        nameText.text = npcName;
        hpBar = h.transform.GetChild(1).GetComponent<Image>();

        h.SetActive(false);

        //base.Awake();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        bod = GetComponent<Rigidbody2D>();
    }

    public void Heal(float amt)
    {
        if (hp + amt > maxHp)
        {
            hp = maxHp;
        }
        else hp += amt;
        hpBar.fillAmount = (hp / maxHp);
    }

    void Damage(float amt)
    {
        hp -= amt;
        hpBar.fillAmount = (hp / maxHp);

        if (hp < maxHp)
        {
            Die();
        }
    }

    void OnEnable()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public void Alive()
    {
        if (h != null) h.SetActive(true);
        if (hpBar == null) hpBar = h.transform.GetChild(1).GetComponent<Image>();
        hp = maxHp;
        if (hpBar != null) hpBar.fillAmount = (hp / maxHp);
        maxFollowDistance = Random.Range(maxDisLow, maxDisHigh);
    }

    private void OnDisable()
    {
        h.SetActive(false);
        ChangeState(npcstates.follow);
    }

    public void ChangeState(npcstates newState)
    {
        curState = newState;
    }

    void Die()
    {
        //Return into a zombie
        GetComponent<EnemyController>().Zombify();
        this.enabled = false;
    }

    void Idle()
    {
        //if (Input.GetKeyDown(KeyCode.Q) && inRange)
        //{
        //    //h.SetActive(true);
        //    ChangeState(npcstates.follow);
        //}
    }

    void UpdatePath()
    {
        if (target == null)
        {
            seeker.StartPath(bod.position, startPos, OnPathComplete);
            return;
        }

        if (seeker.IsDone() && target != null) seeker.StartPath(bod.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        else Debug.Log("Error making path");
    }


    void MovePath()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - bod.position).normalized;
        Vector2 force = direction * spd * Time.deltaTime;

        bod.AddForce(force);

        distance = Vector2.Distance(bod.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //if (dis >= idleRange) ChangeState(npcstates.idle);

        //if (target != null && dis > maxFollowDistance)
        //{
        //    Vector2 dir = target.position - transform.position;
        //    bod.AddForce(dir * spd * Time.deltaTime);
        //}
    }

    void Attack()
    {

    }

    //void Panic()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q) && inRange)
    //    {
    //        ChangeState(npcstates.follow);
    //    }
    //}

    void ChasePickup()
    {

    }

    void GetPickup()
    {

    }

    void Follow()
    {
        //Distance to target
        dis = Vector2.Distance(transform.position, target.position);
        if (dis > maxFollowDistance) MovePath();
    }

    private void Update()
    {
        switch (curState)
        {
            case (npcstates.idle):
                Idle();
                break;
            case (npcstates.follow):
                Follow();
                break;
            //case (npcstates.panic):
            //    Panic();
            //    break;
            case (npcstates.die):
                Die();
                break;
            case (npcstates.attack):
                Attack();
                break;
            case (npcstates.chasePickup):
                ChasePickup();
                break;
            case (npcstates.getPickup):
                GetPickup();
                break;
        }

        //if (inRange && talkCools <= 0)
        //{
        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        //if (Input.GetKeyDown(KeyCode.E) && !dCanv.isTalkingAlready)
        //        //Do Dialogue
        //        if (mainDialogue)
        //        {
        //            dCanv.EndDialogue();
        //            dCanv.StartDialogue(dialogue[0], npcName);
        //        }
        //        else
        //        {
        //            if (!speechBubbleParent.activeInHierarchy)
        //            {
        //                EndDialogue();
        //                StartDialogue(dialogue[0]);
        //            }
        //        }
        //    }
        //}
        //
        //if (talkCools > 0) talkCools -= Time.deltaTime;
    }

    //public virtual void StartDialogue(Dialogue d)
    //{
    //    sentences.Clear();
    //
    //    foreach (string s in d.sentences)
    //    {
    //        sentences.Enqueue(s);
    //    }
    //
    //    //Add continue button function to UI button
    //    //continueButton => DisplayNextSentence();
    //    speechBubbleParent.SetActive(true);
    //    Invoke("DisplayNextSentence", 0f);
    //}

    //public virtual void DisplayNextSentence()
    //{
    //    if (sentences.Count <= 0)
    //    {
    //        Invoke("EndDialogue", dialogueTime);
    //        return;
    //    }
    //
    //    string sen = sentences.Dequeue();
    //    StopAllCoroutines();
    //    StartCoroutine(TypeSentence(sen));
    //    Invoke("DisplayNextSentence", dialogueTime);
    //    //dCanv.dialogueText.text = sen;
    //}

    //IEnumerator TypeSentence(string sentence)
    //{
    //    speechBubbleText.text = "";
    //    foreach (char letter in sentence.ToCharArray())
    //    {
    //        speechBubbleText.text += letter;
    //        yield return timeBetweenChars;
    //    }
    //}

    //public virtual void EndDialogue()
    //{
    //    speechBubbleText.text = "";
    //    speechBubbleParent.SetActive(false);
    //}
}
