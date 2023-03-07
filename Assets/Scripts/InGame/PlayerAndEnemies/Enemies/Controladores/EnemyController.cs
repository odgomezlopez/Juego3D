using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IGenericController
{
    private EnemyStats stats;
    LevelControllerBase control;
    ParticleSystem explosion;
    GameObject mesh,UI;

    GenericSoundController soundController;
    // Start is called before the first frame update
    void Awake()
    {
        control = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelControllerBase>();

        stats = GetComponent<EnemyStats>();
        soundController = GetComponent<GenericSoundController>();
        stats.RestartStats();

        mesh = gameObject.transform.Find("Mesh").gameObject;
        UI = gameObject.transform.Find("Life").gameObject;

        explosion = GetComponentInChildren<ParticleSystem>();

        //Me suscribo al cambio de HP del jugador
        stats.HP.OnIndicatorChange += OnHPUpdate;
    }

    //Gestion de Stats
    public Stats GetStats()
    {
       return stats;
    }

    //Gestion de daño
    public void OnHealing(float heal)
    {
        stats.HP.CurrentValue += heal;
    }

    public void OnDamage(float damage, GameObject damageSource, bool knockback = true, AttackType attackType = AttackType.HIT)
    {
        soundController.OnAttackSound();
        stats.HP.CurrentValue -= damage;
    }



    //Gestion de gameover si se acaba la vida desde cualquier lado
    public virtual void OnHPUpdate(float val)
    {
        if (stats.HP.CurrentValue <= 0)
        {
            StartCoroutine(OnDie());
        }

    }

    IEnumerator OnDie()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSecondsRealtime(0.2f);

        soundController.OnDamageSound();
        explosion.Play();
        mesh.SetActive(false);
        UI.SetActive(false);

        yield return new WaitForSecondsRealtime(0.2f);

        if (control is LevelControllerPoints)
        {
            ((LevelControllerPoints)control).AddPoints(stats.points);
        }

        yield return new WaitForSecondsRealtime(3f);

 

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        stats.HP.OnIndicatorChange -= OnHPUpdate;//HAY QUE DESUSCRIBIRSE AL MORIR
    }
}
