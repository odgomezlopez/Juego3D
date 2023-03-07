using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public static class Utils
    {
        public static bool IsGrounded(GameObject g, float dist= 0.4f)
        {
            Vector3 centro = g.GetComponent<Collider2D>().bounds.center;//g.transform.position

            //Comprueba si toca el centro
            float[] positionsX = { 0, g.GetComponent<Collider2D>().bounds.extents.x , -g.GetComponent<Collider2D>().bounds.extents.x };
            foreach (float x in positionsX)
            {
                Vector3 downPoint = new Vector3(x, -(g.GetComponent<Collider2D>().bounds.extents.y + 0.0001f));
                RaycastHit2D isTouchingTheGround = Physics2D.Raycast(centro + downPoint, Vector2.down, dist);

                Debug.DrawRay(centro + downPoint, Vector2.down * dist, Color.red);

                if (isTouchingTheGround.collider?.CompareTag("Floor") ?? false)
                {
                    Debug.DrawRay(centro + downPoint, Vector2.down * dist, Color.red);
                    return true;
                }
                else {
                    Debug.DrawRay(centro + downPoint, Vector2.down * dist, Color.yellow);
                }
            }

            return false;
        }
    }
}
