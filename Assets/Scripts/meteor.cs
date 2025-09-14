using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteor : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = enemies[0].gameObject.transform;
    }

    void Update()
    {
       

        transform.position = Vector2.MoveTowards(transform.position, target.position, 5 * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            CameraShake cameraShake = FindObjectOfType<CameraShake>();
            cameraShake.StartCoroutine(cameraShake.Shake(1f, 5f)); //Èçµé¸®°í

            Player player = FindObjectOfType<Player>();
            player.StartCoroutine("MeteorEnd");

            Destroy(gameObject);
        }
    }

}
