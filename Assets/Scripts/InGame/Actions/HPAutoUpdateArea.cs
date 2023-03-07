using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaType
{
    POISON = -1,
    HEAL = 1
}

public class HPAutoUpdateArea : MonoBehaviour
{
    public AreaType areaType = AreaType.POISON;
    
    [Range(0,10)]
    public float autoChangeValuePerSecond = 1;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            IGenericController controlador = collision.gameObject.GetComponent<IGenericController>();
            controlador.GetStats().HP.autoUpdateRate += autoChangeValuePerSecond * (int) areaType;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            IGenericController controlador = collision.gameObject.GetComponent<IGenericController>();
            controlador.GetStats().HP.autoUpdateRate -= autoChangeValuePerSecond * (int)areaType;


        }
    }
}

