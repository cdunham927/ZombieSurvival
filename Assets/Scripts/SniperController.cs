using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperController : MonoBehaviour
{
    public float atk;
    LineRenderer line;
    [HideInInspector] public float laserCools;
    public float maxSize = 0.325f;
    public float lerpSpd;
    GameController cont;
    public bool sizeIncrease = false;

    void Awake()
    {
        cont = FindObjectOfType<GameController>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    public void StartLaserCoroutine()
    {
        if (laserCools <= 0)
        {
            StopCoroutine(FireLaser());
            StartCoroutine(FireLaser());
        }
    }

    private void Update()
    {
        if (laserCools > 0) laserCools -= Time.deltaTime;

        line.startWidth = Mathf.Lerp(line.startWidth, 0, lerpSpd * Time.deltaTime);

        if (line.startWidth <= 0.00125f)
            line.enabled = false;
    }

    public IEnumerator FireLaser()
    {
        line.enabled = true;
        line.startWidth = (sizeIncrease) ? maxSize * 3 : maxSize;

        while (true)
        {
            //line.material.mainTextureOffset = new Vector2(0, Time.time);

            Ray2D ray = new Ray2D(transform.position, transform.up);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up * 15f);

            line.SetPosition(0, ray.origin);

            if (hit.collider != null)
            {
                //line.SetPosition(1, hit.point);

                if (hit.collider.CompareTag("Enemy") && line.startWidth > 0.00125f)
                {
                    hit.collider.gameObject.GetComponent<IDamageable<float>>().Damage(atk);

                    //HitObjController hitt = cont.GetHitObj().GetComponent<HitObjController>();
                    //hitt.transform.position = hit.point + new Vector2(Random.Range(-0.3f, 0.3f), 0);
                    //hitt.SetText(Mathf.RoundToInt(atk), false);
                    //hitt.gameObject.SetActive(true);
                }
            }

            line.SetPosition(1, ray.GetPoint(15f));

            yield return null;
        }
    }
}
