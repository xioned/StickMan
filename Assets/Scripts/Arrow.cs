using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject bloodparticle;
    bool isAttached = false;
    Rigidbody2D rb;
    [HideInInspector] public bool isEnemyArrow = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttached) { return; }
        if(isEnemyArrow && collision.gameObject.CompareTag("Enemy")) { return; }
        if(!isEnemyArrow && collision.gameObject.CompareTag("Player")) { return; }

        collision.transform.TryGetComponent(out BodyPart bp);
        if (bp == null) { return; }

        
        GameObject particle = Instantiate(bloodparticle, transform.position, Quaternion.identity);
        particle.transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.parent = collision.transform;

        if (bp.isHead)
        {
            bp.health.DecreaseHealth(2);
        }
        else
        {
            bp.health.DecreaseHealth(1);
        }

        if (bp.health.health <= 0)
        {
            int force = 20;
            if (!isEnemyArrow)
            {
                force = -20;
            }
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.left * force;
            
        }
        Destroy(rb);
        isAttached = true;
        this.enabled = false;
    }
}
