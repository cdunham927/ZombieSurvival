using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperController : MonoBehaviour
{
    public float dmgLow;
    public float dmgHigh;
    LineRenderer line;
    [HideInInspector] public float laserCools;
    public float maxSize = 0.325f;
    public float lerpSpd;
    GameController cont;
    //public bool sizeIncrease = false;
    public float maxLength = 15f;
    public float disableTime = 0.1f;

    WeaponController weapon;

    public LayerMask hitMask;

    void Awake()
    {
        weapon = GetComponentInParent<WeaponController>();
        cont = FindObjectOfType<GameController>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    Ray2D hitRay;
    bool hitWall = false;
    //RaycastHit2D hit;
    float closestWall;

    public void StartLaserCoroutine()
    {
        hitWall = false;
        closestWall = 999;

        line.SetPosition(0, transform.position);
        //line.SetPosition(1, transform.position + (weapon.transform.right * maxLength));
        line.enabled = true;
        Invoke("DisableLaser", disableTime);

        //Enable laser
        hitRay = new Ray2D(transform.position, weapon.transform.right);
        //hit = Physics2D.Raycast(hitRay.origin, weapon.transform.right);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, weapon.transform.right, maxLength * 10f, hitMask);
        //RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, weapon.transform.right, maxLength);

        if (hit.Length > 0)
        {
            foreach (RaycastHit2D h in hit)
            {
                if (h.collider.CompareTag("Wall"))
                {
                    hitWall = true;
                    closestWall = h.distance;
                    line.SetPosition(1, h.point);
                }
                if (h.distance < closestWall && h.collider.CompareTag("Enemy"))
                {
                    h.collider.gameObject.GetComponent<EnemyController>().Damage(Random.Range(dmgLow, dmgHigh));
                }
            }
        }
        if (!hitWall)
        {
            line.SetPosition(1, transform.position + (weapon.transform.right) * maxLength);
        }

        //if (hit.collider != null)
        //{
        //    //if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Enemy"))
        //    //    line.SetPosition(1, hit.point);
        //
        //    if (hit.collider.CompareTag("Wall"))
        //    {
        //        line.SetPosition(1, hit.point);
        //        //hitWall = true;
        //    }
        //
        //    if (hit.collider.CompareTag("Enemy"))
        //    {
        //        Debug.Log("Hitting enemy");
        //        hit.collider.gameObject.GetComponent<EnemyController>().Damage(Random.Range(dmgLow, dmgHigh));
        //
        //        //HitObjController hitt = cont.GetHitObj().GetComponent<HitObjController>();
        //        //hitt.transform.position = hit.point + new Vector2(Random.Range(-0.3f, 0.3f), 0);
        //        //hitt.SetText(Mathf.RoundToInt(atk), false);
        //        //hitt.gameObject.SetActive(true);
        //    }
        //}

        //if (hitWall)
        //{
        //    line.SetPosition(1, hit.point);
        //}
        //else line.SetPosition(1, hitRay.origin + (Vector2)weapon.transform.right * maxLength);

        //if (laserCools <= 0)
        //{
        //    StopCoroutine(FireLaser());
        //    StartCoroutine(FireLaser());
        //}
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position + (weapon.transform.right * maxLength));
    }

    void DisableLaser()
    {
        line.enabled = false;
    }

    private void Update()
    {
        //if (laserCools > 0) laserCools -= Time.deltaTime;
        //
        //line.startWidth = Mathf.Lerp(line.startWidth, 0, lerpSpd * Time.deltaTime);
        //
        //if (line.startWidth <= 0.00125f)
        //    line.enabled = false;
    }

    //public IEnumerator FireLaser()
    //{
    //    line.enabled = true;
    //    //line.startWidth = (sizeIncrease) ? maxSize * 3 : maxSize;
    //
    //    while (true)
    //    {
    //        //line.material.mainTextureOffset = new Vector2(0, Time.time);
    //
    //        Ray2D ray = new Ray2D(transform.position, transform.right);
    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * 15f);
    //
    //        line.SetPosition(0, ray.origin);
    //
    //        if (hit.collider != null)
    //        {
    //            //line.SetPosition(1, hit.point);
    //
    //            if (hit.collider.CompareTag("Enemy") && line.startWidth > 0.00125f)
    //            {
    //                hit.collider.gameObject.GetComponent<IDamageable<float>>().Damage(Random.Range(dmgLow, dmgHigh));
    //
    //                //HitObjController hitt = cont.GetHitObj().GetComponent<HitObjController>();
    //                //hitt.transform.position = hit.point + new Vector2(Random.Range(-0.3f, 0.3f), 0);
    //                //hitt.SetText(Mathf.RoundToInt(atk), false);
    //                //hitt.gameObject.SetActive(true);
    //            }
    //        }
    //
    //        line.SetPosition(1, ray.GetPoint(maxLength));
    //
    //        yield return null;
    //    }
    //}
}
