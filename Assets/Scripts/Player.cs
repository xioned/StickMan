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
    bool isDead =false;
    Vector2 velocity, startMousePos, currentMousePos;

    [SerializeField] float shootDelay;
    [SerializeField] float timer;
    [SerializeField] bool canShoot = true;
    

    private void Update()
    {
        if (!canShoot)
        {
            if(timer >= 0) timer -= Time.deltaTime;
            else
            {
                canShoot = true;
                timer = shootDelay;
                heldSpear.SetActive(true);
            }
        }

        if(isDead) return;

        if (canShoot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lineRenderer.enabled = true;
                startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, startMousePos);
            }
            if (Input.GetMouseButton(0))
            {
                //if(!heldSpear.activeSelf) { heldSpear.SetActive(true); }
                currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                velocity = (startMousePos - currentMousePos) * launchForce;
                RotateBody();
                lineRenderer.SetPosition(1, currentMousePos);

            }
            if (Input.GetMouseButtonUp(0))
            {

                lineRenderer.enabled = false;
                FireProjectile();
            }
        }
        
    }

    private void FireProjectile()
    {
        Debug.Log("Shooting");

        heldSpear.SetActive(false);
        Transform projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.GetComponentInChildren<Rigidbody2D>().velocity = spawnPoint.right * launchForce;
        canShoot = false;
        timer = shootDelay;
    }



    void RotateBody()
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        bodyRotate.rotation = Quaternion.AngleAxis(Mathf.Clamp(angle, -40, 40), Vector3.forward);
    }

    public void PlayerDied()
    {
        lineRenderer.enabled = false;

        isDead = true;
    }
}