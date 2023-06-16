using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbedWire : MonoBehaviour
{
    GameController cont;
    public AudioClip clip;
    public float timeBetweenSounds = 1f;
    float curTime = 0f;
    AudioSource src;
    public float pitchLow;
    public float pitchHigh;
    public float pitch;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        cont = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (curTime > 0) curTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && curTime <= 0)
        {
            pitch = Random.Range(pitchLow, pitchHigh);
            src.Play();
            curTime = timeBetweenSounds;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && curTime <= 0)
        {
            pitch = Random.Range(pitchLow, pitchHigh);
            src.Play();
            curTime = timeBetweenSounds;
        }
    }
}
