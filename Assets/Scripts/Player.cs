using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform projectilePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform bodyRotate;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] float launchForce = 15f;
    public GameObject heldSpear;
    public Transform spinArm;

    Vector2 velocity, startMousePos, currentMousePos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            if(!heldSpear.activeSelf) { heldSpear.SetActive(true); }
            currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            velocity = (startMousePos - currentMousePos) * launchForce;
            RotateBody();
        }
        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.positionCount = 0;
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        StartCoroutine(FireProjectileRoutine());
    }

    private IEnumerator FireProjectileRoutine()
    {
        heldSpear.SetActive(false);
        Transform projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.GetComponentInChildren<Rigidbody2D>().velocity = spawnPoint.right * launchForce;
        yield return new WaitForSeconds(2);
    }

    void RotateBody()
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        bodyRotate.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}