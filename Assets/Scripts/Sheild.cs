using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("enemyAttack"))
        //{
        //    Destroy(collision.gameObject);
        //    StartCoroutine(Hide());
        //}
    
    }

    public void SpawnSheild()
    {
        StartCoroutine(Hide());
    }
    IEnumerator Hide()
    {
        yield return new WaitForSeconds(4f);
        this.gameObject.SetActive(false);
    }

}
