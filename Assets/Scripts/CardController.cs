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

    public List<Card> allCards; // ��� ī��
    public GameObject[] cardPrefab; //ȭ�鿡 ��� ī�� â 3��
    public GameObject cardSee; //ȭ�鿡 ���â ���� ������ ����
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
        cards.Clear(); //���� ī�� �ʱ�ȣ���ϰ�
        CardPack(allCards);//��ī�� �̾Ƽ� cardPrefab�� �ְ�
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
        // databaseCards���� �������� 3�� �̱�
        List<Card> temp = new List<Card>(databaseCards);
        for (int i = 0; i < 3; i++)
        {
            if (temp.Count == 0) break; // ���� ī�� ������ ����
            int index = Random.Range(0, temp.Count);
            cards.Add(temp[index]);
            temp.RemoveAt(index); // �ߺ� ����
        }
    }

    public void CardChoice(int cardnumber)
    {
        Debug.Log(cards[cardnumber].cardName + " ����");
        Debug.Log(cards[cardnumber].CardNumber + " ����");
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
                Debug.Log("��ȭ��");
                player.SelectArrow(1,20);
                break;
            case 4:
                Debug.Log("����ȭ��");
                player.SelectArrow(2, 20);
                break;
            case 5:
                Debug.Log("���");
                player.Skill1();
                break;
            case 6:
                Debug.Log("����");
                player.Sheild();
                break;
            case 7:
                Debug.Log("���׿�");
                player.Skill3();
                break;
            default:
                break;
        }
    }
}
