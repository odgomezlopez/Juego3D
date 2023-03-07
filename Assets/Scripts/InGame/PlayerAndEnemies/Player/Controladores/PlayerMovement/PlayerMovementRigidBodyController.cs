using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementRigidBodyController : PlayerMovementController
{
    //private Rigidbody fisicas;
    private Rigidbody fisicasRigidBody;

    protected override void Start()
    {
        base.Start();
        fisicasRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (!moveEnable) return;

        if (moveInput == Vector3.zero)
        {
            fisicasRigidBody.velocity = new Vector3(0, fisicasRigidBody.velocity.y, 0);
        }
        else
        {
            float airMomentum = (inGround) ? 1 : stats.airMomentum;
            fisicasRigidBody.AddForce(transform.localToWorldMatrix * moveInput.normalized * stats.speedForce * airMomentum, ForceMode.Acceleration);
        }

    }

    //Sobreescribir acciones
    protected override IEnumerator JumpPhysics()
    {
        yield return new WaitForFixedUpdate();
        fisicasRigidBody.velocity = new Vector3(fisicasRigidBody.velocity.x, 0, fisicasRigidBody.velocity.z);
        fisicasRigidBody.AddForce(Vector3.up * stats.jumpForce, ForceMode.Impulse);
    }

    protected override IEnumerator KnockBackPhysics(Vector3 direction, float forceKnockBack, float timeKnockBack)
    {
        yield return new WaitForFixedUpdate();//Espero al siguiente fixedUpdate para un comportamiento adecuado de las fisicas
        if (moveEnable)
        {
            DisableMovement();//Deshabilito el movimiento
            fisicasRigidBody.velocity = Vector3.zero;//Paro al jugador
            fisicasRigidBody.AddForce(direction * forceKnockBack, ForceMode.Impulse); //Y le aplico la fuerza de knockback
            yield return new WaitForFixedUpdate(); //espero hasta el siguiente frame
            yield return new WaitForSecondsRealtime(timeKnockBack);//por ultimo, espero el tiempo que se indica
            EnableMovement();//y vuelvo a activar el movimiento
            fisicasRigidBody.angularVelocity = Vector3.zero;
        }
    }

    //Metodos de control de salto
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            numSaltos = 0;
            inGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            inGround = false;
        }
    }
}
