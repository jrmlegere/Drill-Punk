using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LairEnemy : MonoBehaviour
{
    public GameObject home;

    public void OnDestroy()
    {
        if (GetComponent<EnemyManager>().health <= 0)
        {
            home.GetComponent<LairBehaviour>().enemyCount--;
        }
    }
}
