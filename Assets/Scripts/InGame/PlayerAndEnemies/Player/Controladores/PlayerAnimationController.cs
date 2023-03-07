using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Utils.Utils;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody fisicas;

    PlayerInit init;
    PlayerMovementController movementController;

    public bool ready;

    IEnumerator Start()
    {
        //TODO ESPERAR AL INIT
        ready = false;

        init = GetComponent<PlayerInit>();
        yield return new WaitUntil(() => (init.isReady));

        movementController = GetComponent<PlayerMovementController>();

        animator = gameObject.GetComponentInChildren<Animator>();
        fisicas = gameObject.GetComponent<Rigidbody>();

        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready) return;
        //AnimacionCodigo();
        AnimacionParametros();
    }

    private void AnimacionParametros()
    {
        animator.SetFloat("velocidadX", Math.Abs(fisicas.velocity.x));
        animator.SetFloat("velocidadY", fisicas.velocity.y);
        animator.SetBool("tocaSuelo",false);
    }

    /*public enum AnimationOptions { IDLE, RUN, JUMP };
    private void AnimacionCodigo()
    {
        if (Math.Abs(fisicas.velocity.y) > 1 || !movementController.inGround)
        {
            animator.Play(AnimationOptions.JUMP.ToString());
        }
        else if (Math.Abs(fisicas.velocity.x) > 1)
        {
            animator.Play(AnimationOptions.RUN.ToString());
        }
        else {
            animator.Play(AnimationOptions.IDLE.ToString());
        }
    }*/
}
