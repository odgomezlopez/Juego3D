using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision2D : MonoBehaviour
{
    public bool visible;
    public Vector3 lastPlayerPos;

    public float viewDistanceX;
    public float viewDistanceY;

    public float detectRadius;

    public bool invertAxis;
    private int invertAxisInt;
    GameObject player;

  

    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        invertAxisInt = (invertAxis) ? 1 : -1;
        player = GameObject.FindGameObjectWithTag("Player");
        lastPlayerPos = Vector3.zero;
    }

    private void Update()
    {
        invertAxisInt = (invertAxis) ? 1 : -1;
    }
    // Update is called once per frame
    void FixedUpdate()
    {


        float angulo = (detectRadius) / 2;
        Vector3 punto1 = new Vector3(invertAxisInt * viewDistanceX, PosToAnguloY(angulo) * viewDistanceY, 0);
        Vector3 punto2 = new Vector3(invertAxisInt * viewDistanceX, PosToAnguloY(-angulo) * viewDistanceY, 0);



        Vector3 punto3 = new Vector3(invertAxisInt * PosToAnguloX(angulo) * viewDistanceX, viewDistanceY, 0);
        Vector3 punto4 = new Vector3(invertAxisInt * PosToAnguloX(-angulo) * viewDistanceX, -viewDistanceY, 0);


        if (detectRadius > 90)
        {
            punto1.y = viewDistanceY;
            punto2.y = -viewDistanceY;
        }

        punto1 = transform.localToWorldMatrix.MultiplyPoint(punto1);
        punto2 = transform.localToWorldMatrix.MultiplyPoint(punto2);
        punto3 = transform.localToWorldMatrix.MultiplyPoint(punto3);
        punto4 = transform.localToWorldMatrix.MultiplyPoint(punto4);

        Vector3[] posicionesX = { player.transform.position, new Vector3(player.transform.position.x, player.GetComponentInChildren<Collider2D>().bounds.center.y + player.GetComponentInChildren<Collider2D>().bounds.extents.y, player.transform.position.z), new Vector3(player.transform.position.x, player.GetComponentInChildren<Collider2D>().bounds.center.y - player.GetComponentInChildren<Collider2D>().bounds.extents.y, player.transform.position.z) };

        foreach (Vector3 destino in posicionesX)
        {
            if (
                PointInsideTrigon(destino, gameObject.transform.position, punto1, punto2) || 
                (detectRadius > 90 && (PointInsideTrigon(destino, gameObject.transform.position, punto1, punto3) || PointInsideTrigon(destino, gameObject.transform.position, punto2, punto4))) ||
                (detectRadius > 270 && PointInsideTrigon(destino, gameObject.transform.position, punto3, punto4))
                )
                
            {
                //ESTA EN EL CAMPO DE VISION. FALTA COMPROBAR SI HAY OBSTACULOS
                //Unity 4.6.1 has the option turn on/off the ability to detect a collider that overlaps the start of any 2D line/raycast in Edit -> Project Settings -> Physcis 2D -> Raycasts Start In Colliders.

                RaycastHit2D toca = Physics2D.Raycast(gameObject.transform.position, destino - gameObject.transform.position);

                if (toca.collider?.CompareTag("Player") ?? false)
                {
                    Debug.DrawRay(gameObject.transform.position, destino - gameObject.transform.position, Color.red);
                    visible = true;
                    lastPlayerPos = player.transform.position;
                    return;
                }
                else
                {
                    Debug.DrawRay(gameObject.transform.position, destino - gameObject.transform.position, Color.yellow);
                }
            }
        }
        visible = false;
        return;
       
    }
    //Matematical operations
    bool PointInsideTrigon(Vector3 s, Vector3 a, Vector3 b, Vector3 c)
    {
        float as_x = s.x - a.x;
        float as_y = s.y - a.y;

        bool s_ab = (b.x - a.x) * as_y - (b.y - a.y) * as_x > 0;

        if ((c.x - a.x) * as_y - (c.y - a.y) * as_x > 0 == s_ab)
            return false;
        if ((c.x - b.x) * (s.y - b.y) - (c.y - b.y) * (s.x - b.x) > 0 != s_ab)
            return false;
        return true;
    }
    public static double ConvertDegreesToRadians(double degrees)
    {
        double radians = (System.Math.PI / 180) * degrees;
        return (radians);
    }

    public static double ConvertRadiansToDegrees(double radians)
    {
        double degrees = (180 / System.Math.PI) * radians;
        return (degrees);
    }

    private float PosToAnguloX(float angulo) {
        return ((float) System.Math.Cos(ConvertDegreesToRadians(angulo)));
    }

    private float PosToAnguloY(float angulo)
    {
        return (float)System.Math.Sin(ConvertDegreesToRadians(angulo));
    }

    private void OnDrawGizmos()
    {

        float invertirGizmo = (invertAxis) ? 1 : -1;

        //Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        //Gizmos.matrix = rotationMatrix;
        Gizmos.color = visible ? Color.red : Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.DrawFrustum(transform.localPosition, detectRadius,viewDistance,0,1);

        float angulo = (detectRadius)/2;
        Vector3 punto1= new Vector3(invertirGizmo * viewDistanceX, PosToAnguloY(angulo) * viewDistanceY, 0);
        Vector3 punto2 = new Vector3(invertirGizmo * viewDistanceX, PosToAnguloY(-angulo) * viewDistanceY, 0);
        if (detectRadius > 90)
        {
            punto1.y = viewDistanceY;
            punto2.y = -viewDistanceY;
        }

        Gizmos.DrawLine(Vector3.zero, punto1);
        Gizmos.DrawLine(Vector3.zero, punto2);
        Gizmos.DrawLine(punto1, punto2);

        //Permitir angulos de mas de 90 grados
        if (detectRadius > 90)
        {
            Gizmos.DrawLine(Vector3.zero, punto1);
            Gizmos.DrawLine(Vector3.zero, punto2);
            Gizmos.DrawLine(punto1, punto2);

            Vector3 punto3 = new Vector3(invertirGizmo * PosToAnguloX(angulo) * viewDistanceX, viewDistanceY, 0);
            Gizmos.DrawLine(Vector3.zero, punto1);
            Gizmos.DrawLine(Vector3.zero, punto3);
            Gizmos.DrawLine(punto1, punto3);

            Vector3 punto4 = new Vector3(invertirGizmo * PosToAnguloX(-angulo) * viewDistanceX, -viewDistanceY, 0);
            Gizmos.DrawLine(Vector3.zero, punto2);
            Gizmos.DrawLine(Vector3.zero, punto4);
            Gizmos.DrawLine(punto2, punto4);

            if(detectRadius > 270) Gizmos.DrawLine(punto3, punto4);

        }

    }
}
