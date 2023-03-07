using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenericController
{
    //Stats
    public Stats GetStats();

    //Metodos genericos entre jugadores y enemigos
    public void OnHealing(float heal);

    public void OnDamage(float damage, GameObject damageSource, bool knockback = true, AttackType attackType = AttackType.HIT);


    //Gestion de eventos de cambio de HP
    public void OnHPUpdate(float val);

}
