using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject bloodparticle;
    bool isAttached = false;
    Rigidbody2D rb;
    [HideInInspector] public bool isEnemyArrow = false;
    public AudioSource source;
    public AudioClip[] hit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttached) { return; }
        if(isEnemyArrow && collision.gameObject.CompareTag("Enemy")) { return; }
        if(!isEnemyArrow && collision.gameObject.CompareTag("Player")) { return; }

        collision.transform.TryGetComponent(out BodyPart bp);
        if (bp == null) { return; }

        if(!isEnemyArrow && bp.gameObject.CompareTag("Enemy")) 
        {
            CameraShake.Shake(0.1f, 0.1f);
            source.PlayOneShot(hit[0]);
        }

        GameObject particle = Instantiate(bloodparticle, transform.position, Quaternion.identity);
        particle.transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.parent = collision.transform;

        bp.health.DecreaseHealth(bp.damageAmount);
        if (!isEnemyArrow)
        {
            GameEvents.CallEnemyDamageUiEvent(bp.damageAmount, transform.position);
        }
        
        if (bp.health.health <= 0)
        {
            int force = !isEnemyArrow ? -20 : 20;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.left * force;
        }
        Destroy(rb);
        isAttached = true;
        this.enabled = false;
    }
}
