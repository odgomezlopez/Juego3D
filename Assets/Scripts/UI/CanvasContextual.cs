using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasContextual : MonoBehaviour
{

    private float colliderSizeY;

    GameObject target;
    RectTransform canvasRect;
    RectTransform lifeBarRect;
    private void Start()
    {
        target = gameObject.transform.parent.gameObject;
        canvasRect = GetComponent<RectTransform>();
        lifeBarRect = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();


        colliderSizeY = target.GetComponentInChildren<Collider>().bounds.size.y;//g.transform.position
    }


    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    /*
        // Offset position above object bbox (in world space)
        float offsetPosY = target.transform.position.y + colliderSizeY;

        // Final position of marker above GO in world space
        Vector3 offsetPos = new Vector3(target.transform.position.x, offsetPosY, target.transform.position.z);

        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);

        // Set
        lifeBarRect.localPosition = canvasPos;*/
}
