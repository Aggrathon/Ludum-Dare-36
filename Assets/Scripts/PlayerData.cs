using System;

namespace aggrathon.ld36
{
	[Serializable]
	public class PlayerData
	{
		static PlayerData[] players;
		public static PlayerData[] Players
		{
			get {
				if (players == null)
				{
					players = new PlayerData[] {
						new PlayerData("Player 1", Controller.player),
						new PlayerData("Bot 1", Controller.ai),
						new PlayerData("Bot 2", Controller.ai),
						new PlayerData("Bot 3", Controller.ai),
						new PlayerData("Bot 4", Controller.ai),
						new PlayerData("Bot 5", Controller.ai),
						new PlayerData("Bot 6", Controller.ai),
						new PlayerData("Bot 7", Controller.ai)
					};
				}
				return players;
			}
			set { players = value; }
		}

		public enum Controller
		{
			ai,
			player,
			player1,
			player2
		}

		public string name;
		public Controller controller;
		public int[] upgrades;

		public PlayerData(string name, Controller cont, params int[] upgrades)
		{
			this.name = name;
			this.controller = cont;
			this.upgrades = upgrades;
		}

		public PlayerData() { }
	}
}
