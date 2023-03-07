using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Utils.Utils;


public class PlayerMovementController : MonoBehaviour
{


    private Rigidbody fisicas;

    //Variables para la gestion del movimiento en el eje X
    private bool moveEnable=true;
    private Vector3 movementInput;
    private float speed;


    public PlayerStats stats;
    //Controlador de estados
    private PlayerStateController stateController;

    //Giro
    Vector3 turningInput;
    float turningSpeed;


    //Variables para la gestion de saltos
    private int numSaltos = 0;
    public bool inGround;

    void Start()
    {
        moveEnable = true;
        fisicas = gameObject.GetComponent<Rigidbody>();

        stateController = GetComponent<PlayerStateController>();
        stats = GameObject.FindGameObjectWithTag("GameData").GetComponent<PlayerStats>();

        numSaltos = 0;


    }

    void FixedUpdate()
    {
        
        //Hacemos las acciones de las fisicas
        MovePhysics();
        TurningFixed();
    }



    //Activar/Desactivar Movimiento
    public void EnableMovement()
    {
        moveEnable = true;
    }

    public void DisableMovement()
    {
        moveEnable = false;
    }

    //Movimiento
    public void Move(Vector2 input,float speed)
    {
        //if (!moveEnable) return;
        //if (input.x != movementInput.x) fisicas.velocity= new Vector3(0, fisicas.velocity.y, fisicas.velocity.z);

        this.movementInput = (new Vector3(input.x,0, input.y)).normalized;
        this.speed = speed;
    }

    public bool Jump(float jumpForce, int jumpMax)
    {
        if (!moveEnable) return false;
        if (numSaltos < jumpMax)
        {
            //if (numSaltos > 0) jumpForce = jumpForce / 2;
            StartCoroutine(JumpPhysics(jumpForce));
            //if(numSaltos==0) StartCoroutine(IsJumpEnded());

            numSaltos++;
            return true;
        }
        return false;
    }

    public void Turn(Vector2 input, float speed)
    {
        this.turningInput = new Vector3(0, input.x, 0).normalized;
        this.turningSpeed = speed;
    }

    //KnockBack
    public void KnockBack(Vector3 direction, float forceKnockBack, float timeKnockBack)
    {
        if (!moveEnable) return; //No se permiten dos KnockBack simultaneos
        StartCoroutine(KnockBackPhysics(direction, forceKnockBack, timeKnockBack));
        this.movementInput = Vector2.zero;

    }

    //METODOS DE MANIPULACION DE FISICAS

    private void MovePhysics()
    {
        if (!moveEnable) return;
        if (movementInput == Vector3.zero)
        {
            fisicas.velocity = new Vector3(0, fisicas.velocity.y, 0);
            return;
        }

       //1. Calculo de la velocidad
       Vector3 velocityAux1 = movementInput*stats.speedForce;//Calculo la velocidad en cada eje

        //Transformo el movimiento hacia donde miro
        velocityAux1 = transform.localToWorldMatrix * velocityAux1;//Y la convierto a coordenadas mundo 

        //2. Aplico la fuerzas
        //Debug.Log(velocityAux1);
        fisicas.AddForce(velocityAux1,ForceMode.Acceleration);

    }

    private void TurningFixed()
    {
        transform.Rotate(turningInput * turningSpeed * Time.deltaTime);
    }

    private IEnumerator JumpPhysics(float jumpForce)
    {
        yield return new WaitForFixedUpdate();
        if (moveEnable)
            fisicas.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }


    private IEnumerator KnockBackPhysics(Vector3 direction, float forceKnockBack,float timeKnockBack)
    {
        yield return new WaitForFixedUpdate();//Espero al siguiente fixedUpdate para un comportamiento adecuado de las fisicas
        if (moveEnable)
        {
            DisableMovement();//Deshabilito el movimiento
            fisicas.velocity = Vector2.zero;//Paro al jugador
            fisicas.AddForce(direction * forceKnockBack, ForceMode.Impulse); //Y le aplico la fuerza de knockback
            yield return new WaitForFixedUpdate(); //espero hasta el siguiente frame
            yield return new WaitForSecondsRealtime(timeKnockBack);//por ultimo, espero el tiempo que se indica
            EnableMovement();//y vuelvo a activar el movimiento
            fisicas.angularVelocity = Vector3.zero;


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
