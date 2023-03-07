using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    //Contador de info
    [Header("Player Info")]
    public int deathCount;

    [Header("Player Look")]
    public float turnHorizontalSpeed;

    public float turnVerticalSpeed;
    public float minVerticalPos;
    public float maxVerticalPos;

    //public Vector3 lookRange;



    //Constructor
    public override void RestartStats()
    {
        base.RestartStats();
        invulnerable = false;
    }

}
