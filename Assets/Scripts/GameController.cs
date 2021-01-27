using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public GameObject shopMenu;

    //Waves
    public int wave = 1;
    bool timerStarted = false;
    public float timeBetweenSpawns = 1f;
    public GameObject enemy;
    public int enemiesToSpawn;
    float timeToNextWave;


    //Test IDamageable interface
    public float dmg = 5;

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
    }

    public void EnemyDied()
    {
        Debug.Log("Dead Enemy");
    }

    IEnumerator DoWave()
    {

        yield return null;
    }
}
