public class NPCbattleManager : BattleManager
{
	public static NPCbattleManager npcinstance;

	protected override void Start()
	{
		npcinstance = this;
		sceneName = "NPC場地";
		pos = -260;
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
		StartCoroutine(GetCard(card,NPCdeckmanager.npcinstance,-126,-70));
	}

}
