using UnityEngine;
using UnityEngine.UI;

public class DeckObject : MonoBehaviour
{
	public int index;

	private void Start()
	{
		transform.Find("增加").GetComponent<Button>().onClick.AddListener(AddCard);
		transform.Find("減少").GetComponent<Button>().onClick.AddListener(DeleteCard);
	}

	public void AddCard()
	{
		DeckManager.instance.AddCard(index);
	}
	public void DeleteCard()
	{
		DeckManager.instance.DeleteCard(index);
	}
}
