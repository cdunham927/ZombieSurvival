using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController cont;
    public GameObject shopMenu;
    public bool inMenu;
    //Waves
    public int wave = 1;
    public GameObject[] enemy;
    public int enemiesToSpawn;
    //If player kills all enemies, the countdown to the next wave starts
    [SerializeField]
    int enemiesSpawned;
    //Keeps track of enemies spawned during this wave
    int curEnemies;
    public float timeBetweenWaves = 10f;
    float nextWaveCools;
    float cools;
    public float timeBetweenSpawnsLow;
    public float timeBetweenSpawnsHigh;
    Vector2 bounds;

    //Test IDamageable interface
    public float dmg = 5;


    private void Awake()
    {
        if (cont == null)
        {
            cont = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        //Get info for bounds
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        enemiesSpawned = 0;
    }

    public void CalculateWave()
    {

    }

    public void DecrementEnemy()
    {
        curEnemies--;

        if (curEnemies <= 0) nextWaveCools = timeBetweenWaves;
    }

    private void Update()
    {
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

        if (cools > 0) cools -= Time.deltaTime;

        if (nextWaveCools > 0) nextWaveCools -= Time.deltaTime;

        //If theres no enemies and our countdown is 0
        if (nextWaveCools <= 0 && curEnemies <= 0)
        {
            CalculateWave();
            enemiesSpawned = 0;
        }

        //If our cooldown is 0 and we havent spawned enough enemies yet
        if (cools <= 0 && enemiesSpawned < enemiesToSpawn && nextWaveCools <= 0)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        int num = Random.Range(0, 4);
        if (num == 0)
        {
            //Spawn an enemy at a random position at the top of the screen
            Instantiate(enemy[Random.Range(0, enemy.Length)], new Vector3(Random.Range(-bounds.x + 1f, bounds.x - 1f), bounds.y + Random.Range(0f, 3f)), Quaternion.identity);
        }
        else if (num == 1)
        {
            //Spawn an enemy at a random position at the bottom of the screen
            Instantiate(enemy[Random.Range(0, enemy.Length)], new Vector3(Random.Range(-bounds.x + 1f, bounds.x - 1f), -bounds.y - Random.Range(0f, 3f)), Quaternion.identity);
        }
        else if (num == 2)
        {
            //Spawn an enemy at a random position at the right of the screen
            Instantiate(enemy[Random.Range(0, enemy.Length)], new Vector3(-bounds.x - Random.Range(0f, 3f), Random.Range(-bounds.y + 1f, bounds.y - 1f)), Quaternion.identity);
        }
        else if (num == 3)
        {
            //Spawn an enemy at a random position at the left of the screen
            Instantiate(enemy[Random.Range(0, enemy.Length)], new Vector3(bounds.x + Random.Range(0f, 3f), Random.Range(-bounds.y + 1f, bounds.y - 1f)), Quaternion.identity);
        }
        enemiesSpawned++;
        curEnemies++;
        //Reset our timer so we have a wait between enemy spawns
        cools = Random.Range(timeBetweenSpawnsLow, timeBetweenSpawnsHigh);
    }

    public void EnemyDied()
    {
        Debug.Log("Dead Enemy");
    }
}
