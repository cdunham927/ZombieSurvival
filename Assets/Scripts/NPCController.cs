using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCController : MonoBehaviour
{
    public enum npcstates { idle, follow, chasePickup, getPickup, attack, die }
    [Header("Basics")]
    public npcstates curState = npcstates.follow;
    public bool inRange = false;
    Rigidbody2D bod;
    //PlayerController player;
    Transform target;
    public float maxDisLow = 1.25f;
    public float maxDisHigh = 3f;
    public float maxFollowDistance;
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
    public float spd;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        cont = FindObjectOfType<GameController>();

        uiParent = GameObject.FindGameObjectWithTag("NPCText");
        h = Instantiate(uiPrefab);
        nameText = h.GetComponentInChildren<TMP_Text>();
        h.transform.SetParent(uiParent.transform);
        nameText.text = npcName;
        hpBar = h.transform.GetChild(1).GetComponent<Image>();

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
        h.SetActive(true);
        maxFollowDistance = Random.Range(maxDisLow, maxDisHigh);
        hpBar.fillAmount = 1;
    }

    public void ChangeState(npcstates newState)
    {
        curState = newState;
    }

    void Die()
    {
        h.SetActive(false);
        ChangeState(npcstates.idle);
    }

    void Idle()
    {
        if (Input.GetKeyDown(KeyCode.Q) && inRange)
        {
            h.SetActive(true);
            ChangeState(npcstates.follow);
        }
    }

    void Follow()
    {
        dis = Vector2.Distance(transform.position, target.position);

        //if (dis >= idleRange) ChangeState(npcstates.idle);

        if (target != null)
        {
            Vector2 dir = target.position - transform.position;
            bod.AddForce(dir * spd * Time.deltaTime);
        }

        if (target != null)
        {
            //Animator
            //anim.SetFloat("moveX", bod.velocity.x);
            //anim.SetFloat("moveY", bod.velocity.y);




            //if (target.position.y > transform.position.y) rend.sprite = sprites[0];
            //if (target.position.x < transform.position.x) rend.sprite = sprites[3];
            //if (target.position.x > transform.position.x) rend.sprite = sprites[2];
            //if (target.position.y < transform.position.y) rend.sprite = sprites[1];

            //rend.flipX = (target.position.x > transform.position.x);
        }

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    n.SetActive(false);
        //    h.SetActive(false);
        //    ChangeState(npcstates.follow);
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
