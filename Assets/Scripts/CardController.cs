using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [System.Serializable]
    public class Card
    {
        public string cardName;
        public int CardNumber;
        public Sprite cardImage;
    }

    public List<Card> allCards; // 모든 카드
    public GameObject[] cardPrefab; //화면에 띄울 카드 창 3개
    public GameObject cardSee; //화면에 띄울창 눈에 보일지 말지
    void Start()
    {
        cardSee.SetActive(false);
        StartCoroutine(startGame());
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    IEnumerator startGame()
    {
        yield return new WaitForSeconds(5f);

        cardSee.SetActive(true);
        cards.Clear(); //기존 카드 초기호ㅓ하고
        CardPack(allCards);//새카드 뽑아서 cardPrefab에 넣고
        for (int i = 0; i < cardPrefab.Length; i++)
        {
            Image img = cardPrefab[i].GetComponent<Image>();
            img.sprite = cards[i].cardImage;

        }
        Time.timeScale = 0f;
        yield return new WaitForSeconds(5f);
        StartCoroutine(startGame());
    }


    public List<Card> cards = new List<Card>();

    public void CardPack(List<Card> databaseCards)
    {
        // databaseCards에서 랜덤으로 3장 뽑기
        List<Card> temp = new List<Card>(databaseCards);
        for (int i = 0; i < 3; i++)
        {
            if (temp.Count == 0) break; // 남은 카드 없으면 종료
            int index = Random.Range(0, temp.Count);
            cards.Add(temp[index]);
            temp.RemoveAt(index); // 중복 방지
        }
    }

    public void CardChoice(int cardnumber)
    {
        Debug.Log(cards[cardnumber].cardName + " 선택");
        Debug.Log(cards[cardnumber].CardNumber + " 선택");
        GetAbillity(cards[cardnumber].CardNumber);
        cardSee.SetActive(false);
        Time.timeScale = 1f;

    }

    public void GetAbillity(int cardnumber)
    {
        Player player = FindObjectOfType<Player>();

        switch (cardnumber)
        {
            case 0:
                Debug.Log("HP Up");
                player.currentHP += 10;
                player.UpdateHPUI();
                
                break;
            case 1:
                Debug.Log("Jump Force Up");
                player.jumpForce += 5f;
                break;
            case 2:
                Debug.Log("Move Speed Up");
                player.moveSpeed += 5f;
                break; 
            case 3:
                Debug.Log("불화살");
                player.SelectArrow(1,20);
                break;
            case 4:
                Debug.Log("얼음화살");
                player.SelectArrow(2, 20);
                break;
            case 5:
                Debug.Log("에어샷");
                player.Skill1();
                break;
            case 6:
                Debug.Log("쉴드");
                player.Sheild();
                break;
            case 7:
                Debug.Log("메테오");
                player.Skill3();
                break;
            default:
                break;
        }
    }
}
