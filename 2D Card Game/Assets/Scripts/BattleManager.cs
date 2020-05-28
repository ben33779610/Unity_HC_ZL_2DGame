using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private Transform canvas;
	private Transform hand;

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
	/// 擲硬幣
	/// </summary>
	private void ThrowCoin()
	{
		coin.AddForce(0, Random.Range(200, 300), 0);
		coin.maxAngularVelocity = 50;
		coin.AddTorque(Random.Range(50, 120), 0, 0);

		CheckCoin();
		

	}
	/// <summary>
	/// 判斷硬幣正反面
	/// rotation.x =0 正面
	/// rotation.x = 1 || -1 反面
	/// </summary>
	private void CheckCoin()
	{
		
		firstattack = coin.transform.GetChild(0).position.y > 0.1 ? true : false;

		StartCoroutine(GetCard(3));
	}
	/// <summary>
	/// 取得手牌
	/// </summary>
	private IEnumerator GetCard(int count)
	{
		yield return new WaitForSeconds(3);
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
		
		yield return new WaitForSeconds(1f);

		card.localScale = Vector3.one * 0.5f;
		while (card.anchoredPosition.y > -450)
		{
			card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(0, -451), 0.5f * Time.deltaTime * 50);
			yield return null;
		}

		card.SetParent(hand);
		card.gameObject.AddComponent<HandCard>();
	}
}
