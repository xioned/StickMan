using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorderCollider : MonoBehaviour
{
    private Camera cam;
    public EdgeCollider2D[] borders = new EdgeCollider2D[4];

    void Awake()
    {
        cam = Camera.main;
        AddCollider();
    }
    private void Start()
    {
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].enabled = true;
        }
    }

    void AddCollider()
    {
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector2 topLeft = new(bottomLeft.x, topRight.y);
        Vector2 bottomRight = new(topRight.x, bottomLeft.y);

        borders[0].points = new Vector2[2] { bottomLeft, topLeft };
        borders[1].points = new Vector2[2] { topLeft, topRight }; ;
        borders[2].points = new Vector2[2] { topRight, bottomRight }; ;
        borders[3].points = new Vector2[2] { bottomRight, bottomLeft }; ;
    }
}
