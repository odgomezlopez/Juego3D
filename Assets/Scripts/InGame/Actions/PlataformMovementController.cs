using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Si se cambia de heredar de PlayerAction a AutoAction, la acción de Execute se ejecutará al cargar la escena. También se podría heredar solo de Action y activar la acción manualmente desde codigo llamando a Execute
public class PlataformMovementController : ContextualActionController
{
    [Header("PlataformMovement Parameters")]
    //Velocidad
    public float speed=5;
    public bool loop=false;


    //[Header("PlataformMovement Debug")]
    //Patrulla lista posiciones
    private Vector3 initialPosition;

    private List<Vector3> positions;
    private int towardsPosition;

    private bool moving;

    // Start is called before the first frame update
    protected override void Start()
    {
        actionName = "Activar";

        positions=new List<Vector3>();

        GameObject patrulla = gameObject.transform.Find("Patrulla").gameObject;
        for (int i = 0; i < patrulla.transform.childCount; i++)
        {
            positions.Add(patrulla.transform.GetChild(i).position);
        }

        //Initial position
        initialPosition = gameObject.transform.position;//Vector3.zero;
        positions.Add(initialPosition);
        moving = false;

        //Requerido para que funcionen todos los parametros
        base.Start();
    }

    //Sobreescribimos la accion que se ejecuta
    public override IEnumerator ExecuteAction()
    {
        if (CheckRequirement())
        {
            //Codigo 
            yield return StartCoroutine(MoveToNextPosition());
            yield return true;
        }

        //Requerido para que funcionen todos los parametros
        yield return base.ExecuteAction();
    }

    public override bool CheckRequirement()
    {
        //Requerido que tenga && base.CheckRequirement(); para que funcione el limite de acciones generico

        //Si se quiere limitar el numero de acciones poner 
        return !moving && base.CheckRequirement();
    }

    //Moverse hacia la siguiente posicion
    private IEnumerator MoveToNextPosition()
    {
        moving = true;

        do
        {
            while (Vector3.Distance(transform.position, positions[towardsPosition]) > 0.1)
            {
                transform.position = Vector3.MoveTowards(transform.position, positions[towardsPosition], speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSecondsRealtime(0.5f);
            towardsPosition = (towardsPosition + 1) % positions.Count;
        } while (loop);

        moving = false;
    }
}