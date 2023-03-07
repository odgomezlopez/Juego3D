using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : ActionController
{
    public GameObject attackOrigin;


    [Header("Attack Base Conf.")]
    [Range(0,50)] public float lifeTimeSeconds=5f;
    public bool destroyOnColision=true;
    public bool knockBack = false;

    //private info
    protected AttackStats attackStats;
    
    private Coroutine coroutine;



    protected virtual void Awake()
    {
        attackStats = GetComponent<AttackStats>();
    }

    //Corutina de ataque
    public override IEnumerator ExecuteAction()
    {
        coroutine = StartCoroutine(ExecuteAttack(Vector3.forward));
        yield return null;
    }

    public virtual IEnumerator ExecuteAttack(Vector3 dir)
    {
        yield return null;
    }

    //Sistema de colisiones del ataque
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            AttackEnter(collision.gameObject);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            AttackEnter(collision.gameObject);
        }
    }


    //Codigo de gestion de colisión
    private void AttackEnter(GameObject attackTarget)
    {
        if (attackOrigin == null || !attackOrigin.CompareTag(attackTarget.tag)) //Comrpuebo que el emisor y receptor del daño no son el mismo GameObject
        {
            //Si me encuentro algo matable, paro la corutina
            if (coroutine != null) StopCoroutine(coroutine);

            //Calculo el daño
            IGenericController golpeado = attackTarget.GetComponent<IGenericController>();
            float attackDamage = attackStats.baseAttack * golpeado.GetStats().attackDamage;
            golpeado.OnDamage(attackDamage, gameObject,true);

            //Por último, el ataque se autodestruye
            if(destroyOnColision) Destroy(gameObject);
        }
    }
}
