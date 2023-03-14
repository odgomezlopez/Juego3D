using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraJugador : MonoBehaviour
{
    private Transform jugadorTransform;
    private Vector3 ultimaPosicion;

    public float velocidadX;
    public float velocidadZ;

    void Start()
    {
        jugadorTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ultimaPosicion = jugadorTransform.position;
    }
    void FixedUpdate()
    {
        Vector3 movimiento = jugadorTransform.position - ultimaPosicion;

        transform.position = transform.position + new Vector3(movimiento.x*velocidadX, 0, movimiento.z * velocidadZ);

        ultimaPosicion = jugadorTransform.position;
    }
}
