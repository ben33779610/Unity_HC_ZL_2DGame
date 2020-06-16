using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 卡牌管理器
/// </summary>
public class DeckManager : MonoBehaviour
{
	//將物件新增一個靜態物件
	/// <summary>
	/// 實體化DeckManager
	/// </summary>
	public static DeckManager instance;
	[Header("牌組資料")]
	public List<CardData> Deck = new List<CardData>();
	[Header("牌組遊戲物件")]
	public List<GameObject> Deckgameobject = new List<GameObject>();
	[Header("牌組資訊")]
	public GameObject deckinfo;
	[Header("套牌內容")]
	public Transform deckcontent;
	[Header("卡牌數量")]
	public Text Deckcount;
	[Header("開始按鈕")]
	public Button btnstart;
	[Header("洗牌後牌組內容")]
	public Transform content;


	private void Awake()
	{
		
		instance = this; //設為這個物件
		btnstart.interactable = false;
		btnstart.onClick.AddListener(StartBattle);
	}

	/// <summary>
	/// 新增卡牌
	/// </summary>
	public void AddCard(int index)
	{
		
		if (Deck.Count < 30)  //List的長度是count
		{
			//尋找要增加卡排在清單內的資料
			//=>黏巴達(Lambda c#7)
			//相同卡牌 = 牌組.尋找全部( 卡牌=>卡牌.等於(目前點選的卡牌資訊))
			List<CardData> sameCard =  Deck.FindAll(c => c.Equals(GetCard.instance.cards[index - 1]));
			if (sameCard.Count < 3)
			{
				//取得卡牌資訊
				CardData card = GetCard.instance.cards[index - 1];
				Transform temp;
				Deck.Add(GetCard.instance.cards[index - 1]);
				if (sameCard.Count < 1)
				{
					temp = Instantiate(deckinfo, deckcontent).transform;
					temp.gameObject.AddComponent<DeckObject>().index = card.index;
					temp.name = "牌組卡牌資訊 : " + card.name;
				}
				else
				{
					temp = GameObject.Find("牌組卡牌資訊 : " + card.name).transform;
				}
				//更新卡牌數量
				Deckcount.text = ("卡牌數量 " + Deck.Count + "/30");
				temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
				temp.Find("名稱").GetComponent<Text>().text = card.name.ToString();
				temp.Find("數量").GetComponent<Text>().text = (sameCard.Count+1).ToString();
			}
		}
		if (Deck.Count == 30)
		{
			btnstart.interactable = true;
		}
	}

	/// <summary>
	/// 刪除卡牌
	/// </summary>
	public void DeleteCard(int index)
	{
		//選取的卡牌
		CardData card = GetCard.instance.cards[index - 1];
		//相同卡牌 = 牌組.尋找全部( 卡牌=>卡牌.等於(目前點選的卡牌資訊))
		List<CardData> sameCard = Deck.FindAll(c => c.Equals(card));
		//移除卡牌
		Deck.Remove(card);

		Transform temp = GameObject.Find("牌組卡牌資訊 : " + card.name).transform;
		//相同卡牌>1
		if (sameCard.Count  > 1)
		{
			temp.Find("數量").GetComponent<Text>().text = (sameCard.Count-1).ToString();
		}
		else
		{
			Destroy(temp.gameObject);
		}
		Deckcount.text = ("卡牌數量 " + Deck.Count + "/30");
		btnstart.interactable = false;
	}

	/// <summary>
	/// 建立卡牌在洗牌區
	/// </summary>
	private void CreateCard()
	{
		for (int i = 0; i < Deck.Count; i++)
		{
			Transform temp = Instantiate(GetCard.instance.cardobject, content).transform;
			CardData card = Deck[i];
			temp.Find("名稱").GetComponent<Text>().text = card.name.ToString();
			temp.Find("描述").GetComponent<Text>().text = card.description.ToString();
			temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
			temp.Find("血量").GetComponent<Text>().text = card.hp.ToString();
			temp.Find("攻擊").GetComponent<Text>().text = card.attack.ToString();
			temp.Find("卡圖").GetComponent<Image>().sprite = Resources.Load<Sprite>(card.file);

			temp.gameObject.AddComponent<BookCard>().index = card.index;
			Deckgameobject.Add( temp.gameObject );
		}
	}

	/// <summary>
	/// 洗牌
	/// </summary>
	private void Shuffle()
	{
		for (int i = 0; i < Deck.Count; i++)
		{
			CardData original = Deck[i];
			int r =  Random.Range(0, Deck.Count);
			Deck[i] = Deck[r];
			Deck[r] = original;

		}
		CreateCard();
	}

	private void StartBattle()
	{
		
		Shuffle();
		BattleManager.instance.StartBattle();
	}

	private void Update()
	{
		
		ChooseCard();
	}
	private void ChooseCard()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 1; j <= 10; j++)
				{
					AddCard(j);
				}
			}
		}
	}

}
