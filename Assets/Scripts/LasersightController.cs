using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasersightController : MonoBehaviour
{
    public LayerMask hitMask;
    public LineRenderer line;
    WeaponController weapon;
    public float maxLength;

    private void Awake()
    {
        weapon = GetComponentInParent<WeaponController>();
        line = GetComponent<LineRenderer>();
    }

    Ray2D hitRay;

    private void FixedUpdate()
    {
        line.SetPosition(0, transform.position);
        //line.SetPosition(1, transform.position + (weapon.transform.right * maxLength));

        //Enable laser
        hitRay = new Ray2D(transform.position, weapon.transform.right);
        //hit = Physics2D.Raycast(hitRay.origin, weapon.transform.right);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, weapon.transform.right, maxLength * 10f, hitMask);
        //RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, weapon.transform.right, maxLength);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Wall"))
                line.SetPosition(1, hit.point);
        }
    }
}
