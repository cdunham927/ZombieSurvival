using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShellController : MonoBehaviour
{
    public float rotSpd;
    public int dir;
    Animator anim;
    public AnimationClip clip;
    public float targetPosY = 1f;
    float targY;
    public float lerpSpd;
    public float changeSpd;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        targY = targetPosY;
        dir = Random.Range(-1, 1);
        anim.Play("ShellEmpty");
        Invoke("Disable", clip.length);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        targY -= changeSpd * Time.deltaTime;
        transform.Rotate(0, 0, rotSpd * Time.deltaTime * Mathf.Sign(dir));

        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, targY, 0), Time.deltaTime * lerpSpd);
    }
}
