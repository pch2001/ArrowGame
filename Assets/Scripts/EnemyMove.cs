using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 3f;
    private int moveDirection = 1;

    private float changeDirectionTime = 0f; // ���� �ٲ� �ð�
    private float timer = 0f;
    void Start()
    {
        SetRandomTime();
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);

        // ���� �ð��� ������ �����ϰ� ���� ����
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            moveDirection *= -1;
            SetRandomTime();
        }
    }
    void SetRandomTime()
    {
        changeDirectionTime = Random.Range(1f, 2f);
        timer = 0f;
    }

    public void slow()
    {
        StartCoroutine(StopMove());
    }

    IEnumerator StopMove()
    {
        speed = 0f;
        yield return new WaitForSeconds(1f);
        speed = 3;

    }
}
