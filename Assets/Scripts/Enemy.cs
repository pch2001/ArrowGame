using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public int maxHP = 300;
    public int currentHP;
    public Image hpBar; // Canvas�� HP�� �̹���

    private float originalSpeed = 2f;
    private float currentSpeed;

    // ���� ���� ����
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
            yield return new WaitForSeconds(1f); // 2�ʸ��� �߻�
        }
    }

    private int count = 0;
    public void Shot()
    {
        //Debug.Log("Shot Arrow");
        count++;
        // �߻� ��ġ ���� ���� ����
        Vector2 direction = ((Vector2)player.transform.position - (Vector2)firePos.position).normalized;
        direction.y += 0.5f;
        direction = direction.normalized; // �ٽ� ����ȭ

        if(count % 5 == 0)
        {
            StartCoroutine(SkillRoutine());
        }
        GameObject arrow = Instantiate(arrowPrefab, firePos.position, Quaternion.identity);
        // �ʱ� �ӵ� ��� (�� ���� ����)
        arrow.GetComponent<Rigidbody2D>().velocity = direction * 15;
    }


    public void enemyfire2()
    {
        
        Instantiate(arrowPrefabs2, firePos.position, Quaternion.identity);
    }

    IEnumerator SkillRoutine()
    {
        // range ������Ʈ ���İ� ������
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
            Time.timeScale = 0f; // ���� �Ͻ�����
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
