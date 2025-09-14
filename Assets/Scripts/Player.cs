using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{

    [SerializeField]

    //==================================이동 관련 변수
    public Vector2 inputVec;
    public float moveSpeed = 50f;
    public float jumpForce = 10f;
    private bool isGrounded;          // 땅 체크


    //================================== 공격 관련 변수
    public float yplus = 0.3f; //화살 발사 시 y축 추가값
    public float force = 10f; // 화살 힘

    public Transform firePos;
    public Transform meteoPos;
    public GameObject meteorobj;

    public GameObject[] arrowPrefabs; // 0: 일반, 1: 불, 2: 얼음

    [Header("UI")]
    public Image arrowImage;       // 현재 화살 이미지
    public Text arrowCountText;    // 남은 화살 수 텍스트
    [Header("화살 갯수")]
    public int[] arrowCount = {999, 50, 30 };
    [Header("화살 이미지")]
    public Sprite[] arrowSprite;

    [Header("쿨타임 설정")]
    public float cooldownTime1 = 2f; // 2초 쿨타임
    public float cooldownTime2 = 5f; // 5초 쿨타임
    public float cooldownTime3 = 10f; // 10초 쿨타임

    private bool isCooldown1 = false;
    private bool isCooldown2 = false;
    private bool isCooldown3 = false;

    public Image cooldownImage1; 
    public Image cooldownImage2;
    public Image cooldownImage3;

    private float cooldownTimer = 0f;


    public GameObject arrowPrefabs2;

    public GameObject range;

    public GameObject shieldobj;
    private int currentMode;

    public Image white;
    public Image black;
    

    //================================== 체력
    public int maxHP = 300;
    public int currentHP;

    public Image hpBar;

    //================================== 내부 변수
    private Rigidbody2D rb;
    private Animator animator;
    private Camera maincamera;
    private Collider2D col;

    public GameObject LoseUI;

    private bool isMove = true; // 필살기 공격 쓸때 ㅇ못움직이게하기
    void Start()
    {
        isMove = true;
        currentMode = 0;
        isGrounded = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        maincamera = Camera.main;
        col = this.GetComponent<Collider2D>();

        currentHP = maxHP;
        UpdateHPUI();
        UpdateArrowUI();

        // 스킬 쿨타임 0으로 초기회ㅏㅣ
        if(cooldownImage1) cooldownImage1.fillAmount = 0;
        if (cooldownImage2) cooldownImage2.fillAmount = 0;
        if (cooldownImage3) cooldownImage3.fillAmount = 0;
    }
    void Update()
    {
        rb.velocity = new Vector2(inputVec.x * moveSpeed, rb.velocity.y);


        animator.SetFloat("speed", Mathf.Abs(inputVec.x)); // 좌우 이동량만 전달


        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
        }
    }


    public void Shot()
    {
        if (arrowCount[currentMode] <= 0)
        {
            Debug.Log("화살이 없습니다!");
            currentMode = 0; // 일반 화살로 변경
            UpdateArrowUI();
            return;

        }
        else
        {
            arrowCount[currentMode]--;
            UpdateArrowUI();
        }

        //Debug.Log("Shot Arrow");
        Vector2 mousePos = maincamera.ScreenToWorldPoint(Input.mousePosition);

        // 발사 위치 기준 방향 벡터
        Vector2 direction = (mousePos - (Vector2)firePos.position).normalized;
        direction.y += yplus;
        direction = direction.normalized; // 다시 정규화

        GameObject arrow = Instantiate(arrowPrefabs[currentMode], firePos.position, Quaternion.identity);
        // 초기 속도 계산 (힘 조절 가능)
        arrow.GetComponent<Rigidbody2D>().velocity = direction * force;
    }


    public void ChangeArrow()
    {
        //Debug.Log("Change Arrow");
        currentMode++;
        if ((int)currentMode >= arrowPrefabs.Length)
        {
            currentMode = 0;
        }

        UpdateArrowUI();
    }

    public void SelectArrow(int arrownum, int arrowCount) //num이 화살 종류, count가 화살 갯수
    {

        currentMode = arrownum;
        this.arrowCount[arrownum] = arrowCount; //갯수 추가
        UpdateArrowUI();
    }

    public void UpdateArrowUI()
    {
        if (currentMode == 0)
        {
            arrowImage.sprite = arrowSprite[currentMode];
            arrowCountText.text = "X " + arrowCount[currentMode];
        }
        else if (currentMode == 1)
        {
            arrowImage.sprite = arrowSprite[currentMode];
            arrowCountText.text = "X " + arrowCount[currentMode];
        }
        else if (currentMode == 2)
        {
            arrowImage.sprite = arrowSprite[currentMode];
            arrowCountText.text = "X " + arrowCount[currentMode];
        }
    }
    private IEnumerator CooldownRoutine(float cooldownTime, Image cooldownImage, int skillIndex)
    {
        switch (skillIndex)
        {
            case 1: isCooldown1 = true; break;
            case 2: isCooldown2 = true; break;
            case 3: isCooldown3 = true; break;
        }

        float timer = 0f;
        if (cooldownImage) cooldownImage.fillAmount = 1;

        while (timer < cooldownTime)
        {
            timer += Time.deltaTime;
            if (cooldownImage)
                cooldownImage.fillAmount = 1 - (timer / cooldownTime);
            yield return null;
        }

        if (cooldownImage) cooldownImage.fillAmount = 0;
        switch (skillIndex)
        {
            case 1: isCooldown1 = false; break;
            case 2: isCooldown2 = false; break;
            case 3: isCooldown3 = false; break;
        }
    }
    public void Skill1()
    {
        if(isCooldown1) return; // 쿨타임 중이면 실행하지 않음
        StartCoroutine(CooldownRoutine(cooldownTime1, cooldownImage1, 1));

        StartCoroutine(SkillRoutine());
    }

    IEnumerator SkillRoutine()
    {
        // range 오브젝트 알파값 조절용
        SpriteRenderer sr = range.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 1; i++)
        {
            yield return StartCoroutine(Fade(sr, 0f, 1f, 0.5f));
            yield return StartCoroutine(Fade(sr, 1f, 0f, 0.5f));
        }
        animator.SetTrigger("isAttack2");

    }

    private void fire2()
    {
        CameraShake cameraShake = maincamera.GetComponent<CameraShake>();
        cameraShake.StartCoroutine(cameraShake.Shake(0.5f, 1f));
        Instantiate(arrowPrefabs2, firePos.position, Quaternion.identity);
    }

    public void Sheild()
    {
        if (isCooldown2) return; // 쿨타임 중이면 실행하지 않음

        StartCoroutine(CooldownRoutine(cooldownTime2, cooldownImage2, 2));

        shieldobj.SetActive(true);

        Sheild sheild = shieldobj.GetComponent<Sheild>();
        sheild.SpawnSheild();

    }

    public void Skill3()
    {
        if (isCooldown3) return; // 쿨타임 중이면 실행하지 않음
        StartCoroutine(Meteor());
        StartCoroutine(CooldownRoutine(cooldownTime3, cooldownImage3, 3));

    }

    public IEnumerator Meteor()
    {
        playerOnDisable();
        yield return StartCoroutine(FadeCanvasImage(black,0,1,2)); //어두워지고

        MP4Player mp4Player = FindObjectOfType<MP4Player>();
        mp4Player.PlayFromStreamingAssets("meteo.mp4");

        yield return new WaitForSeconds(5f);

        yield return StartCoroutine(FadeCanvasImage(black, 1, 0, 0.3f)); //밝아지고
        Instantiate(meteorobj, meteoPos.position, Quaternion.identity); //메토어 소환
        CameraShake cameraShake = maincamera.GetComponent<CameraShake>();
        cameraShake.StartCoroutine(cameraShake.Shake(3f, 0.5f)); //흔들리고
    }

    public IEnumerator MeteorEnd()
    {
        yield return StartCoroutine(FadeCanvasImage(white, 0, 1, 0.5f)); //밝아지고
        Enemy enemy = FindObjectOfType<Enemy>();
        enemy.TakeDamage(40);
        yield return StartCoroutine(FadeCanvasImage(white, 1, 0, 0.3f)); //복귀
        playerOnEnable();

    }


    private IEnumerator FadeCanvasImage(Image img, float from, float to, float time)
    {
        float elapsed = 0f;
        Color c = img.color;
        c.a = from;
        img.color = c;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, elapsed / time);
            img.color = c;
            yield return null;
        }

        c.a = to;
        img.color = c;
    }
    public IEnumerator Fade(SpriteRenderer sr, float start, float end, float duration) //스프라이트 값 일때
    {
        float time = 0;
        Color c = sr.color;

        while (time < duration)
        {
            float t = time / duration;
            c.a = Mathf.Lerp(start, end, t);
            sr.color = c;
            time += Time.deltaTime;
            yield return null;
        }

        c.a = end;
        sr.color = c;
    }

    public void TakeDamage(int amount)
    {
        CameraShake cameraShake = maincamera.GetComponent<CameraShake>();
        if (cameraShake != null) { 
            cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
        }

        animator.SetTrigger("isHit");

        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPUI();


        if (currentHP <= 0)
        {
            animator.SetTrigger("isDie");
            this.enabled = false;
            LoseUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void UpdateHPUI()
    {
        hpBar.fillAmount = (float)currentHP / maxHP;
    }
    void OnMove(InputValue value)
    {
        if (!isMove) return;
        inputVec = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!isMove) return;
        if (value.isPressed && isGrounded)
        {
            isGrounded = false;
            Debug.Log("OnJump");    
            StartCoroutine(JumpAction());
        }
    }
    IEnumerator JumpAction()
    {
        animator.SetTrigger("isJump");
        yield return new WaitForSeconds(0.3f);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    }

    bool isAttacking = false;
    void OnFire(InputValue value)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() && isAttacking)
            return;
        isAttacking = true;
        Debug.Log("화살발사");
        if (value.isPressed)
        {
            animator.SetTrigger("isAttack");
        }
    }
    
    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
    }


    public void playerOnDisable()
    {
        col.enabled = false;

        isMove = false;
        rb.gravityScale = 0f;
        
    }
    public void playerOnEnable()
    {
        col.enabled = true;

        isMove = true;
        rb.gravityScale = 4f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            animator.SetTrigger("isJumpEnd");
           // Debug.Log("땅에 닿음");
            isGrounded = true; // 땅에 닿았을 때
        }
    }


}
