using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public int maxHP = 300;
    public int currentHP;
    public Image hpBar; // Canvas의 HP바 이미지

    private float originalSpeed = 2f;
    private float currentSpeed;

    // 공격 관련 변수
    public GameObject player;
    public Transform firePos;
    public GameObject arrowPrefab;

    public GameObject fireEffect;
    public GameObject iceEffect;

    public GameObject winUI;

    public GameObject arrowPrefabs2;
    public GameObject range;

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        currentHP = maxHP;
        UpdateHPUI();
        StartCoroutine(shooLoop());
        fireEffect.SetActive(false);
        iceEffect.SetActive(false);
    }

    IEnumerator shooLoop()
    {
        while (true)
        {
            Shot();
            yield return new WaitForSeconds(1f); // 2초마다 발사
        }
    }

    private int count = 0;
    public void Shot()
    {
        //Debug.Log("Shot Arrow");
        count++;
        // 발사 위치 기준 방향 벡터
        Vector2 direction = ((Vector2)player.transform.position - (Vector2)firePos.position).normalized;
        direction.y += 0.5f;
        direction = direction.normalized; // 다시 정규화

        if(count % 5 == 0)
        {
            StartCoroutine(SkillRoutine());
        }
        GameObject arrow = Instantiate(arrowPrefab, firePos.position, Quaternion.identity);
        // 초기 속도 계산 (힘 조절 가능)
        arrow.GetComponent<Rigidbody2D>().velocity = direction * 15;
    }


    public void enemyfire2()
    {
        
        Instantiate(arrowPrefabs2, firePos.position, Quaternion.identity);
    }

    IEnumerator SkillRoutine()
    {
        // range 오브젝트 알파값 조절용
        SpriteRenderer sr = range.GetComponent<SpriteRenderer>();

        Player playerScript = player.GetComponent<Player>();

        for (int i = 0; i < 1; i++)
        {
            yield return StartCoroutine(playerScript.Fade(sr, 0f, 1f, 0.5f));
            yield return StartCoroutine(playerScript.Fade(sr, 1f, 0f, 0.5f));
        }
        anim.SetTrigger("isHit");

        //fire2();
    }

    public void TakeDamage(int amount)
    {
        anim.SetTrigger("isAttack");
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPUI();

        if (currentHP <= 0)
        {
            winUI.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
        }
    }
    void UpdateHPUI()
    {
        hpBar.fillAmount = (float)currentHP / maxHP;
    }


    public void Burn()
    {
        StartCoroutine(BurnCoroutine());
    }
    public void Slow()
    {
        StartCoroutine(SlowCoroutine());
    }

    IEnumerator BurnCoroutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float elapsed = 0;
        while (elapsed < 3)
        {
            TakeDamage(5);
            sr.color = Color.red;
            fireEffect.SetActive(true);

            float t = 0f;
            while (t < 0.2f)
            {
                sr.color = Color.Lerp(Color.red, Color.white, t / 0.2f);
                t += Time.deltaTime;
                yield return null;
            }

            sr.color = Color.white;
            fireEffect.SetActive(false);

            elapsed += 1f;
            yield return new WaitForSeconds(0.8f);
        }
    }
    


    IEnumerator SlowCoroutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.color = Color.blue;
        iceEffect.SetActive(true);

        currentSpeed = 0f;
        yield return new WaitForSeconds(1f);
        currentSpeed = originalSpeed;

        sr.color = Color.white;
        iceEffect.SetActive(false);

    }


}
