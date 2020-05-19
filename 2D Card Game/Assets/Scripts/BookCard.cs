using UnityEngine;
using UnityEngine.UI;


public class BookCard : MonoBehaviour
{
	public int index;

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(ChooseBookCard);
	}

	private void ChooseBookCard()
	{
		DeckManager.instance.AddCard(index);
	}
}
