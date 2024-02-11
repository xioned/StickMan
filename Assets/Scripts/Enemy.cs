using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform rotatePoint;
    public Transform spawnPoint;
    public float launchForce = 15;
    public float waitTime;
    public Vector2 rotateClampPoint;
    public GameObject heldSpear;
    public Transform spinArm;
    public bool isDead = false;

    private void Start()
    {
        StartCoroutine(FireProjectileRoutine());
    }

    private IEnumerator FireProjectileRoutine()
    {
        while (!isDead)
        {
            heldSpear.SetActive(false);

            yield return new WaitForSeconds(waitTime);
            
            heldSpear.SetActive(true);
            FireProjectile();
        }
    }
    public void Dead()
    {
        isDead = true;
        Destroy(this.gameObject,3);
        
    }
    private void OnDestroy()
    {
        if(isDead)
        GameManager.Instance.SpawnNewEnemy();
    }
    private void FireProjectile()
    {
        if(isDead) { return; } 
        int randf = Random.Range((int)rotateClampPoint.x, (int)rotateClampPoint.y);
        rotatePoint.rotation = Quaternion.Euler(0,0,randf);

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Arrow>().isEnemyArrow = true;
        projectile.GetComponentInChildren<Rigidbody2D>().velocity = -spawnPoint.right * launchForce;
    }
}
