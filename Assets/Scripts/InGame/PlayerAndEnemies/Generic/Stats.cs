using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Basic Info")]
    public string characterName;

    [Header("HP")]
    //Atributos de vidas    
    [SerializeField]
    public Indicator HP;


    //Atributos de movimiento
    [Header("Movement")]
    public float speedForce;
    public float jumpForce;
    public int maxJump;

    [Header("KnockBack")]
    public int forceKnockBack;
    public float secondsKnockBack;

    [Header("Attack")]
    public float attackDamage;
    public float attackSpeed;
    public float attackTimeBetween = 0.5f;
    public GameObject attack;//TODO List<GameObject> attacks + ENUM

    //Invulnerabilidad
    [Header("Invulnerability")]
    public bool invulnerable = false;
    public float secondsInvulnerable;

    //Clips de sonido
    [Header("Sounds")]
    public AudioClip attackSFX;
    public AudioClip damageSFX;

    private void Update()
    {
        //Se comprueba si el deterioro esta o no activo del HP
        if(HP.autoUpdateRate > 0 || (HP.autoUpdateRate < 0 && !invulnerable ))
        {
            HP.CurrentValue += HP.autoUpdateRate * Time.deltaTime;
        }

        //TODO hacer con todos los stats del tipo Indicator
    }

    public virtual void RestartStats() {
        HP.RestartStats();
    }
}
