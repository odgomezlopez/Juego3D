using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Utils.Utils;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementCharacterController : PlayerMovementController
{
    //private Rigidbody fisicas;
    private CharacterController fisicasController;

    protected override void Start()
    {
        base.Start();
        fisicasController = gameObject.GetComponent<CharacterController>();
    }

    protected override void Update()
    {
        base.Update();
        ApplyGravity();
        PlayerMovement();
    }

    void ApplyGravity()
    {
        inGround = IsGrounded(gameObject);
        if (inGround) numSaltos = 0;

        if (inGround && moveInput.y < 0)
        {
            moveInput.y = 0;
        }

        if (!inGround) {
            moveInput.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    void PlayerMovement()
    {
        if (!moveEnable) return;
        float airMomentum = (inGround) ? 1 : stats.airMomentum;
        fisicasController.Move(transform.localToWorldMatrix * new Vector3(0, moveInput.y, moveInput.z * stats.speedForce * airMomentum)  * Time.deltaTime);

        FaceMoveDirection();
    }

    protected void FaceMoveDirection()
    {
        Vector3 faceDirection = new(0, moveInput.x , 0);

        if (faceDirection == Vector3.zero)
            return;

        transform.Rotate(faceDirection * stats.turnHorizontalSpeed * Time.deltaTime);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(faceDirection*360), Time.deltaTime);
    }

    //Sobreescribir acciones
    protected override IEnumerator JumpPhysics()
    {
        moveInput.y = Mathf.Sqrt(stats.jumpForce * -2f * Physics.gravity.y);
        yield return null;
    }

    //Metodo para detectar colisiones cuando se utiliza el character controller
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }
}
