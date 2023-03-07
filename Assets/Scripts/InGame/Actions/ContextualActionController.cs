using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualActionController : ActionController
{
    public enum ActionTrigger
    {
        BYTRIGGER,
        BYDISTANCE
    };

    [Header("Type of Trigger")]
    public ActionTrigger type;

    [Header("AutoTrigger Conf.")]
    public bool autoTriggerOnEnter = false;
    public string autoTriggerSecundaryTag="PlayerAttack"; //TODO

    [Header("GamePlay and UI Conf.")]
    public bool disableGamePlay = false;
    public bool alwaysShowUI = false;
    private bool contextualActive = false;

    [Header("BYDISTANCE Parameter")]
    public float distance = 0;

    [Header("Max Actions")]
    public bool infinite = true;
    private int numAction = 0;
    public int maxAction = 1;

    //Control parameter
    private bool inside;
    private GameObject player;
    private PlayerContextualActionController playerContextualActionController;

    //Nota. Este script es separable en dos. PlayerContextualAction, con la parte de colisiones, y PlayerTriggeredAction con la parte de autoactivación y distancia. 

    protected override void Start()
    {
        inside = false;
        contextualActive = false;

        player = GameObject.FindGameObjectWithTag("Player");
        playerContextualActionController = player.GetComponent<PlayerContextualActionController>();

    }

    protected virtual void Update()
    {

        if (type == ActionTrigger.BYTRIGGER && inside)
        {
            OnPlayerEnter();
        }

        if (contextualActive && !CheckNumAction())
        {
            OnPlayerExit();
        }


        if (type == ActionTrigger.BYDISTANCE)
        {
            CheckDistance();
        }
 
    }

    //METODOS DE FUNCIONAMIENTO CUANDO EL JUGADOR ENTRA EN EL AREA DE COLISION

    private void OnTriggerEnter(Collider collision)
    {
        if (type != ActionTrigger.BYTRIGGER) return;

        if (collision.gameObject.CompareTag("Player") || (autoTriggerOnEnter && collision.gameObject.CompareTag(autoTriggerSecundaryTag)))
        {
            OnPlayerEnter();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (type != ActionTrigger.BYTRIGGER) return;

        if (collision.gameObject.CompareTag("Player") || (autoTriggerOnEnter && collision.gameObject.CompareTag(autoTriggerSecundaryTag)))
        {
            OnPlayerEnter();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (type != ActionTrigger.BYTRIGGER) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerExit();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (type != ActionTrigger.BYTRIGGER) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerExit();
        }
    }

    private void OnPlayerEnter()
    {
        if (autoTriggerOnEnter)
        {
            if (CheckRequirement()) StartCoroutine(ExecuteAction());
        }
        else
        {
            if (CheckRequirement() || (alwaysShowUI && CheckNumAction())) {
                playerContextualActionController.EnableContextualAction(this, CheckRequirement());
                contextualActive = true;
            }
        }
        inside = true;
    }

    private void OnPlayerExit()
    {
        if (!autoTriggerOnEnter)
        {
            playerContextualActionController.DiableContextualAction();
        }
        inside = false;
        contextualActive = false;
    }

    //Metodos de distancia
    private void CheckDistance()
    {
        if (type != ActionTrigger.BYDISTANCE) return;

        if (Vector3.Distance(transform.position, player.transform.position) < distance)
        {
            OnPlayerEnter();
        }
        else
        {
            OnPlayerExit();
        }
    }


    public override IEnumerator ExecuteAction()
    {
        numAction++;
        yield return new WaitForEndOfFrame();
    }

    public bool CheckNumAction()
    {
        return (infinite || numAction < maxAction);
    }

    public override bool CheckRequirement()
    {
        return CheckNumAction();
    }
}
