using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContextualActionController : MonoBehaviour
{
    //Acciones contextuales
    //[System.NonSerialized]
    public ContextualActionController contextualAction;

    //Controladores con los que tiene que comunicarse
    private PlayerController playerController;
    private PlayerStateController stateController;
    private PlayerInGameUIController inGameUIController;

    // Start is called before the first frame update
    void Start()
    {
        //Acciones contextuales
        contextualAction = null;

        //Controladores de apoyo
        playerController = GetComponent<PlayerController>();
        inGameUIController = GetComponent<PlayerInGameUIController>();
        stateController = GetComponent<PlayerStateController>();
    }

    public void EnableContextualAction(ContextualActionController newAction, bool checkRequirement)
    {
        if (!stateController.contextualActionEnabled) return;

        inGameUIController.EnableUI(newAction.actionName, checkRequirement);

        if (contextualAction == null)
        {
            if (checkRequirement)
            {
                contextualAction = newAction;
            }

        }
    }

    public void DiableContextualAction()
    {
        inGameUIController.DisableUI();
        contextualAction = null;
    }
    public void ExecuteContextualAction() {
        StartCoroutine(ExecuteContextualActionCoroutine());
    }
    private IEnumerator ExecuteContextualActionCoroutine()
    {
        bool disGP = contextualAction.disableGamePlay;//Lo guardo en una variable por si la accion contextual destruyera el objeto donde esta el componente

        inGameUIController.DisableUI();

        //Se desactiva el gameplay si lo requiere la acción
        if (disGP)
        {
            playerController.DisableGamePlay();
        }

        //Se ejecuta la acción contextual
        yield return contextualAction.ExecuteAction();

        //Se reactiva el game play
        if (disGP)
        {
            playerController.EnableGamePlay();
        }
        DiableContextualAction();
    }

    public bool IsContextualActionAsigned()
    {
        return contextualAction != null;
    }
}
