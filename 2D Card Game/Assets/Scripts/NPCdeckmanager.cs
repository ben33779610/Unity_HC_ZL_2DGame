public class NPCdeckmanager : DeckManager
{
	public static NPCdeckmanager npcinstance;

	protected override void Awake()
	{
		npcinstance = this;
		btnstart.onClick.AddListener(ChooseCard);
	}

	protected override void Update()
	{
		
	}

	protected override void ChooseCard()
	{
		base.ChooseCard();
		Invoke("Shuffle", 1);
	}

}
