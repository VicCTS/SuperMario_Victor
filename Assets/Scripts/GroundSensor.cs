using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    public bool isGrounded;
    public bool canDoubleJump = true;

    public float jumpDamage = 5;

    void Awake()
    {
        _rigidBody = GetComponentInParent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 3)
        {
            isGrounded = true;
            canDoubleJump = true;
        }
        else if(collider.gameObject.layer == 6)
        {
            _rigidBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            Enemy _enemyScript = collider.gameObject.GetComponent<Enemy>();
            _enemyScript.TakeDamage(jumpDamage);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 3)
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 3)
        {
            isGrounded = false;
        }
    }
}
