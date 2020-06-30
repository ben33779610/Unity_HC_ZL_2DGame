using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;        // 引用 UI 額外功能 API


public class AttackCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    private UILineRenderer line;     // UI 線條渲染
    private Transform arrow;        //箭頭
    private Vector2 posBegin,posEnd; //開始拖拉為志,結束拖拉位置
    private bool canatk;
    private static Transform parent;
    private static Transform target;

    private void Awake()
    {
        line = GameObject.Find("線條").GetComponent<UILineRenderer>();                  // 取得
        arrow = GameObject.Find("箭頭").transform;

        line.Points[0] = posBegin;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        line.enabled = true;
        //介面.x = 事件.x - 螢幕.寬度 / 2
        posBegin.x = eventData.position.x - Screen.width / 2;
        //介面.y = 事件.y - 螢幕.高度 / 2
        posBegin.y = eventData.position.y - Screen.height / 2;
        line.Points[1] = posEnd;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //介面.x = 事件.x - 螢幕.寬度 / 2
        posEnd.x = eventData.position.x - Screen.width / 2;
        //介面.y = 事件.y - 螢幕.高度 / 2
        posEnd.y = eventData.position.y - Screen.height / 2;

        line.Resoloution = (posEnd - posBegin).magnitude / 200;//解析度 = 拖拉點-起始點 /200    
        arrow.GetComponent<RectTransform>().anchoredPosition = posEnd; //箭頭座標 = 拖拉中位置
        arrow.up = posEnd - posBegin;
    }   

    public void OnEndDrag(PointerEventData eventData)
    {
        line.enabled = false;
        arrow.position = Vector2.one * 1000;
        if (canatk)
        {

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canatk = true;
        parent = transform.parent;
        target = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canatk = false;
        parent = null;
        target = null;
    }

    private IEnumerator Attack()
    {
        transform.parent.SetAsLastSibling();    //變形.付物件.設為最後一個子物件

        Vector3 pos = transform.position; //原始位置

        while (transform.position.y != target.position.y)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.9f * Time.deltaTime * 30);
            yield return new WaitForSeconds(0.5f); 
        }

        transform.position = pos;       //回到原始位置
    }
}
