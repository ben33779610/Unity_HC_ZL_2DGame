public class NPCbattleManager : BattleManager
{
	public static NPCbattleManager npcinstance;

	protected override void Awake()
	{
		npcinstance = this;
	}


	protected override void CheckCoin()
	{
		firstattack = !instance.firstattack;

		


		int card = 4;

		if (firstattack)
		{
			crystalTotal = 1;
			crystal = crystalTotal;
			card = 3;
		}

		Crystal();
		StartCoroutine(GetCard(card,NPCdeckmanager.npcinstance,200,275));
	}

}
