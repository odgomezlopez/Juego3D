using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public PlayerStats stats;
    protected PlayerStateController stateController;

    protected bool moveEnable = true;

    //INPUT READER
    protected Vector3 moveInput;
    protected Vector2 turningInput;

    //Gestion Saltos
    protected int numSaltos = 0;
    public bool inGround;

    public GameObject CameraLookAtPoint;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        moveEnable = true;

        stateController = GetComponent<PlayerStateController>();
        stats = GameObject.FindGameObjectWithTag("GameData").GetComponent<PlayerStats>();

        numSaltos = 0;

        CameraLookAtPoint = transform.Find("CameraLookAtPoint").gameObject;
    }

    protected virtual void Update()
    {
        TurningUpdate();

    }
    private void TurningUpdate()
    {
        //Saco el componente de rotación der/izq. GIRANDO EL PERSONAJE
        /*Vector3 girarY = Vector3.zero;
        girarY.y = turningInput.x;
        transform.Rotate(girarY * stats.turnHorizontalSpeed * Time.deltaTime);*/

        //Saco el componente de rotacion arriba/abajo. SUBIENDO BAJANDO EL PUNTO AL QUE MIRA CINEMACHINE
        /*float lookX = 0; //turningInput.x*10;

        float lookY = Mathf.Clamp(CameraLookAtPoint.transform.localPosition.y + turningInput.y * stats.turnVerticalSpeed, stats.minVerticalPos, stats.maxVerticalPos);
        CameraLookAtPoint.transform.localPosition = new Vector3(lookX, lookY, CameraLookAtPoint.transform.localPosition.z);*/
    }

    //Activar desactivar movimiento
    public void EnableMovement()
    {
        moveEnable = true;
    }
    public void DisableMovement()
    {
        moveEnable = false;
    }

    // Update is called once per frame
    public virtual void Move(Vector2 input)
    {
        if (!moveEnable) return;
        moveInput.x = input.x ;
        moveInput.z = input.y;
    }
    public virtual void Turn(Vector2 input)
    {
        this.turningInput = input.normalized;
    }
    public bool Jump()
    {
        if (!moveEnable) return false;
        if (numSaltos < stats.maxJump)
        {
            //_velocity.y += stats.jumpForce;
            StartCoroutine(JumpPhysics());
            numSaltos++;
            return true;
        }
        return false;
    }


    public virtual void KnockBack(Vector3 direction, float forceKnockBack, float timeKnockBack)
    {
        if (!moveEnable) return; //No se permiten dos KnockBack simultaneos
        StartCoroutine(KnockBackPhysics(direction, forceKnockBack, timeKnockBack));
        this.moveInput = Vector3.zero;
    }

    //Definiciones genericas de los movimientos con fisicas
    protected virtual IEnumerator JumpPhysics()
    {
        yield return null;
    }
    protected virtual IEnumerator KnockBackPhysics(Vector3 direction, float forceKnockBack, float timeKnockBack)
    {
        yield return null;
    }
}
