using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAttack : AttackController
{
    private Light myLight;

    protected override void Start()
    {
        base.Start();
        myLight = GetComponent<Light>();
    }
    public override IEnumerator ExecuteAttack(Vector3 dir)
    {
        //Calculo la velocidad
        float speed = attackStats.baseSpeed;
        float initTime = Time.time;

        //Me desacoplo al padre
        transform.SetParent(null);


        if (attackOrigin != null)
        { //Si hay un originador del ataque tenerlo en cuenta en el calculo de las estadisticas

            speed = attackStats.baseSpeed * attackOrigin.GetComponent<PlayerController>().GetStats().attackSpeed;//TODO poner la formula de daño qe s quuiera
        }

        //base.ExecuteAction();
        //El ataque empieza a moverse durante 5 segundos
        yield return new WaitForEndOfFrame();
        //Paso 1. Activar SFX y VFX

        //Paso 2. Mover hacia adelante
        yield return new WaitForFixedUpdate();

        Vector3 direccion = (transform.localToWorldMatrix * Vector3.forward).normalized;//
        gameObject.GetComponent<Rigidbody>().AddForce(direccion*speed, ForceMode.Impulse);


        //Espero a que termite
        yield return new WaitForSecondsRealtime(lifeTimeSeconds);
        while(myLight.intensity > 0)
        {
            myLight.intensity -= 0.02f;
            yield return new WaitForSecondsRealtime(0.05f);

        }

        Destroy(gameObject);

    }
}