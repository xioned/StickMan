using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float waitTime;
    public float launchForce;
    public Vector2 rotateClampPoint;

    public GameObject bossArrowPrefab;
    public Transform aimRotate;
    public Transform projectileSpawnPos;

    bool isPlayerDead = false;
    
    private void OnEnable()
    {
        GameEvents.OnPlayerIsDeadEvent += PlayerDied;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerIsDeadEvent -= PlayerDied;
    }
    private void PlayerDied() => isPlayerDead = true;
    private void Start() => StartCoroutine(FireProjectileRoutine());


    private IEnumerator FireProjectileRoutine()
    {
        while (!isPlayerDead)
        {
            yield return new WaitForSeconds(waitTime);
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        if (isPlayerDead) { return; }
        int randf = Random.Range((int)rotateClampPoint.x, (int)rotateClampPoint.y);
        aimRotate.rotation = Quaternion.Euler(0, 0, randf);
        GameObject projectile = Instantiate(bossArrowPrefab, projectileSpawnPos.position, Quaternion.identity);
        projectile.GetComponent<Arrow>().isEnemyArrow = true;
        projectile.GetComponent<Arrow>().isBossArrow = true;
        projectile.GetComponentInChildren<Rigidbody2D>().velocity = -projectileSpawnPos.right * launchForce;
    }
}
