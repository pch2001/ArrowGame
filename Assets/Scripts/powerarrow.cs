using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class powerarrow : MonoBehaviour
{
    private Rigidbody2D rb;

    public GameObject boomEffect;

    public bool isEnemy = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
        if(isEnemy)
            rb.velocity = -transform.right * 30;
        else
            rb.velocity = transform.right * 30;

        // 5초 뒤 자동 삭제
        Destroy(gameObject, 5f);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(boomEffect, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Enemy>().TakeDamage(20);

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(boomEffect, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Player>().TakeDamage(20);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {

            Destroy(gameObject);
        }
    }
}
