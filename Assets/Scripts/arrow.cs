using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public string mode;
    public GameObject fireEffect;


    private void Start()
    {
        //Debug.Log("Arrow Fired");
        Destroy(gameObject, 5f);
    }



    void Update()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log("충돌한 오브젝트 태그: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
       else if (collision.gameObject.CompareTag("Shield"))

        {
           // Debug.Log("Sheild Hit");
            collision.gameObject.SetActive(false);

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy Hit");
            Instantiate(fireEffect, transform.position, Quaternion.identity);
            ApplyEffect(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player Hit");
            Instantiate(fireEffect, transform.position, Quaternion.identity);

            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(10);
            Destroy(gameObject);
        }
    }

        void ApplyEffect(GameObject target)
        {
            switch (mode)
            {
                case "normal":
                    target.GetComponent<Enemy>().TakeDamage(10);
                    break;
                case "fire":
                    target.GetComponent<Enemy>().TakeDamage(5);
                    target.GetComponent<Enemy>().Burn();
                    break;
                case "ice":
                    target.GetComponent<Enemy>().TakeDamage(5);
                    target.GetComponent<Enemy>().Slow();
                    target.GetComponent<EnemyMove>().slow();
                    break;
            }
        }
    
}

