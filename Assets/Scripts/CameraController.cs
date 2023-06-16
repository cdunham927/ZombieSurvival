using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpd;
    Vector3 mod;
    [Range(0, 1)]
    public float clampAmt = 0.5f;
    PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate()
    {
        mod = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mod = Vector3.ClampMagnitude(mod, clampAmt);

        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + mod.x, player.transform.position.y + mod.y, transform.position.z), lerpSpd * Time.deltaTime);
    }
}
