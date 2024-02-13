using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform projectilePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform bodyRotate;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] float launchForce = 15f;
    public GameObject heldSpear;
    public Animator animator;
    bool isDead =false;
    Vector2 velocity, startMousePos, currentMousePos;

    [SerializeField] float shootDelay;
    [SerializeField] float rotateSpeed;
    [SerializeField] float timer;
    [SerializeField] bool canShoot = true;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] clips;
    [Space][Header("Power Up")]
    public GameObject hatObject;
    public GameObject shieldObject;
    public GameObject[] speedObject;

    private void Update()
    {
        if(isDead) return;
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


        if (canShoot && !GameManager.Instance.levelCelared)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lineRenderer.enabled = true;
                startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, startMousePos);
            }
            if (Input.GetMouseButton(0))
            {
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
        animator.SetTrigger("Rotate");
        Transform projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.GetComponentInChildren<Rigidbody2D>().velocity = spawnPoint.right * launchForce;
        canShoot = false;
        timer = shootDelay;
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        //CameraShake.Shake(0.1f,0.1f);
    }



    void RotateBody()
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        bodyRotate.rotation = Quaternion.AngleAxis(Mathf.Clamp(angle, -45, 45), Vector3.forward);
    }

    public void PlayerDied()
    {
        GameEvents.CallPlayerIsDeadEvent();
        lineRenderer.enabled = false;
        GameManager.Instance.gameOver = true;
        isDead = true;
    }

    public void ActivePlayerPowerUp(int itemCode)
    {
        switch (itemCode)
        {
            case 0:
                hatObject.SetActive(true);
                break;
            case 1:
                shieldObject.SetActive(true);
                break;
            case 2:
                speedObject[0].SetActive(true);
                speedObject[1].SetActive(true);
                break;
            case 3:
                hatObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}