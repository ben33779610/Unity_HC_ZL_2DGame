using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	/// <summary>
	/// 實體化BattleManager
	/// </summary>
	public static BattleManager instance;

	public List<CardData> battleDeck = new List<CardData>();
	[Header("硬幣")]
	public Rigidbody coin;
	[Header("遊戲畫面")]
	public GameObject gameview;
	[Header("是否先攻")]
	public bool firstattack;

	private void Awake()
	{
		instance = this;
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
		coin.AddForce(0, Random.Range(200, 250), 0);
		coin.maxAngularVelocity = 50;
		coin.AddTorque(Random.Range(50, 100), 0, 0, ForceMode.Force);

		Invoke("CheckCoin", 3);
	}
	/// <summary>
	/// 判斷硬幣正反面
	/// rotation.x =0 正面
	/// rotation.x = 1 || -1 反面
	/// </summary>
	private void CheckCoin()
	{
		print(coin.rotation.x);
		//firstattack = coin.rotation.x == 0 ? true : false;
		if (coin.rotation.x != 0)
		{
			firstattack = false;
		}
		else
			firstattack =  true;
		print(firstattack);
	}
	private void GetCard()
	{
		battleDeck.Add(DeckManager.instance.Deck[0]);
		DeckManager.instance.Deck.RemoveAt(0);
	}
}
