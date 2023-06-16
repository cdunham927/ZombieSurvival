using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiedEvent : MonoBehaviour
{
    public GameEvent ev;

    private void OnDisable()
    {
        ev.Raise();
    }
}
