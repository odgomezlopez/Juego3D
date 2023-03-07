using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    EnemyStats stats;

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
    }

}
