using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
/// 取得卡牌資訊
/// </summary>
public class GetCard : MonoBehaviour
{
	public static GetCard instance;
	public CardData[] cards;   //卡牌
	[Header("卡牌物件")]
	public GameObject cardobject;    
	[Header("卡牌內容")]
	public Transform cardcontent;
	private CanvasGroup loadingPanel;
	private Image loading;

	private IEnumerator GetCardData()
	{
		loadingPanel.alpha = 1;
		loadingPanel.blocksRaycasts = true;


		//引用(網路要求 www = 網路要求.Post("網址","")
		using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyivbUKcqLsbwFCK_DkSVSdwaFDgGEVTjTCWhq6hyKEA5VUq5A/exec", ""))
		{
			//等待網路要求時間
			//yield return www.SendWebRequest();
			www.SendWebRequest();

			while (www.downloadProgress < 1)
			{
				yield return null;
				loading.fillAmount = www.downloadProgress;
			}

			if (www.isHttpError || www.isNetworkError)
			{
				print("連線錯誤:" + www.error);
			}
			else
			{
				cards = JsonHelper.FromJson<CardData>(www.downloadHandler.text);
				CreateCard();
			}


		}
		yield return new WaitForSeconds(0.5f);
		loadingPanel.alpha = 0;
		loadingPanel.blocksRaycasts = false;
	}


	private void Awake()
	{
		instance = this;
		loadingPanel = GameObject.Find("載入畫面").GetComponent<CanvasGroup>();
		loading = GameObject.Find("進度條").GetComponent<Image>();
	}

	private void Start()
	{
		StartCoroutine(GetCardData());
	}
	/// <summary>
	/// 卡片資料
	/// </summary>
	/// 序列化:讓資料可以顯示屬性面板上
	


	private void CreateCard()
	{
		for (int i = 0; i < cards.Length; i++)
		{
			Transform temp =  Instantiate(cardobject, cardcontent).transform;
			CardData card = cards[i];
			temp.Find("名稱").GetComponent<Text>().text = card.name.ToString();
			temp.Find("描述").GetComponent<Text>().text = card.description.ToString();
			temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
			temp.Find("血量").GetComponent<Text>().text = card.hp.ToString();
			temp.Find("攻擊").GetComponent<Text>().text = card.attack.ToString();
			temp.Find("卡圖").GetComponent<Image>().sprite = Resources.Load<Sprite>(card.file);

			temp.gameObject.AddComponent<BookCard>().index = card.index;
		}
	}

	public static class JsonHelper
	{
		public static T[] FromJson<T>(string json)
		{
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		[System.Serializable]
		private class Wrapper<T>
		{
			public T[] Items;
		}
	}
}

[System.Serializable]
public class CardData
{
	public int index;
	public string name;
	public int cost;
	public int hp;
	public int attack;
	public string description;
	public string file;
}
