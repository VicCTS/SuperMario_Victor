using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    public bool isGrounded;

    void Awake()
    {
        _rigidBody = GetComponentInParent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 3)
        {
            isGrounded = true;
            Debug.Log(collider.gameObject.name);
            Debug.Log(collider.gameObject.transform.position);
        }
        else if(collider.gameObject.layer == 6)
        {
            _rigidBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            Enemy _enemyScript = collider.gameObject.GetComponent<Enemy>();
            _enemyScript.Death();
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
