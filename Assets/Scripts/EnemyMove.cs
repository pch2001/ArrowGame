using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 3f;
    private int moveDirection = 1;

    private float changeDirectionTime = 0f; // 방향 바꿀 시간
    private float timer = 0f;
    void Start()
    {
        SetRandomTime();
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);

        // 일정 시간이 지나면 랜덤하게 방향 변경
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
