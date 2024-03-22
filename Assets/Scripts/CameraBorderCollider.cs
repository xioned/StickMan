using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorderCollider : MonoBehaviour
{
    private Camera cam;
    private EdgeCollider2D edge;
    private Vector2[] edgePoints;

    void Awake()
    {
        cam = Camera.main;
        edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();
        edgePoints = new Vector2[5];
        AddCollider();
    }

    void AddCollider()
    {
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector2 topLeft = new(bottomLeft.x, topRight.y);
        Vector2 bottomRight = new(topRight.x, bottomLeft.y);

        edgePoints[0] = bottomLeft;
        edgePoints[1] = topLeft;
        edgePoints[2] = topRight;
        edgePoints[3] = bottomRight;
        edgePoints[4] = bottomLeft;

        edge.points = edgePoints;
    }
}
