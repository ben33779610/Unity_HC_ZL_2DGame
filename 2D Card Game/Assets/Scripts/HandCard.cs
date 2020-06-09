using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 origin;

	private RectTransform rect;
	private Transform scene;

	private bool inscene;
	private int crystalcost;

	private void Start()
	{
		rect = GetComponent<RectTransform>();

		scene = GameObject.Find("我方場地").transform;

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
		if (rect.anchoredPosition.y >= 70 && CheckCrystal())
		{

		}
		else
		{
			//回到原始座標
			transform.position = origin;
		}
	}

	private bool CheckCrystal()
	{
		if (crystalcost <= BattleManager.instance.crystal)
		{
			inscene = true;
			transform.SetParent(scene);
			BattleManager.instance.crystal -= crystalcost;
			BattleManager.instance.UpdateCrystal();
			print(BattleManager.instance.crystal);
			return true;
		}
		else
			return false;
	}
}
