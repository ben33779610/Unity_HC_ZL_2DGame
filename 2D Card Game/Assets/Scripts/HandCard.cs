using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Transform scene; //場地
	public string sceneName;//場地名稱
	public float pos;		//條件位置

	private Vector3 origin;
	private RectTransform rect;

	private bool inscene;
	private int crystalcost;

	private void Start()
	{
		rect = GetComponent<RectTransform>();

		scene = GameObject.Find(sceneName).transform;

		crystalcost = int.Parse(transform.Find("消耗").GetComponent<Text>().text);
		
	}


	public void OnBeginDrag(PointerEventData eventData)
	{
		//原始座標
		origin = transform.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (inscene) return;
		//拖拉
		transform.position = eventData.position;
	}


	public void OnEndDrag(PointerEventData eventData)
	{
		bool con;

		if (sceneName.Contains("NPC")) con = rect.anchoredPosition.y <= pos;
		else con = rect.anchoredPosition.y >= pos;

		if (con )
		{
			CheckCrystal();
		}
		else
		{
			//回到原始座標
			transform.position = origin;
		}
	}

	private void CheckCrystal()
	{
		if (crystalcost <= BattleManager.instance.crystal)
		{
			inscene = true;
			transform.SetParent(scene);
			BattleManager.instance.crystal -= crystalcost;
			BattleManager.instance.UpdateCrystal();
			print(BattleManager.instance.crystal);
			
		}
		else
		{
			//回到原始座標
			transform.position = origin;
		}


	}
}
