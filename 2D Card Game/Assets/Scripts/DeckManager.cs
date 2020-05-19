using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
	//將物件新增一個靜態物件
	public static DeckManager instance;
	public List<CardData> Deck = new List<CardData>();

	[Header("牌組資訊")]
	public GameObject deckinfo;
	[Header("套牌內容")]
	public Transform deckcontent;
	[Header("卡牌數量")]
	public Text Deckcount;

	private void Awake()
	{
		//設為這個物件
		instance = this;
	}

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
	}

	public void DeleteCard()
	{

	}

}
