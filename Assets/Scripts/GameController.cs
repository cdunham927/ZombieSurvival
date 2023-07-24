using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //public static GameController cont;
    public GameObject shopMenu;
    public bool inMenu;
    //Waves
    public int wave = 1;
    public GameObject[] enemy;
    public List<GameObject> zombies;
    public List<GameObject> zombieSquids;
    public List<GameObject> zombieBehemoths;
    public float enemiesToSpawn;
    //If player kills all enemies, the countdown to the next wave starts
    [SerializeField]
    float enemiesSpawned;
    float totalEnemies;
    //Keeps track of enemies spawned during this wave
    float curEnemies;
    public float timeBetweenWaves = 10f;
    float nextWaveCools;
    public float firstWaveCountdown = 3f;
    float cools;
    public float timeBetweenSpawnsLow;
    public float timeBetweenSpawnsHigh;
    Vector2 bounds;

    PlayerController player;

    //Test IDamageable interface
    public float dmg = 5;

    //Testing money calculations per wave
    public AnimationCurve moneyGraph;
    public AnimationCurve zombieGraph;

    //Testing zombies per wave
    public float zombIncrease = 25;
    public float initialZombieScore = 75;

    public GameObject waveTimerParent;
    public TMP_Text waveTimerText;
    public TMP_Text zombiesLeftText;

    public float holdForWaveTime = 1.5f;
    float curHoldTime;
    public Image holdParent;
    public Image holdImage;
    public Image killImage;

    //Probably need this for the fire stuff too
    //
    //
    [HideInInspector]
    public List<GameObject> explosionList;
    public GameObject explosionPrefab;

    public AudioSource soundSrc;

    public GameObject gameOverMenu;
    public GameObject gcUI;

    //Spawn within a box;
    public Vector2 boxSpawns;

    [Space]
    [Header("Pause game for JUICE")]
    //Our slow timescale, 0.1 or something probably
    public float freezeTime;
    //The speed at which we start going back to normal, probably 0.9
    public float freezeFPS;
    //How long we wait until we start increasing time
    public float freezeResumeTime;
    float freezeCurTime;
    //How fast we go from 0.9 to 1.0 timescale
    public float freezeLerpSpd;

    //Object pool


    private void Awake()
    {
        nextWaveCools = firstWaveCountdown;

        //if (cont == null)
        //{
        //    cont = this;
        //    gameOverMenu = GameObject.FindGameObjectWithTag("GameOver");
        //    gameOverMenu.SetActive(false);
        //    DontDestroyOnLoad(gameObject);
        //}
        //else Destroy(gameObject);

        //gameOverMenu = GameObject.FindGameObjectWithTag("GameOver");
        //gameOverMenu.SetActive(false);

        //Start wave
        enemiesSpawned = 0;
        totalEnemies = 0;
        enemiesToSpawn = CalculateWave(wave);
        player = FindObjectOfType<PlayerController>();
        GetEnemiesToSpawn();

        //Get info for bounds
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        //Fill money graph with values to test
        //for (int i = 0; i < 25; i++) {
        //    moneyGraph.AddKey(i, CalculateMoney(i));
        //}

        //Fill zombie graph with values to test
        for (int i = 0; i < 25; i++) {
            zombieGraph.AddKey(i, CalculateWave(i));
        }
    }

    //Spawn explosion prefab at location
    //Get prefab from list
    public void ActivateExplosion(Vector3 pos)
    {
        GameObject exp = GetExplosion();
        exp.transform.position = pos;
        exp.SetActive(true);
    }

    public GameObject GetExplosion()
    {
        //Return hit obj
        for (int i = 0; i < explosionList.Count; i++)
        {
            if (!explosionList[i].activeInHierarchy)
            {
                return explosionList[i];
            }
        }

        GameObject obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionList.Add(obj);
        obj.SetActive(false);

        return obj;
    }

    public float CalculateWave(int w)
    {
        //float score = Mathf.RoundToInt(Mathf.Pow(2, w));
        float score = Mathf.RoundToInt(zombIncrease * w) + initialZombieScore;
        return score;
    }

    GameObject GetNewEnemy(List<GameObject> eList, int ets)
    {
        if (eList.Count <= 0)
        {
            GameObject o = Instantiate(enemy[ets], transform.position, Quaternion.identity);
            eList.Add(o);
            //o.SetActive(false);

            return o;
        }
        //Return hit obj
        for (int i = 0; i < eList.Count; i++)
        {
            if (!eList[i].activeInHierarchy && !enemiesThisWave.Contains(eList[i]))
            {
                return eList[i];
            }
        }

        GameObject obj = Instantiate(enemy[ets], transform.position, Quaternion.identity);
        eList.Add(obj);
        //obj.SetActive(false);

        return obj;
    }

    //What enemies we'll spawn this wave
    //We can object pool them later
    public List<GameObject> enemiesThisWave = new List<GameObject>();

    public void GetEnemiesToSpawn()
    {
        //Reset values
        float sco = 0;
        totalEnemies = 0;
        enemiesThisWave.Clear();

        //Loop and get random enemies until we have enough for the wave
        while (sco < enemiesToSpawn)
        {
            //Instantiate an enemy, add it to our list, then disable it. We should have everything object pooled beforehand
            int index = Random.Range(0, enemy.Length);
            GameObject o;
            switch (index)
            {
                case 0:
                    o = GetNewEnemy(zombies, 0);
                    break;
                case 1:
                    o = GetNewEnemy(zombieSquids, 1);
                    break;
                case 2:
                    o = GetNewEnemy(zombieBehemoths, 2);
                    break;
                default:
                    o = GetNewEnemy(zombies, 0);
                    break;
            }

            //GameObject o = Instantiate(enemy[Random.Range(0, enemy.Length)]);

            EnemyController e = o.GetComponent<EnemyController>();
            o.SetActive(false);
            sco += e.spawnScore;
            enemiesThisWave.Add(o);
            totalEnemies++;
        }
        curEnemies = totalEnemies;
    }

    public void DecrementEnemy()
    {
        curEnemies--;

        if (curEnemies <= 0 && nextWaveCools <= 0 && enemiesSpawned >= enemiesToSpawn)
        {
            GiveWaveMoney();
            nextWaveCools = timeBetweenWaves;
        }
    }

    //Modifier changes every 3 waves
    public float mod = 25;
    public float wavesToMod = 3;
    public float incAmt = 10;

    public float CalculateMoney(int w)
    {
        if (w % wavesToMod == 0) mod += mod;
        float toGive = Mathf.RoundToInt(mod + (w * incAmt));
        //float toGive = Mathf.RoundToInt(25 / (1 + Mathf.Pow(w, (2))));
        return toGive;
    }

    public void GiveWaveMoney()
    {
        player.AddMoney(CalculateMoney(wave));
    }

    private void Update()
    {
        if (waveTimerParent == null)
        {
            waveTimerParent = GameObject.FindGameObjectWithTag("WaveTimer");
            waveTimerText = GameObject.FindGameObjectWithTag("WaveText").GetComponent<TMP_Text>();
            zombiesLeftText = GameObject.FindGameObjectWithTag("ZombieText").GetComponent<TMP_Text>();
        }

        if (gameOverMenu == null)
        {
            gameOverMenu = GameObject.FindGameObjectWithTag("GameOver");
            gameOverMenu.SetActive(false);
        }

        if (gameOverMenu != null && !gameOverMenu.activeInHierarchy)
        {
            float minutes = Mathf.Floor(nextWaveCools / 60);
            float seconds = Mathf.RoundToInt(nextWaveCools % 60);
            float milli = Mathf.RoundToInt((nextWaveCools * 100) % 100);
            //waveTimerText.text = "Next Wave - " + minutes.ToString() + ":" + seconds.ToString() + ":" + milli.ToString();

            if (waveTimerParent != null && waveTimerParent != null)
            {
                if (nextWaveCools > 0)
                {
                    waveTimerParent.SetActive(true);
                    waveTimerText.text = seconds.ToString() + ":" + milli.ToString();
                }
                else
                {
                    FindObjectOfType<ShopProximityController>().ExitStore();
                    FindObjectOfType<VendingMachineController>().ExitStore();
                    waveTimerParent.SetActive(false);
                }
            }

            zombiesLeftText.text = curEnemies.ToString() + "/" + totalEnemies.ToString();
            //Add fill bar of enemies killed
            if (totalEnemies > 0) killImage.fillAmount = (curEnemies / totalEnemies);

            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    shopMenu.SetActive(!shopMenu.activeInHierarchy);
                }

                if (Input.GetKeyDown(KeyCode.K))
                {
                    var ob = FindObjectsOfType<MonoBehaviour>().OfType<IDamageable<float>>();
                    foreach (IDamageable<float> d in ob)
                    {
                        d.Damage(dmg);
                    }
                }
            }

            //Skip to wave press
            if (Input.GetButton("Submit") && curEnemies <= 0)
            {
                curHoldTime += Time.deltaTime;
            }
            if (Input.GetButtonUp("Submit"))
            {
                holdParent.gameObject.SetActive(false);
                curHoldTime = 0;
                holdImage.fillAmount = 0;
            }

            //Skip to wave button activation and filling
            if (curHoldTime > 0)
            {
                holdParent.gameObject.SetActive(true);
                holdImage.fillAmount = (curHoldTime / holdForWaveTime);
            }
            else
            {
                holdParent.gameObject.SetActive(false);
                holdImage.fillAmount = 0;
            }

            if (curEnemies > 0)
            {
                holdParent.gameObject.SetActive(false);
                holdImage.fillAmount = 0;
            }

            if (curHoldTime >= holdForWaveTime && curEnemies <= 0)
            {
                nextWaveCools = 0;
            }

            if (cools > 0) cools -= Time.deltaTime;

            if (nextWaveCools > 0) nextWaveCools -= Time.deltaTime;

            //If theres no enemies and our countdown is 0
            if (nextWaveCools <= 0 && curEnemies <= 0 && enemiesSpawned >= enemiesToSpawn)
            {
                //nextWaveCools = timeBetweenWaves;
                wave++;
                enemiesSpawned = 0;
                totalEnemies = 0;
                enemiesToSpawn = CalculateWave(wave);
                GetEnemiesToSpawn();
            }

            //If our cooldown is 0 and we havent spawned enough enemies yet
            if (cools <= 0 && enemiesSpawned < enemiesToSpawn && nextWaveCools <= 0)
            {
                SpawnEnemy();
            }

            nextWaveCools = Mathf.Clamp(nextWaveCools, 0, 999);

            if (freezeCurTime > 0) freezeCurTime -= Time.deltaTime;

            if (Time.timeScale <= 1f && freezeCurTime <= 0)
            {
                Time.timeScale += Time.deltaTime * freezeLerpSpd;
            }
        }
    }

    void SpawnEnemy()
    {
        //if (enemiesThisWave.Count > 0)
        //{
        //int num = Random.Range(0, 4);
        GameObject enemy = enemiesThisWave[0];
        //if (num == 0)
        //{
        //    //Spawn an enemy at a random position at the top of the screen
        //    //Instantiate(enemy[Random.Range(0, enemy.Length)], new Vector3(Random.Range(-bounds.x + 1f, bounds.x - 1f), bounds.y + Random.Range(0f, 3f)), Quaternion.identity);
        //    //Spawns out of camera range
        //    enemy.transform.position = new Vector3(Random.Range(-bounds.x + 1f, bounds.x - 1f), bounds.y + Random.Range(0f, 3f));
        //}
        //else if (num == 1)
        //{
        //    //Spawn an enemy at a random position at the bottom of the screen
        //    //Instantiate(enemy[Random.Range(0, enemy.Length)], new Vector3(Random.Range(-bounds.x + 1f, bounds.x - 1f), -bounds.y - Random.Range(0f, 3f)), Quaternion.identity);
        //    //Spawns out of camera range
        //    enemy.transform.position = new Vector3(Random.Range(-bounds.x + 1f, bounds.x - 1f), -bounds.y - Random.Range(0f, 3f));
        //}
        //else if (num == 2)
        //{
        //    //Spawn an enemy at a random position at the right of the screen
        //    //Instantiate(enemy[Random.Range(0, enemy.Length)], new Vector3(-bounds.x - Random.Range(0f, 3f), Random.Range(-bounds.y + 1f, bounds.y - 1f)), Quaternion.identity);
        //    //Spawns out of camera range
        //    enemy.transform.position = new Vector3(-bounds.x - Random.Range(0f, 3f), Random.Range(-bounds.y + 1f, bounds.y - 1f));
        //}
        //else
        //{
        //    //Spawn an enemy at a random position at the left of the screen
        //    //Instantiate(enemy[Random.Range(0, enemy.Length)], new Vector3(bounds.x + Random.Range(0f, 3f), Random.Range(-bounds.y + 1f, bounds.y - 1f)), Quaternion.identity);
        //    //Spawns out of camera range
        //    enemy.transform.position = new Vector3(bounds.x + Random.Range(0f, 3f), Random.Range(-bounds.y + 1f, bounds.y - 1f));
        //}

        //Spawn within map bounds
        Vector3 spawnPos = new Vector3(Random.Range(-boxSpawns.x / 2, boxSpawns.x / 2), Random.Range(-boxSpawns.y / 2, boxSpawns.y / 2));
        //Gotta see if we spawn the enemy in any colliders
        Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.5f);
        if (hit != null)
        {
            while (hit != null)
            {
                spawnPos = new Vector3(Random.Range(-boxSpawns.x / 2, boxSpawns.x / 2), Random.Range(-boxSpawns.y / 2, boxSpawns.y / 2));
                hit = Physics2D.OverlapCircle(spawnPos, 0.5f);
            }
        }
        if (hit == null) enemy.transform.position = spawnPos;
        enemy.SetActive(true);
        enemiesSpawned += enemy.GetComponent<EnemyController>().spawnScore;
        enemiesThisWave.Remove(enemy);
        //Reset our timer so we have a wait between enemy spawns
        cools = Random.Range(timeBetweenSpawnsLow, timeBetweenSpawnsHigh);
        //}
    }

    public void FreezeTime()
    {
        Time.timeScale = freezeTime;
        freezeCurTime = freezeResumeTime;

        Invoke("ResumeTime", freezeResumeTime);
    }

    public void ResumeTime()
    {
        Time.timeScale = freezeFPS;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(boxSpawns.x, boxSpawns.y, 1));
    }

    public void PlaySound(AudioClip cl, float vol = 1f)
    {
        //soundSrc.volume = vol;
        soundSrc.PlayOneShot(cl, vol);
    }

    public void EnemyDied()
    {
        //Debug.Log("Dead Enemy");
    }

    public void GameOver()
    {
        //Activate UI for resetting level
        gcUI.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
