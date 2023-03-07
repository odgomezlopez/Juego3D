using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateController : MonoBehaviour
{
    //Gestión de estados
    public PlayerStates previousState;
    public PlayerStates currentState;


    //Referencias a los subcontroladores necesarios
    private PlayerMovementController movementController;

    private PlayerInput inputController;

    private InputAction m_MoveAction;
    private InputAction m_LookAction;
    private InputAction m_ContextualAction;
    private InputAction m_AttackAction;
    private InputAction m_JumpAction;

    private InputAction m_MenuAction;
    private InputAction m_PauseAction;
    private InputAction m_ShowHUDAction;
    private InputAction m_InventoryAction;

    public bool moveActionEnabled => m_MoveAction.enabled;
    public bool lookActionEnabled => m_LookAction.enabled;
    public bool contextualActionEnabled => m_ContextualAction.enabled;
    public bool attackActionEnabled => m_AttackAction.enabled;
    public bool jumpActionEnabled => m_JumpAction.enabled;
    [System.NonSerialized] public bool damageRecibeActionEnabled;

    // Start is called before the first frame update
    void Awake()
    {
        //Inicializacion
        previousState = PlayerStates.NONE;
        currentState = PlayerStates.NONE;

        //Subcontroladores refe
        movementController = GetComponent<PlayerMovementController>();

        inputController = GetComponent<PlayerInput>();
        m_MoveAction = inputController.actions["Move"];
        m_LookAction = inputController.actions["Look"];
        m_ContextualAction = inputController.actions["ContextualAction"];
        m_AttackAction = inputController.actions["Fire"];
        m_JumpAction = inputController.actions["Jump"];

        m_MenuAction = inputController.actions["MainMenu"];
        m_PauseAction = inputController.actions["Pause"];
        m_ShowHUDAction = inputController.actions["ShowHUD"];
        m_InventoryAction = inputController.actions["Inventory"];

        UpdateState();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    //Actualización manual de estado
    public void UpdateState(PlayerStates nextState)
    {
        if (nextState == currentState) return;

        previousState = currentState;
        currentState = nextState;
        UpdateEnableActions();

        //Aquí se podría mandar una orden al AnimatorController de cambiar la animación
    }

    //Actualización automatica de estado
    private void UpdateState()
    {
        switch (currentState)
        {
            case PlayerStates.MOVE:
            case PlayerStates.ATTACK:
            case PlayerStates.ONDAMAGE:
            case PlayerStates.NONE:
            case PlayerStates.DISABLED:
                //NO SE PERMITE EL CAMBIO AUTOMATICO DESDE ESTOS ESTADOS
                break;  
        }
    }

    private void UpdateEnableActions()
    {
        switch (currentState)
        {
            case PlayerStates.MOVE:
                m_MoveAction.Enable();
                m_JumpAction.Enable();
                m_LookAction.Enable();
                m_ContextualAction.Enable();
                m_AttackAction.Enable();
                damageRecibeActionEnabled = true;

                m_MenuAction.Disable();
                m_PauseAction.Enable();
                m_ShowHUDAction.Enable();
                m_InventoryAction.Enable();
                break;
            case PlayerStates.ATTACK:
            case PlayerStates.ONDAMAGE:
                m_MoveAction.Enable();
                m_LookAction.Enable();
                m_JumpAction.Enable();

                m_ContextualAction.Disable();
                m_AttackAction.Disable();
                damageRecibeActionEnabled = false;

                m_MenuAction.Disable();
                m_PauseAction.Enable();
                m_ShowHUDAction.Enable();
                m_InventoryAction.Disable();
                break;
            case PlayerStates.NONE:
            case PlayerStates.DISABLED:
                m_MoveAction.Disable();
                m_LookAction.Disable();
                m_JumpAction.Disable();
                m_ContextualAction.Disable();
                m_AttackAction.Disable();
                damageRecibeActionEnabled = false;

                m_MenuAction.Enable();
                m_PauseAction.Enable();
                m_ShowHUDAction.Disable();
                m_InventoryAction.Disable();
                break;
        }
    }
}
