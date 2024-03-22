using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Arrow : MonoBehaviour
{
    public GameObject bloodparticle;
    bool isAttached = false;
    Rigidbody2D rb;
    [HideInInspector] public bool isEnemyArrow = false;
    [HideInInspector] public bool isBossArrow = false;
    public AudioSource source;
    public AudioClip[] hit;

    public LayerMask borderLayerMask;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, borderLayerMask);
        //Debug.DrawRay(transform.position, transform.right, Color.red, 50f);
        if (hit.collider)
        {
            BounceArrow(hit.collider);
        }

        if (!GetComponent<Renderer>().isVisible)
        {
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBossArrow && isEnemyArrow && collision.gameObject.GetComponent<CameraBorderCollider>())
        {
            BounceArrow(collision);
            return;
        }

        if (isAttached) { return; }
        if (isEnemyArrow && collision.gameObject.CompareTag("Enemy")) { return; }
        if (!isEnemyArrow && collision.gameObject.CompareTag("Player")) { return; }

        collision.transform.TryGetComponent(out BodyPart bp);
        if (bp == null) { return; }

        if (!isEnemyArrow && bp.gameObject.CompareTag("Enemy"))
        {
            CameraShake.Shake(0.1f, 0.1f);
            source.PlayOneShot(hit[0]);
        }
        if (isEnemyArrow)
        {


            Debug.Log(bp.gameObject.layer);

            if (bp.gameObject.layer == 6)
            {
                if (Global.isHitActive)
                {
                    Global.isHitActive = false;
                    Destroy(gameObject);
                    for (int j = 0; j < GameManager.Instance.powerupViews[0].powerUpPrefab.Length; j++)
                    {
                        GameManager.Instance.powerupViews[0].powerUpPrefab[j].SetActive(false);
                    }
                    Debug.Log("Got free headshot");
                    return;
                }

            }
            else if (bp.gameObject.layer == 7)
            {
                if (Global.isBodyActive)
                {
                    Global.totalBodyCount++;

                    if (Global.totalBodyCount >= 3)
                    {
                        Global.isBodyActive = false;
                        Global.totalBodyCount = 0;
                    }
                    Destroy(gameObject);
                    for (int j = 0; j < GameManager.Instance.powerupViews[1].powerUpPrefab.Length; j++)
                    {
                        GameManager.Instance.powerupViews[1].powerUpPrefab[j].SetActive(false);
                    }
                    Debug.Log("Got free bodyshot");
                    return;
                }
            }
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

    private void BounceArrow(Collider2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);
        Debug.Log(contacts[0].normal);

        Vector2 direction = Vector3.Reflect(rb.velocity.normalized, contacts[0].normal);
        rb.velocity = direction * Mathf.Max(rb.velocity.magnitude, 15f);
    }
}
