using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
	/// <summary>
	/// 實體化BattleManager
	/// </summary>
	public static BattleManager instance;
	[Header("手牌資料")]
	public List<CardData> battleDeck = new List<CardData>();
	[Header("手牌遊戲物件")]
	public List<GameObject> Handgameobject = new List<GameObject>();
	[Header("硬幣")]
	public Rigidbody coin;
	[Header("遊戲畫面")]
	public GameObject gameview;
	[Header("是否先攻")]
	public bool firstattack;
	[Header("水晶數量")]
	public int crystal ;
	[Header("水晶數量介面")]
	public Text crstaltext;
	[Header("水晶物件"),Tooltip("水晶圖片,用來顯示的10張")]
	public GameObject[] crystalobject;
	[Header("擲硬幣畫面")]
	public GameObject coinview;

	private int crystalTotal;
	private bool myturn;

	private Transform canvas;
	private Transform hand;

	public int handcardcount; //手牌數量

	private void Awake()
	{
		instance = this;
		canvas = GameObject.Find("畫布").GetComponent<Transform>();
		hand = GameObject.Find("手牌區域").GetComponent<Transform>();
		
	}

	public void StartBattle()
	{
		gameview.SetActive(true); //顯示遊戲畫面
		ThrowCoin();
		
	}
	/// <summary>
	/// 結束回合
	/// </summary>
	public void EndTurn()
	{
		myturn = false;
	}
	/// <summary>
	/// 開始回合
	/// </summary>
	public void StartTurn()
	{
		myturn = true;
		crystalTotal++;
		crystalTotal = Mathf.Clamp(crystalTotal, 1, 10);
		crystal = crystalTotal;
		Crystal();
		StartCoroutine(GetCard(1));
		
	}

	/// <summary>
	/// 擲硬幣
	/// </summary>
	private void ThrowCoin()
	{
		coin.AddForce(0, Random.Range(200, 300), 0);
		coin.maxAngularVelocity = 50;
		coin.AddTorque(Random.Range(50, 120), 0, 0);

		Invoke("CheckCoin",2);
		

	}
	/// <summary>
	/// 判斷硬幣正反面
	/// rotation.x =0 正面
	/// rotation.x = 1 || -1 反面
	/// </summary>
	private void CheckCoin()
	{
		

		firstattack = coin.transform.GetChild(0).position.y > 0.1 ? true : false;

		
		coinview.SetActive(false);

		
		int card = 4;
		
		if (firstattack)
		{
			crystalTotal = 1;
			crystal = crystalTotal;
			card = 3;
		}
		
		Crystal();
		StartCoroutine(GetCard(card));
	}
	/// <summary>
	/// 取得手牌
	/// </summary>
	private IEnumerator GetCard(int count)
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < count; i++)
		{
			battleDeck.Add(DeckManager.instance.Deck[0]);
			DeckManager.instance.Deck.RemoveAt(0);
			Handgameobject.Add(DeckManager.instance.Deckgameobject[0]);
			DeckManager.instance.Deckgameobject.RemoveAt(0);
			yield return StartCoroutine(MoveCard());

		}
	}

	private IEnumerator MoveCard()
	{
		RectTransform card = Handgameobject[Handgameobject.Count-1].GetComponent<RectTransform>();

		card.SetParent(canvas);
		card.anchorMin = Vector2.one * 0.5f;
		card.anchorMax = Vector2.one * 0.5f;
		card.localScale = Vector3.one * 1.5f;
		while (card.anchoredPosition.x > 501)
		{
			card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(500, 0), 0.5f * Time.deltaTime * 50);
			yield return null;
		}
		
		yield return new WaitForSeconds(0.2f);

		//爆牌
		if (handcardcount == 10)
		{
			card.GetChild(1).GetComponent<Image>().material = Instantiate(card.GetChild(1).GetComponent<Image>().material);
			card.GetChild(0).GetComponent<Image>().material = Instantiate(card.GetChild(0).GetComponent<Image>().material);

			Material m = card.GetChild(1).GetComponent<Image>().material;
			Material m0 = card.GetChild(0).GetComponent<Image>().material;
			m.SetFloat("switch",1);
			m0.SetFloat("switch",1);
			float a = 0;

			Text[] texts = card.GetComponentsInChildren<Text>();
			for (int i = 0; i < texts.Length; i++) texts[i].enabled = false;

			while (m.GetFloat("alphaclip") < 4)
			{
				a += 0.1f;
				m.SetFloat("alphaclip", a);
				m0.SetFloat("alphaclip", a);
				yield return null;
			}
			Destroy(card.gameObject);
			battleDeck.RemoveAt(battleDeck.Count - 1);
			Handgameobject.RemoveAt(battleDeck.Count - 1);

		}
		else
		{
			card.localScale = Vector3.one * 0.5f;
			while (card.anchoredPosition.y > -450)
			{
				card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(0, -451), 0.5f * Time.deltaTime * 50);
				yield return null;
			}

			card.SetParent(hand);
			card.gameObject.AddComponent<HandCard>();
			handcardcount++;
		}
	}

	/// <summary>
	/// 回合開始時更新水晶
	/// </summary>
	private void Crystal()
	{
		for (int i = 0; i < crystal; i++)
		{
			crystalobject[i].SetActive(true);
		}
		crstaltext.text = crystal + "/ 10";
	}

	/// <summary>
	/// 使用卡牌時更新水晶數量
	/// </summary>
	public void UpdateCrystal()
	{
		for (int i = 0; i < crystalobject.Length; i++)
		{
			if (i < crystal) continue;      //如果i < 水晶數量  就跳過此次

			crystalobject[i].SetActive(false);
		}
		crstaltext.text = crystal + "/ 10";
	}

}
