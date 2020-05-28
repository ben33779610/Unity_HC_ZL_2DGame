using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{
	private Vector3 origin;

	public void OnBeginDrag(PointerEventData eventData)
	{
		//原始座標
		origin = transform.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		//拖拉
		transform.position = eventData.position;
	}

	public void OnDrop(PointerEventData eventData)
	{
		//回到原始座標
		transform.position = origin;
	}

}
