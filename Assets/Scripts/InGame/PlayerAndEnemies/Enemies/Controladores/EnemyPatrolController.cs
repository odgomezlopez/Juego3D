using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    EnemyStats stats;

    //Espera
    public int waitSeconds;

    //Movimiento

    //Modo patrulla
    public enum PatrolsModes
    {
        GameObjects,
        Positions
    };

    public PatrolsModes mode;

    //Patrulla general

    private Vector3 initialPosition;
    public int towardsPosition;

    //Patrulla lista posiciones
    public List<Vector3> positions;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        //Movimiento

        //Patrulla general
        towardsPosition = 0;

        GeneratePositions();

        StartCoroutine(Patrol());
    }

    public void GeneratePositions()
    {
        positions.Clear();

        if (mode == PatrolsModes.Positions)//Patrulla lista posiciones
        {
            initialPosition = transform.position;
            positions.Add(transform.position);
        }
        else
        {//Patrulla gameObjects    
            initialPosition = Vector3.zero;
            GameObject patrulla = gameObject.transform.Find("Patrulla").gameObject;
            for (int i = 0; i < patrulla.transform.childCount; i++)
            {
                positions.Add(patrulla.transform.GetChild(i).position);
            }
        }
    }

    private IEnumerator Patrol()
    {


        while (Vector3.Distance(transform.position, initialPosition + positions[towardsPosition]) > 0.2)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + positions[towardsPosition], stats.speedForce * Time.deltaTime);

            transform.Rotate(new Vector3(stats.rotationPerSecond.x * Time.deltaTime,0));
            yield return new WaitForEndOfFrame();
        }
         
        towardsPosition = (towardsPosition + 1) % positions.Count;
        
        yield return StartCoroutine(Wait());
        yield return StartCoroutine(Patrol());
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(waitSeconds);
        //yield return StartCoroutine(Patrulla());
    }



    //Metodos de ayuda

}
