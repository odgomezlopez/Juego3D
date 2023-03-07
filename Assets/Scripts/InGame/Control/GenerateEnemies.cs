using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> prefabs;
    public int numberEnemies = 40;

    public int minPointValue = 1;
    public int maxPointValue = 3;
    public int HPmultiplier = 5;

    public Vector3 minPos;
    public Vector3 maxPos;
    public Vector3 patrolMaxDist;

    public float minSpeed = 5;
    public float maxSpeed = 15;

    public Vector3 minRot;
    public Vector3 maxRot;

    void Start()
    {
        GameObject parent = GameObject.Find("====ENEMIGOS====");

        for (int i=0;i< numberEnemies; i++)
        {
            //Genero aleatoriamente los datos de la mesh
            int nMesh = Random.Range(0, prefabs.Count-1);
            int nPoints= Random.Range(minPointValue, maxPointValue+1);//1, default color, 2 yellow, 3 red. Los puntos es igual al ataque.

            float nSpeed = Random.Range(minSpeed, maxSpeed);
            Vector3 nRotSpeed = new Vector3(Random.Range(minRot.x, maxRot.x), Random.Range(minRot.y, maxRot.y), Random.Range(minRot.z, maxRot.z));

            //Obtengo las posiciones
            Vector3 nPos1 = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), Random.Range(minPos.z, maxPos.z));

            Vector3 nPos2= new Vector3(Random.Range(-patrolMaxDist.x, patrolMaxDist.x), Random.Range(-patrolMaxDist.y, patrolMaxDist.y), Random.Range(-patrolMaxDist.z, patrolMaxDist.z));

            //Instancio el gameObject
            GameObject nuevo =Instantiate(prefabs[nMesh], nPos1, Quaternion.identity, parent.transform);

            //actualizamos la patrulla
            nuevo.transform.Find("Patrulla").GetChild(0).localPosition = Vector3.zero;//posicion 1 de patrulla
            nuevo.transform.Find("Patrulla").GetChild(1).localPosition = nPos2;

            //Check if the position is in range
            Vector3 postTemp = nuevo.transform.Find("Patrulla").GetChild(1).position;
            nuevo.transform.Find("Patrulla").GetChild(1).position=new Vector3(
                System.Math.Clamp(postTemp.x, minPos.x, maxPos.x), 
                System.Math.Clamp(postTemp.y, minPos.y, maxPos.y), 
                System.Math.Clamp(postTemp.z, minPos.z, maxPos.z));



            nuevo.GetComponent<EnemyPatrolController>().GeneratePositions();
            //nuevo.transform.worldToLocalMatrix;

            //Modifico el gameobject
            EnemyStats stats = nuevo.GetComponent<EnemyStats>();
            stats.speedForce = nPoints * nSpeed;
            stats.rotationPerSecond = nRotSpeed;

            stats.HP.maxValue = nPoints* HPmultiplier;
            stats.HP.initValue = stats.HP.maxValue;
            stats.HP.CurrentValue = stats.HP.maxValue;

            stats.points = nPoints;
            stats.attackDamage = nPoints;

            //Asigno color segun puntos
            if(nPoints > 1)
            {
                MeshRenderer mesh = nuevo.GetComponentInChildren<MeshRenderer>();

                for (int j = 0; j < mesh.materials.Length; j++)
                {
                    if (nPoints == 2)
                    {
                        mesh.materials[j].color = Color.yellow;
                    }
                    else {
                        mesh.materials[j].color = Color.red;
                    }
                }
            }
        }
    }

}
