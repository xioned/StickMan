using System.Collections;
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
    public Animator animator;

    private void OnEnable()
    {
        GameEvents.OnPlayerIsDeadEvent += PlayerDied;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerIsDeadEvent -= PlayerDied;

    }

    private void PlayerDied()=> isDead = true;

    private void Start()
    {
        StartCoroutine(FireProjectileRoutine());
    }


    private IEnumerator FireProjectileRoutine()
    {
        while (!isDead&& !GameManager.Instance.gameOver)
        {
            heldSpear.SetActive(false);

            yield return new WaitForSeconds(.5f);
            heldSpear.SetActive(true);

            yield return new WaitForSeconds(waitTime);
            
            FireProjectile();
        }
    }
    public void Dead()
    {
        isDead = true;
        StartCoroutine(DeadRoutine());
    }

    IEnumerator DeadRoutine()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.IncreaseKillCount();
        GameManager.Instance.SpawnNewEnemy();
        Destroy(this.gameObject);
    }

    private void FireProjectile()
    {
        if(isDead) { return; } 
        int randf = Random.Range((int)rotateClampPoint.x, (int)rotateClampPoint.y);
        rotatePoint.rotation = Quaternion.Euler(0,0,randf);
        animator.SetTrigger("Rotate");
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Arrow>().isEnemyArrow = true;
        projectile.GetComponentInChildren<Rigidbody2D>().velocity = -spawnPoint.right * launchForce;
    }
}
