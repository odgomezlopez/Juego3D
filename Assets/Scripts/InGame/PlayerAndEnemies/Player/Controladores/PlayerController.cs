using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using static Utils.Utils;

public class PlayerController : MonoBehaviour, IGenericController, IReadied
{
    //Stats
    private PlayerStats stats;
    private MeshRenderer mesh;

    //Controlador de estados
    private PlayerInit init;
    private PlayerStateController stateController;

    //Subcontroladores
    private PlayerFireAttackController attackController;
    private PlayerContextualActionController contextualActionController;
    private GenericSoundController soundController;
    private PlayerMovementController movementController;
    private PlayerInGameUIController inGameUIController;

    //Gesionador del nivel
    LevelControllerBase control;

    private bool ready = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //TODO ESPERAR AL INIT
        ready = false;
        
        init = GetComponent<PlayerInit>();
        yield return new WaitUntil(() => (init.IsReady()));

        //Controlador del nivel
        control = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelControllerBase>();

        if (control == null)
        {
            Debug.LogError("ERROR: Debes tener un Controlador de Nivel en la escena");
        }

        mesh = GetComponentInChildren<MeshRenderer>();

        //Inicializamos el estado
        stats = GameObject.FindGameObjectWithTag("GameData").GetComponent<PlayerStats>();

        //Subonctorladores
        stateController = GetComponent<PlayerStateController>();
        stateController.UpdateState(PlayerStates.MOVE);
        //DisableGamePlay(); //Desactivamos el gameplay.Sera activado desde el controlador de nivel


        movementController = GetComponent<PlayerMovementController>();
        attackController = GetComponent<PlayerFireAttackController>();
        contextualActionController = GetComponent<PlayerContextualActionController>();
        soundController = GetComponent<GenericSoundController>();
        inGameUIController = GetComponent<PlayerInGameUIController>();

        stats.HP.OnIndicatorChange += OnHPUpdate;
        //yield return new WaitForSecondsRealtime(1f);
        ready = true;
    }

    public bool IsReady()
    {
        return ready;
    }

    //Gestion de Stats
    public Stats GetStats()
    {
        return stats;
    }

    //Gestion de gameover mediante eventos
    public virtual void OnHPUpdate(float val) {
        //Compruebo si ha terminado el juego
        if (stats.HP.CurrentValue <= 0)
        {
            StartCoroutine(control.LevelGameOver());
        }
    }

    private void OnDestroy()
    {
        stats.HP.OnIndicatorChange -= OnHPUpdate;//HAY QUE DESTRUIR EL OBJETO
    }

    //Metodos para la comunicación desde el exterior con PlayerController
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsReady()) return;

        Vector2 movementInput;
        //Compruebo si puedo moverme y le doy la orden de mover al subocntorlador
        if (stateController.moveActionEnabled)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        else
        {
            movementInput = Vector2.zero;
        }
        movementController.Move(movementInput);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!IsReady()) return;
        if (!stateController.lookActionEnabled) return;
        movementController.Turn(context.ReadValue<Vector2>());
    }

    //Funciones de ataque base
    public void OnFireAttack(InputAction.CallbackContext context)
    {
        if (!ready) return;

        if (!stateController.attackActionEnabled) return;

        if (context.performed && context.action.WasPerformedThisFrame() && context.action.IsPressed())
        {
            //if (attackController.Attack())
            //    soundController.OnAttackSound();
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsReady()) return;
        if (!stateController.jumpActionEnabled) return;
        movementController.Jump();
    }
    //Funciones de acciones contextuales
    public void OnContextualAction(InputAction.CallbackContext context)
    {
        if (!IsReady()) return;
        if (!stateController.contextualActionEnabled) return;

        if (context.performed && context.action.WasPerformedThisFrame() && context.action.IsPressed())
        {
            if (contextualActionController.IsContextualActionAsigned())
            {
                contextualActionController.ExecuteContextualAction();
            }
        }
    }


    //Gestion de daño
    public void OnHealing(float heal)
    {
        stats.HP.CurrentValue += heal;
    }

    public void OnDamage(float damage, GameObject damageSource, bool knockback=true, AttackType attackType = AttackType.HIT) {
        if (!stateController.damageRecibeActionEnabled) return;
        
        if (attackType == AttackType.HIT) StartCoroutine(HitCorutine(damage,damageSource, knockback));

        //TODO decidir como reaccionar a otros tipos de daño
    }

    //TODO Move to a SubController
    private IEnumerator HitCorutine(float damage, GameObject damageSource, bool knockback = true)
    {
        if (!stats.invulnerable)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), true);

            //Activo la invulnerabilidad
            stats.invulnerable = true;
            stateController.UpdateState(PlayerStates.ONDAMAGE);

            //Guardo los colores
            List<Color> auxColor=new List<Color>();
            for(int i=0;i< mesh.materials.Length; i++)
            {
                auxColor.Add(mesh.materials[i].color);
                mesh.materials[i].color = Color.red;
            }

            //Realizo el calculo de dano
            stats.HP.CurrentValue -= damage;

            //Calculo el knockBack
            if (knockback)
            {
                Vector2 knockBackDirection = (transform.position - damageSource.transform.position).normalized;
                movementController.KnockBack(knockBackDirection, stats.forceKnockBack, stats.secondsKnockBack);
            }

            //Espero el tiempo de invulnerabildad
            yield return new WaitForSecondsRealtime(stats.secondsInvulnerable);

            //Restauro los colores
            for (int i = 0; i < mesh.materials.Length; i++)
            {
                mesh.materials[i].color = auxColor[i];
            }

            //Desactivo la invulnerabilidad
            stats.invulnerable = false;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), false);


            stateController.UpdateState(stateController.previousState);
        }
    }
 
    //Metodos para activar/desactivar el GamePlay
    public void EnableGamePlay()
    {
        stateController.UpdateState(stateController.previousState);
    }
    public void DisableGamePlay()
    {
        stateController.UpdateState(PlayerStates.DISABLED);
    }
}

