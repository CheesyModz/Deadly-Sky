using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;

    public Vector2 currentDirection;

    private  float velocity = 40f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Fly();
    }

    void Fly()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + currentDirection * velocity * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage(1);
            break;

            case "Enemy":
            other.gameObject.GetComponent<Enemy>().TakeDamage(1);
            break;

            default:
            break;
        }

        Destroy(gameObject);
    }
}
