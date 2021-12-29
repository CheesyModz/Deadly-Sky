using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;

    float velocity = 10f;

    Vector2 currentDirection;

    public Transform shadow;

    public Transform gunPoint;

    public Bullet bullet;

    public GameController gameController;

    private int health;

    private const int MaxHealth = 5;

    public TMP_Text healthText;

    private int ammo;

    private const int MaxAmmo = 20;

    public TMP_Text ammoText;

    public int score;

    public TMP_Text scoreText;

    public CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        ammo = MaxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        shadow.position = (Vector2)transform.position + new Vector2(-1f, -1.5f);
        Fly();
        LookAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        ammoText.text = "Ammo: " + ammo.ToString();
        healthText.text = "Health: " + health.ToString();
        scoreText.text = "Score: " + score.ToString();
    }

    void Fly()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        currentDirection = new Vector2(horizontal, vertical);

        rigidbody2D.MovePosition(rigidbody2D.position + currentDirection * velocity * Time.deltaTime);
    }

    void LookAtMouse()
    {
        if (gameController.isPaused) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = (mousePosition - (Vector2) transform.position).normalized;

        transform.up = lookDirection;
    }

    void Shoot()
    {
        if (ammo <= 0) return;

        Instantiate(bullet, gunPoint.position, transform.rotation).currentDirection = transform.up;
        ammo--;

        cameraShake.Shake(0.05f, 0.1f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();

        cameraShake.Shake(0.1f, 0.1f);
    }

    void Die()
    {
        gameController.GameOver();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoCollectible"))
        {
            ammo += 5;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HealthCollectible"))
        {
            health++;
            Destroy(other.gameObject);
        }
    }
}
