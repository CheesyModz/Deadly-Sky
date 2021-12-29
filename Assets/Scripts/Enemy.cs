using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    public SpriteRenderer shadow;

    public Sprite[] shipSprites;

    private int health;

    private Vector2Int HealthRange = new Vector2Int(2, 5);

    public Rigidbody2D rigidbody2D;

    private Vector2 targetPosition;

    private Vector2 minPosition = new Vector2(-17f, -8f);

    private Vector2 maxPosition = new Vector2(17f, 8f);

    private float velocity;

    private Vector2 VelocityRange =new Vector2(3f, 6f);

    private PlayerMovement player;

    public Bullet bullet;

    public Transform gunPoint;

    private Vector2 shootIntervalRange = new Vector2(1f, 2f);

    public GameObject ammoCollectiblePrefab;

    public GameObject healthCollectiblePrefab;

    private CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        cameraShake = FindObjectOfType<CameraShake>();
        Generate();
        RandomizePosition();

        Invoke("Shoot", Random.Range(shootIntervalRange.x, shootIntervalRange.y));
    }

    // Update is called once per frame
    void Update()
    {
        shadow.transform.position = (Vector2)transform.position + new Vector2(-1f, -2f);
        Fly();
        LookAtPlayer();
    }

    void Generate()
    {
        Sprite assignedSprite = shipSprites[Random.Range(0, shipSprites.Length)];
        SpriteRenderer.sprite = assignedSprite;
        shadow.sprite = assignedSprite;

        health = Random.Range(HealthRange.x, HealthRange.y);
        velocity = Random.Range(VelocityRange.x, VelocityRange.y);
    }

    void RandomizePosition()
    {
        targetPosition = new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y));
    }

    void Fly()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized; 
        rigidbody2D.MovePosition(rigidbody2D.position + direction * velocity * Time.deltaTime);

        if ((targetPosition - (Vector2)transform.position).magnitude < 0.1f)
        {
            RandomizePosition();
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        Vector2 lookDirection = (player.transform.position - transform.position).normalized;
        transform.up = lookDirection;

    }

    void Shoot()
    {
        Instantiate(bullet, gunPoint.position, transform.rotation).currentDirection = transform.up;

        Invoke("Shoot", Random.Range(shootIntervalRange.x, shootIntervalRange.y));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();

        cameraShake.Shake(0.1f, 0.1f);
    }

    void Die()
    {
        Instantiate(ammoCollectiblePrefab, (Vector2)transform.position + new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)), Quaternion.identity);
        Instantiate(healthCollectiblePrefab, (Vector2)transform.position + new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)), Quaternion.identity);

        player.score += 100;
        Destroy(gameObject);
    }
}
